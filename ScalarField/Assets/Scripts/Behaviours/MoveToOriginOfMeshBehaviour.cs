using UnityEngine;
using VR.Scripts.Behaviours.Button;

namespace Behaviours
{
    public class MoveToOriginOfMeshBehaviour : AbstractButtonBehaviour
    {
        public MeshRenderer BoundingBox;
        
        protected override void HandleButtonEvent()
        {
            var vrig = GameObject.Find("ViveRig");

            var center = BoundingBox.bounds.center;

            if (Physics.Raycast(new Ray(center, Vector3.down), out RaycastHit hit, BoundingBox.bounds.extents.y ))
            {
                center = hit.point;
            }


            vrig.transform.position = center;
        }
    }
}