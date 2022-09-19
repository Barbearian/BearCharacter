using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    public class AnimatorMovementSpeedInputStreamReceiverNodeData:INodeData,IAnimatorMovementSpeedInputStreamReceiver,IOnAttachedToNode{
        public float maxSpeedBlend;
        public string SpeedAttribute;
        public string SpeedMultiAttribute;
        private Action<float> DUpdateSpeed;
        private Action<float> DUpdateSpeedMulti;

        public float MaxSpeedBlend => maxSpeedBlend;

        public void OnInputStreamLinked(IInputStreamSender receiver){}

        public void Attached(INode node){
            if(node is AnimatorNodeView nodeview){
                DUpdateSpeed = (x)=>{
                    nodeview.SetFloat(SpeedAttribute,x);
                };

                DUpdateSpeedMulti = (x)=>{
                    nodeview.SetFloat(SpeedMultiAttribute,x);
                };
            }
        }

        public void UpdateSpeed(float speed)
        {
            DUpdateSpeed?.Invoke(speed);
        }

        public void UpdateSpeedMulti(float speedMulti)
        {
            DUpdateSpeedMulti?.Invoke(speedMulti);
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
    }
}