using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Bear{
    public interface ICinemachineBrainAccessor{

    }

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineVirtualCameraNodeView : NodeView,ICinemachineBrainAccessor
    {
        public CinemachineVirtualCamera cam;
        public CameraTargetNodeData dcland;

        
        private void Awake() {
            this.GetCineMachineBrainNodeData();
            cam = GetComponent<CinemachineVirtualCamera>();

            cam.LookAt = dcland.lookAt;
            cam.Follow = dcland.follow;
            
            dcland.DSetLookAt += this.UpdateLookAt;
            dcland.DSetFollow += this.UpdateFollow;            
        }
        
    }

    public static class CinemachineVirtualCameraNodeViewSystem{
        public static CinemachineBrainNodeData GetCineMachineBrainNodeData(this ICinemachineBrainAccessor view,CinemachineBrainNodeData defaultData = new CinemachineBrainNodeData()){
            if(INodeSystem.GlobalNode.TryGetNodeData<CinemachineBrainNodeData>(out var data)){
                return data;
            }else{
                INodeSystem.GlobalNode.AddNodeData(defaultData);
                defaultData.Init();
                return defaultData;
            }
            //return INodeSystem.GlobalNode.GetOrCreateNodeData<CinemachineBrainNodeData>(defaultData);
        }

        public static void UpdateLookAt(this CinemachineVirtualCameraNodeView view, Transform transform){
            view.cam.LookAt = transform;
        }

        public static void UpdateFollow(this CinemachineVirtualCameraNodeView view, Transform transform){
            view.cam.Follow = transform;
        }
    }

    public struct CinemachineBrainNodeData:INodeData{
        public CinemachineBrain brain;
    }

    public static class CinemachineBrainNodeDataSystem{
        public static void Init(this CinemachineBrainNodeData data){
            if(data.brain != null){
                return;
            }

            Camera cam = Camera.main;

            if(!cam.TryGetComponent<CinemachineBrain>(out var brain)){
                data.brain = cam.gameObject.AddComponent<CinemachineBrain>();
            }else{
                data.brain = brain;
            }
        }
    }
}