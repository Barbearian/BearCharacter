using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Bear
{
    public class NavigatorInputNodeData: INodeData
    {
        public Action<Vector3> DMoveTo;
        public Action DOnStop;

    }

    public class DirectionalMovementInputNodeData: INodeData{
        public Action<Vector3> DMove;
        public Action<Vector3> DRotate;

        public Vector3 MoveDir;
        public Vector3 RotateDir;
    }

    public interface IReceiveNavigationTarget{
        public Action<Vector3> DReceiveMoveTo{get;}
    }

    public interface IReceiveNavigationScan{
         public Action DOnReceive{get;}
    }

}