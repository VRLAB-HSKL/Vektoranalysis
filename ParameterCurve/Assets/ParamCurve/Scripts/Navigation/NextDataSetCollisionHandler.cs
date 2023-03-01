using HTC.UnityPlugin.ColliderEvent;
using ParamCurve.Scripts.Controller;
using UnityEngine;

namespace ParamCurve.Scripts.Navigation
{
    /// <summary>
    /// Switches to next data set on object collision
    /// </summary>
    public class NextDataSetCollisionHandler : MonoBehaviour, IColliderEventHoverEnterHandler
    {
        #region Public members
        
        /// <summary>
        /// Target object containing the function to be called
        /// </summary>
        public GameObject target;

        #endregion Public members
        
        #region Public functions
        
        /// <summary>
        /// Globally moves to next dataset using world controller instance
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
        {
            var world = target.GetComponent<WorldStateController>();
            if(world != null)
            {
                world.SwitchToNextDataset();
            }
        }
        
        #endregion Public functions
    }
}
