
using UnityEngine;

namespace Bear{
    
    public interface IAnimatorNode: IAnimatorClipsPlayer
    {
        public Animator Anim{get;set;}
        public SafeDelegate<PlayAnimationClipInfo> DOnPlayedAnimation{get;set;}
        public SafeDelegate<int> DOnPlayedIndexed{get;set;}
        public SafeDelegate DOnEnterDefaule{get;set;}
        public AnimationClipNodeData ClipData{get;set;}
    }

    [System.Serializable]
    public class AnimatorNode : IAnimatorNode,INode
    {
        public Animator anim;
        public SafeDelegate<PlayAnimationClipInfo> dOnPlayedAnimation = new SafeDelegate<PlayAnimationClipInfo>();
        public SafeDelegate<int> dOnPlayedIndexed = new SafeDelegate<int>();
        public SafeDelegate dOnEnterDefaule = new SafeDelegate();
        public AnimationClipNodeData clipData;
        public Animator Anim { get => anim; set => anim = value; }
        public SafeDelegate<PlayAnimationClipInfo> DOnPlayedAnimation { get => dOnPlayedAnimation; set => dOnPlayedAnimation=value; }
        public SafeDelegate<int> DOnPlayedIndexed { get => dOnPlayedIndexed; set => dOnPlayedIndexed = value; }
        public SafeDelegate DOnEnterDefaule { get => dOnEnterDefaule; set => dOnEnterDefaule = value; }
        public AnimationClipNodeData ClipData { get => clipData; set => clipData = value; }

        public void Play(int index)
        {
            AnimatorNodeSystem.Play(this, index);

        }
    }
    public class AnimatorNodeView : NodeView,IAnimatorClipsPlayer, IAnimatorNode
    {
        public AnimatorNode node = new AnimatorNode();

        public Animator Anim { get => node.Anim; set => node.Anim = value; }
        public SafeDelegate<PlayAnimationClipInfo> DOnPlayedAnimation { get => node.DOnPlayedAnimation; set => node.DOnPlayedAnimation = value; }
        public SafeDelegate<int> DOnPlayedIndexed { get => node.DOnPlayedIndexed; set => node.DOnPlayedIndexed = value; }
        public SafeDelegate DOnEnterDefaule { get => node.DOnEnterDefaule; set => node.DOnEnterDefaule = value; }
        public AnimationClipNodeData ClipData { get => node.ClipData; set => node.ClipData = value; }

        public void Play(int index){
            node.Play(index);

        }

        public override bool Equals(object other)
        {
            if(other is AnimatorNodeView view){
                return node.Equals(view.node);  
            }
            return false;
            
        }

        public override int GetHashCode()
        {
            return node.GetHashCode();
        }
    }

    public static class AnimatorNodeSystem{
        public static void Play(this IAnimatorNode view, string clipName, int layer = 0, float mixedTime = 0){
            PlayAnimationClipInfo info = PlayAnimationClipInfo.Create(clipName,layer,mixedTime);
            view.Play(info);
        }

        public static void EnterDefaultState(this IAnimatorNode view){
            var clip = view.ClipData.EntryClip;
            view.Play(clip);
            view.DOnEnterDefaule.invoker?.Invoke();
        }

        public static void Play(this IAnimatorNode view,int index){
            if(index>=0 && index < view.ClipData.defaultClips.Length){
                var clip = view.ClipData.defaultClips[index];
                view.Play(clip);
                view.DOnPlayedIndexed.invoker?.Invoke(index);
            }
        }

        public static void Play(this IAnimatorNode view, PlayAnimationClipInfo info){
            view.Anim.Play(info.clipName,info.layer,info.mixedTime);
            view.DOnPlayedAnimation.invoker?.Invoke(info);
        }

        public static void SetFloat(this IAnimatorNode view,string floatName,float value){
            view.Anim.SetFloat(floatName,value);
        }
    }

    


}