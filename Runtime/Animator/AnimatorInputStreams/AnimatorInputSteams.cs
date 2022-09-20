using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    public class AnimatorMovementSpeedInputStreamReceiverNodeData:INodeData,IAnimatorMovementSpeedInputStreamReceiver,IOnAttachedToNode{
        

        public float maxSpeedBlend;
        public string SpeedAttribute;
        public string SpeedMultiAttribute;
        public SafeDelegate<float> DUpdateSpeed = new SafeDelegate<float>();
        public SafeDelegate<float> DUpdateSpeedMulti = new SafeDelegate<float>();

        public float MaxSpeedBlend => maxSpeedBlend;

        public Dictionary<AnimatorNodeView,System.Action> links = new Dictionary<AnimatorNodeView, Action>();
        public void OnInputStreamLinked(IInputStreamSender receiver){}

        public void Attached(INode node){
            if(node is AnimatorNodeView nodeview){
                this.Link(nodeview);
            }
        }

        public void UpdateSpeed(float speed)
        {
            DUpdateSpeed.invoker?.Invoke(speed);
           // DUpdateSpeed?.Invoke(speed);
        }

        public void UpdateSpeedMulti(float speedMulti)
        {
            DUpdateSpeedMulti.invoker?.Invoke(speedMulti);
        }
    }


    public interface IAnimatorMovementSpeedInputStreamReceiver{
        float MaxSpeedBlend{get;}
        void UpdateSpeed(float speed);
        void UpdateSpeedMulti(float speedMulti);
    }

    public static class IAnimatorMovementSpeedInputStreamReceiverSystem{
        public static void UpdateSpeedAndMulti(this IAnimatorMovementSpeedInputStreamReceiver receiver,float speed){
            float multi = 1;
            if(speed>=receiver.MaxSpeedBlend && receiver.MaxSpeedBlend>0){
                speed = receiver.MaxSpeedBlend;
                multi = speed/receiver.MaxSpeedBlend;
            }

            receiver.UpdateSpeed(speed);
            receiver.UpdateSpeedMulti(multi);
        }

        public static void Link(this AnimatorMovementSpeedInputStreamReceiverNodeData nodedata, AnimatorNodeView view){
                

                var Delink = nodedata.DUpdateSpeed.Link<float>((x)=>{
                    view.SetFloat(nodedata.SpeedAttribute,x);
                });
                 
                Delink+=nodedata.DUpdateSpeedMulti.Link<float>((x)=>{
                    view.SetFloat(nodedata.SpeedMultiAttribute,x);
                });

                nodedata.Release(view);
                nodedata.links[view] = Delink;
        }

        public static void Release(this AnimatorMovementSpeedInputStreamReceiverNodeData nodedata, AnimatorNodeView view){
            if(nodedata.links.TryGetValue(view,out var Delink)){
                try{
                    Delink.Invoke();
                }catch(Exception e){
                    Debug.LogWarning(e);
                }

                nodedata.links.Remove(view);
            }

        }        



       
    }
}