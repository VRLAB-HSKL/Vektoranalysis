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
        private bool _isInMesh = true;

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

        private GameObject _hintText;
        
        protected new void Update()
        {
            base.Update();

            // if(HintText.activeSelf != _isInMesh)
            //     HintText.SetActive(_isInMesh);
            
            // If user is in the scalar field and hits the specified back button, the user is moved back to the cockpit
            if (_isInMesh)
            {
                // var pose = VivePose.GetPoseEx(HandRole.RightHand);
                // //Debug.Log("posepos: " + pose.pos + ", transformedPosePos: " + pose.TransformPoint(pose.pos));
                //
                // var position = pose.pos + new Vector3(0f, -0.25f, 0f);
                //
                // HintText.transform.localPosition = position; //pose.TransformPoint(pose.pos);
                // HintText.transform.rotation = pose.rot; //Quaternion.Lerp(_vrRig.transform.rotation, pose.rot, 0.5f); 
                //
                // var lr =HintText.GetComponentInChildren<LineRenderer>();
                //
                // lr.positionCount = 2;
                // lr.SetPosition(0, pose.pos);
                // lr.SetPosition(1, position);
                //
                // var canvas = HintText.transform.GetChild(1);
                // canvas.transform.position = position;
                
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
            var ray = new Ray(_boundsCenter, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, boundingBox.bounds.extents.y))
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