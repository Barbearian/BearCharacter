using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [System.Serializable]
    public class CameraNodeData : INodeData
    {
        public Vector3 cameraPosition;
        public Quaternion CameraRotation;
    }

    public static class CameraNodeDataSystem {
        public static Vector3 GetInputDir(this CameraNodeData camNodeData,Vector2 inputDir) {

            float inputx = inputDir.x;
            float inputy = inputDir.y;

            Vector3 dir = camNodeData.CameraRotation*Vector3.forward*inputy + camNodeData.CameraRotation*Vector3.right*inputx;
            dir.y = 0;
            return dir.normalized;
        }
    }
}