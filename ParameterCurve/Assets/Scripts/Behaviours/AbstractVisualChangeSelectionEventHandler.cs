using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviours
{
    public abstract class AbstractVisualChangeSelectionEventHandler : AbstractCanvasRaycastEventHandler
    {
        public MeshRenderer visualTarget;
        public bool updateChildrenRenderers;

        public Material defaultMat;
        public Material hoverMat;
        public Material selectionMat;

        protected bool IsSelected = false;

        private readonly HashSet<PointerEventData> _hovers = new HashSet<PointerEventData>();

        public new void OnPointerClick(PointerEventData eventData)
        {
            if (eventData is VivePointerEventData viveEventData)
            {
                if (viveEventData.viveButton != ControllerButton.Trigger) return;
                
                var newMat = IsSelected ? hoverMat : selectionMat;
                visualTarget.material = newMat;

                if(updateChildrenRenderers)
                {
                    var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                    foreach (var m in meshRenderers)
                    {
                        m.material = newMat;
                    }
                }

                IsSelected = !IsSelected;
                HandlePointerClick(eventData);
            }
            else if (eventData is {button: PointerEventData.InputButton.Left})
            {
                // Standalone button triggered!
                var newMat = IsSelected ? hoverMat : selectionMat;
                visualTarget.material = newMat;

                if(updateChildrenRenderers)
                {
                    var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                    foreach (var m in meshRenderers)
                    {
                        m.material = newMat;
                    }
                }

                IsSelected = !IsSelected;
                HandlePointerClick(eventData);
            }
        }

        public new void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Enter!");

            if (_hovers.Add(eventData) && _hovers.Count == 1 && !IsSelected)
            {
                visualTarget.material = hoverMat;

                if(updateChildrenRenderers)
                {
                    var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer m in meshRenderers)
                    {
                        m.material = hoverMat;
                    }
                }

                HandlePointerEnter(eventData);
            }
        }

        public new void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Exit!");

            if (_hovers.Remove(eventData) && _hovers.Count == 0 && !IsSelected)
            {
                visualTarget.material = defaultMat;
                
                if(updateChildrenRenderers)
                {
                    var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                    foreach (var m in meshRenderers)
                    {
                        m.material = defaultMat;
                    }
                }

                HandlePointerExit(eventData);
            }
        }
    }
}
