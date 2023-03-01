using UnityEngine;
using UnityEngine.EventSystems;

namespace ParamCurve.Scripts.Behaviour.Pointer
{
    /// <summary>
    /// Abstract raycast handler to map event handling of collision and click events between canvas elements
    /// and a raycast line that is generated from VR controllers
    /// </summary>
    public abstract class AbstractCanvasRaycastEventHandler : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        /// <summary>
        /// Handles pointer click event
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            HandlePointerClick(eventData);
        }

        /// <summary>
        /// Handles pointer enter event
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            HandlePointerEnter(eventData);
        }

        /// <summary>
        /// Handles pointer exit event
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            HandlePointerExit(eventData);
        }

        /// <summary>
        /// Abstract method for custom implementation of pointer click
        /// event handling in derived class
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected abstract void HandlePointerClick(PointerEventData eventData);
        
        /// <summary>
        /// Abstract method for custom implementation of pointer enter
        /// event handling in derived class
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected abstract void HandlePointerEnter(PointerEventData eventData);
        
        /// <summary>
        /// Abstract method for custom implementation of pointer exit
        /// event handling in derived class
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected abstract void HandlePointerExit(PointerEventData eventData);
    }
}
