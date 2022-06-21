using HTC.UnityPlugin.Vive;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VR.Scripts.Behaviours.Button;

namespace Behaviours
{
    public class MoveToOriginOfMeshBehaviour : AbstractButtonBehaviour
    {
        public MeshRenderer BoundingBox;

        private GameObject _vrRig;
        private Vector3 _boundsCenter;
        private bool _isInMesh;

        private void Start()
        {
            _vrRig = GameObject.Find("ViveRig");
            _boundsCenter = BoundingBox.bounds.center;
        }
        
        private new void Update()
        {
            base.Update();

            if (_isInMesh)
            {
                if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Grip))
                {
                    _isInMesh = false;
                    MoveToEaglePoint();
                }
            }
        }
       
        protected override void HandleButtonEvent()
        {
            var newPos = _boundsCenter;
            
            if (Physics.Raycast(new Ray(_boundsCenter, Vector3.down), out RaycastHit hit, BoundingBox.bounds.extents.y ))
            {
                newPos = hit.point;
            }

            _isInMesh = true;
            _vrRig.transform.position = newPos;
        }

        private void MoveToEaglePoint()
        {
            var eaglePoint = GameObject.Find("EaglePoint").transform;

            _vrRig.transform.position = eaglePoint.position;
        }
    }
}