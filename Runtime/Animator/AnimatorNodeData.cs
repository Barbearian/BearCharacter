using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear {
    public class AnimatorNodeData : INodeData, IOnAttachedToNode, IOnDetachedFromNode,IAnimatorClipsPlayer, IAnimatorNode
    {
        public Animator anim;
        public SafeDelegate<PlayAnimationClipInfo> dOnPlayedAnimation = new SafeDelegate<PlayAnimationClipInfo>();
        public SafeDelegate<int> dOnPlayedIndexed = new SafeDelegate<int>();
        public SafeDelegate dOnEnterDefaule = new SafeDelegate();
        public AnimationClipNodeData clipData;
        public Animator Anim { get => anim; set => anim = value; }
        public SafeDelegate<PlayAnimationClipInfo> DOnPlayedAnimation { get => dOnPlayedAnimation; set => dOnPlayedAnimation = value; }
        public SafeDelegate<int> DOnPlayedIndexed { get => dOnPlayedIndexed; set => dOnPlayedIndexed = value; }
        public SafeDelegate DOnEnterDefaule { get => dOnEnterDefaule; set => dOnEnterDefaule = value; }
        public AnimationClipNodeData ClipData { get => clipData; set => clipData = value; }

        public void Attached(INode node)
        {
            if (node is NodeView view) { 
                anim = view.GetComponent<Animator>();
            }
        }

        public void Detached(INode node)
        {
            anim = null;
        }

        public void Play(int index)
        {
            AnimatorNodeSystem.Play(this,index);
        }
    }
}