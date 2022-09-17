
using UnityEngine;

namespace Bear{
    public class AnimatorNodeView : NodeView
    {
        public Animator anim;
        public System.Action<PlayAnimationClipInfo> DOnPlayedAnimation;

        public AnimationClipNodeData clipData;
        
    }

    public static class AnimatorNodeViewSystem{
        public static void Play(this AnimatorNodeView view, string clipName, int layer = 0, float mixedTime = 0){
            PlayAnimationClipInfo info = PlayAnimationClipInfo.Create(clipName,layer,mixedTime);
            view.Play(info);
        }

        public static void EnterDefaultState(this AnimatorNodeView view){
            var clip = view.clipData.EntryClip;
            view.Play(clip);
        }

        public static void Play(this AnimatorNodeView view,int index){
            if(index>=0 && index < view.clipData.defaultClips.Length){
                var clip = view.clipData.defaultClips[index];
                view.Play(clip);
            }
        }

        public static void Play(this AnimatorNodeView view, PlayAnimationClipInfo info){
            view.anim.Play(info.clipName,info.layer,info.mixedTime);
            view.DOnPlayedAnimation?.Invoke(info);
        }
    }

    


}