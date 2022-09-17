using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Bear
{
    public struct MovementObserverNodeData : INodeData
    {
        public Action<float> DOnMove;
        public Action DOnStop;
        public Action DOnStartMove;
       
    }
}