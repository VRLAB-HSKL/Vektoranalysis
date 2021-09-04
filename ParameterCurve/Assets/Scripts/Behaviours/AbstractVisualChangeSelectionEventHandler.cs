using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviours
{
    public abstract class AbstractVisualChangeSelectionEventHandler : AbstractCanvasRaycastEventHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        public GameObject visualTarget;
        public bool updateChildrenRenderers;

        public Material defaultMat;
        public Material hoverMat;
        public Material selectionMat;

        private bool _isSelected;

        private readonly HashSet<PointerEventData> _hovers = new HashSet<PointerEventData>();

        private List<MeshRenderer> MeshRenderers
        {
            get
            {
                var mrs = new List<MeshRenderer>();
                
                if (visualTarget.TryGetComponent(out MeshRenderer mr))
                {
                    mrs.Add(mr);
                }

                if (updateChildrenRenderers)
                {
                    mrs.AddRange(visualTarget.GetComponentsInChildren<MeshRenderer>());
                }

                return mrs;
            }
        }
        
        public new void OnPointerClick(PointerEventData eventData)
        {
            if (eventData is VivePointerEventData viveEventData)
            {
                if (viveEventData.viveButton != ControllerButton.Trigger) return;
                
                var newMat = _isSelected ? hoverMat : selectionMat;
                // MeshRenderer mr = visualTarget.GetComponent<MeshRenderer>();
                // if(mr != null) mr.material = newMat;
                //
                // if(updateChildrenRenderers)
                // {
                //     var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                //     foreach (var m in meshRenderers)
                //     {
                //         m.material = newMat;
                //     }
                // }

                foreach (var m in MeshRenderers)
                {
                    m.material = newMat;
                }

                _isSelected = !_isSelected;
                HandlePointerClick(eventData);
            }
            else if (eventData is {button: PointerEventData.InputButton.Left})
            {
                // Standalone button triggered!
                var newMat = _isSelected ? hoverMat : selectionMat;
                // visualTarget.material = newMat;
                //
                // if(updateChildrenRenderers)
                // {
                //     var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                //     foreach (var m in meshRenderers)
                //     {
                //         m.material = newMat;
                //     }
                // }
                
                foreach (var m in MeshRenderers)
                {
                    m.material = newMat;
                }

                _isSelected = !_isSelected;
                HandlePointerClick(eventData);
            }
        }

        public new void OnPointerEnter(PointerEventData eventData)
        {
            if (_hovers.Add(eventData) && _hovers.Count == 1 && !_isSelected)
            {
                // visualTarget.material = hoverMat;
                //
                // if(updateChildrenRenderers)
                // {
                //     var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                //     foreach (MeshRenderer m in meshRenderers)
                //     {
                //         m.material = hoverMat;
                //     }
                // }

                foreach (var m in MeshRenderers)
                {
                    m.material = hoverMat;
                }
                
                HandlePointerEnter(eventData);
            }
        }

        public new void OnPointerExit(PointerEventData eventData)
        {
            if (_hovers.Remove(eventData) && _hovers.Count == 0 && !_isSelected)
            {
                // visualTarget.material = defaultMat;
                //
                // if(updateChildrenRenderers)
                // {
                //     var meshRenderers = visualTarget.GetComponentsInChildren<MeshRenderer>();
                //     foreach (var m in meshRenderers)
                //     {
                //         m.material = defaultMat;
                //     }
                // }

                foreach (var m in MeshRenderers)
                {
                    m.material = defaultMat;
                }
                
                HandlePointerExit(eventData);
            }
        }
    }
}
