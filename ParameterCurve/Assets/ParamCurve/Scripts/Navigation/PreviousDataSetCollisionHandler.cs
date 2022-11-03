using Controller;
using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// Switches to previous data set on object collision
    /// </summary>
    public class PreviousDataSetCollisionHandler : MonoBehaviour, IColliderEventHoverEnterHandler
    {
        /// <summary>
        /// Target object containing the function to be called
        /// </summary>
        public GameObject target;

        public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
        {
            WorldStateController pm = target.GetComponent<WorldStateController>();
            if(pm != null)
            {
                pm.SwitchToPreviousDataset();
            }
        }

    }
}
