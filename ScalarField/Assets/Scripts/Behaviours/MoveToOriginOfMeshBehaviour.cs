using HTC.UnityPlugin.Vive;
using UnityEngine;
using VR.Scripts.Behaviours.Button;

namespace Behaviours
{
    /// <summary>
    /// Button behaviour that moves the player inside the scalar field
    /// </summary>
    public class MoveToOriginOfMeshBehaviour : AbstractButtonBehaviour
    {
        /// <summary>
        /// Bounding box containing the scalar field mesh
        /// </summary>
        [Header("Target"), Tooltip("Bounding box containing the mesh")]
        public MeshRenderer boundingBox;

        /// <summary>
        /// Determines which button is used to return to the cockpit once the user is in the scalar field
        /// </summary>
        public ControllerButton travelBackButton = ControllerButton.Grip;

        /// <summary>
        /// User VR rig root
        /// </summary>
        private GameObject _vrRig;
        
        /// <summary>
        /// Cached center position of the given bounding box
        /// </summary>
        private Vector3 _boundsCenter;
        
        /// <summary>
        /// Signals whether user is currently in the scalar field or not
        /// </summary>
        private bool _isInMesh;

        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update, after
        /// <see>
        ///     <cref>Awake</cref>
        /// </see>
        /// </summary>
        private void Start()
        {
            _vrRig = GameObject.Find("ViveRig");
            _boundsCenter = boundingBox.bounds.center;
        }
        
        protected new void Update()
        {
            base.Update();

            // If user is in the scalar field and hits the specified back button, the user is moved back to the cockpit
            if (_isInMesh)
            {
                if (ViveInput.GetPressDown(HandRole.RightHand, travelBackButton))
                {
                    _isInMesh = false;
                    MoveToEaglePoint();
                }
            }
        }
       
        protected override void HandleButtonEvent()
        {
            // Use center of the bounding box as a starting point
            var newPos = _boundsCenter;
            
            // Try to adjust vertical position of the user if the scalar field is positioned lower than the vertical
            // origin (y = 0.0) at the center of the bounding box
            if (Physics.Raycast(
                    new Ray(_boundsCenter, Vector3.down), out RaycastHit hit, boundingBox.bounds.extents.y )
                )
            {
                newPos = hit.point;
            }

            // Move user to determined point
            _isInMesh = true;
            _vrRig.transform.position = newPos;
        }

        /// <summary>
        /// Move user to eagle point
        /// ToDo: Currently duplicate off <see cref="MoveToEaglePositionBehaviour"/>
        /// </summary>
        private void MoveToEaglePoint()
        {
            var eaglePoint = GameObject.Find("EaglePoint").transform;
            _vrRig.transform.position = eaglePoint.position;
        }
    }
}