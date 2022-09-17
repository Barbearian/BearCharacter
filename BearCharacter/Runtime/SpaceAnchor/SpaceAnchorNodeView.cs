using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    public class SpaceAnchorNodeView : MonoBehaviour
    {
        public Vector3 Position;
        public Quaternion Rotation;

    }


    public static class SpaceAnchorNodeViewSystem{
        public static void SnapTo(this Transform self,SpaceAnchorNodeView target){
            self.position = target.Position;
            self.rotation = target.Rotation;
        }
    }
    
}