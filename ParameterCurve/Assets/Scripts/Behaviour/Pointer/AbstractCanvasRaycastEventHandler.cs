using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.Pointer
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
        public void OnPointerClick(PointerEventData eventData)
        {
            HandlePointerClick(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HandlePointerEnter(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HandlePointerExit(eventData);
        }

        protected abstract void HandlePointerClick(PointerEventData eventData);
        protected abstract void HandlePointerEnter(PointerEventData eventData);
        protected abstract void HandlePointerExit(PointerEventData eventData);
    }
}
