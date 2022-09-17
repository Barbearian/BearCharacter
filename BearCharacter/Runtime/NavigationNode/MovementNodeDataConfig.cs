using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    [CreateAssetMenu(menuName = "NodeDataConfig/MovementNodeDataConfig")]
    public class MovementNodeDataConfig : ScriptableObject
    {
        public MovementNodeData data; 
        public MovementNodeData GetMovementNodeData(){
            return new MovementNodeData(){
                speedMulti = data.speedMulti,
            };
        }
    }
}