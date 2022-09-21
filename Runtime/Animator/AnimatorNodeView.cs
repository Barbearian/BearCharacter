
using UnityEngine;

namespace Bear{
    public class AnimatorNodeView : NodeView,IAnimatorClipsPlayer
    {
        public Animator anim;
        public SafeDelegate<PlayAnimationClipInfo> DOnPlayedAnimation = new SafeDelegate<PlayAnimationClipInfo>();
        public SafeDelegate<int> DOnPlayedIndexed  = new SafeDelegate<int>();
        public SafeDelegate DOnEnterDefaule = new SafeDelegate();
        public AnimationClipNodeData clipData;

        public void Play(int index){
            AnimatorNodeViewSystem.Play(this,index);

        }
        
    }

    public static class AnimatorNodeViewSystem{
        public static void Play(this AnimatorNodeView view, string clipName, int layer = 0, float mixedTime = 0){
            PlayAnimationClipInfo info = PlayAnimationClipInfo.Create(clipName,layer,mixedTime);
            view.Play(info);
        }

        public static void EnterDefaultState(this AnimatorNodeView view){
            var clip = view.clipData.EntryClip;
            view.Play(clip);
            view.DOnEnterDefaule.invoker?.Invoke();
        }

        public static void Play(this AnimatorNodeView view,int index){
            if(index>=0 && index < view.clipData.defaultClips.Length){
                var clip = view.clipData.defaultClips[index];
                view.Play(clip);
                view.DOnPlayedIndexed.invoker?.Invoke(index);
            }
        }

        public static void Play(this AnimatorNodeView view, PlayAnimationClipInfo info){
            view.anim.Play(info.clipName,info.layer,info.mixedTime);
            view.DOnPlayedAnimation.invoker?.Invoke(info);
        }

        public static void SetFloat(this AnimatorNodeView view,string floatName,float value){
            view.anim.SetFloat(floatName,value);
        }
    }

    


}