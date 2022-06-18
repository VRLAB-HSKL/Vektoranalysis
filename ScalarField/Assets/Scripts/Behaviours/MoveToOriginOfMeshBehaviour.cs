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

            vrig.transform.position = center;
        }
    }
}