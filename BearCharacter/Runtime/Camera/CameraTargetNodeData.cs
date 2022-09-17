using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    [System.Serializable]
    public struct CameraTargetNodeData :INode
    {
        public Transform lookAt;
        public Transform follow;
        public System.Action<Transform> DSetLookAt;
        public System.Action<Transform> DSetFollow;
    }

    public static class CameraLookAtNodeDataSystem{
        public static void UpdateLookAt(ref this CameraTargetNodeData cand, Transform lookat){
            if(lookat != cand.lookAt){

                cand.lookAt = lookat;
                cand.DSetLookAt?.Invoke(lookat);
                
            }
        }

        public static void UpdateFollow(ref this CameraTargetNodeData cand, Transform follow){
            if(follow != cand.follow){
                cand.follow = follow;
                cand.DSetFollow?.Invoke(follow);        
            }
        }
    }
}