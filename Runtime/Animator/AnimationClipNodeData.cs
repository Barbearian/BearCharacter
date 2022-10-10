using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Bear
{
    [System.Serializable]
	public class AnimationClipNodeData:INodeData
    {
        public PlayAnimationClipInfo EntryClip;
        public PlayAnimationClipInfo[] defaultClips;
    }

    [System.Serializable]
    public struct PlayAnimationClipInfo{
        public string clipName;
        public int layer;
        public float mixedTime;

        public static PlayAnimationClipInfo Create(string clipName, int layer = 0, float mixedTime = 0){
            return new PlayAnimationClipInfo(){
                clipName = clipName,
                layer = layer,
                mixedTime = mixedTime
            };
        }
    }
}


