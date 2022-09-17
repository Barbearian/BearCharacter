using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [System.Serializable]
    public struct MovementNodeData : INodeData
    {
        public float speedMulti;
        public bool isMoving;
        public Vector3 dir;

    }

    public struct MovementOutputNodeData :INodeData{
        public System.Action<Vector3> DMove;
    }
}