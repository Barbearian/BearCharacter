using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Bear
{
    public struct NavigatorInputNodeData: INodeData
    {
        public Action<Vector3> DMoveTo;
        public Action DOnStop;

    }

    public struct DirectionalMovementInputNodeData: INodeData{
        public Action<Vector3> DMove;

        public Action<Vector3> DRotate;
    }

    public interface IReceiveNavigationTarget{
        public Action<Vector3> DReceiveMoveTo{get;}
    }

    public interface IReceiveNavigationScan{
         public Action DOnReceive{get;}
    }

}