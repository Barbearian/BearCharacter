using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class MovementInputNode : INode, IGlobalNodeDataAccessor
    {
        private bool requsted;
        public CameraNodeData data;
        public System.Action<Vector3> forward;
        public MovementInputNode()
        {
            this.RequestGlobalNodeData<CameraNodeData>(NodeDataRegistered);
        }
        public void Move(Vector2 inputDir)
        {
            if (requsted) {
                forward?.Invoke(data.GetInputDir(inputDir));
            }
        }
        private void NodeDataRegistered(CameraNodeData data)
        {
            requsted = true;
            this.data = data;
            
            
        }
    }

    public interface IMovementInputReceiver{
        System.Action<Vector3> DOnReceiveMovementInput{get;}
    }


}