using UnityEngine;
using VR.Scripts.Behaviours.Button;

namespace Behaviours
{
    /// <summary>
    /// Button behaviour to move user (vive rig) and cockpit to a specified position (eagle point)
    /// </summary>
    public class MoveToEaglePositionBehaviour : AbstractButtonBehaviour
    {

        /// <summary>
        /// Cockpit root game object
        /// </summary>
        [Header("Move Objects")]
        public GameObject cockpit;
        
        /// <summary>
        /// Vive rig root game object
        /// </summary>
        public GameObject user;
        
        /// <summary>
        /// GameObject in the scene that will function as the eagle point
        /// </summary>
        private Transform _eaglePoint;

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
            cockpit ??= GameObject.Find("UserConsole");
            user ??= GameObject.Find("ViveRig");
            _eaglePoint = GameObject.Find("EaglePoint").transform;
        }
        
        /// <summary>
        /// Moves the user and cockpit to the eagle point
        /// </summary>
        protected override void HandleButtonEvent()
        {
            // Get eagle point position
            var position = _eaglePoint.position;
            
            // Move cockpit
            cockpit.transform.position = position;
            cockpit.transform.rotation = _eaglePoint.rotation;
            
            // Move user
            user.transform.position = position;
        }
    }
}