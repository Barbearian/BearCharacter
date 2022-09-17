using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [RequireComponent(typeof(Camera))]
    public class CameraNodeView : NodeView,IGlobalNodeDataAccessor
    {
        public CameraNodeData cnd;
        
        private void Awake()
        {
            
            Init();
        }

        private void FixedUpdate()
        {
            cnd.cameraPosition = transform.position;
            cnd.CameraRotation = transform.rotation;
        }

        public void Init(){
            if(!this.TryGetGlobalNodeData<CameraNodeData>(out cnd)){
                cnd = new CameraNodeData();
                this.AddGlobalNodeData<CameraNodeData>(cnd);
            }
            
        }
    }
}