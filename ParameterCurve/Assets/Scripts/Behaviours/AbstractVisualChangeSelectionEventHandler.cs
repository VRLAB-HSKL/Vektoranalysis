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
        public bool isToggle;


        public ThreeSelectionView threeSel;
        public SelectionChoice selectionChoice = SelectionChoice.None;

        protected bool IsSelected => selectionChoice == threeSel.selection;

        public Material defaultMat;
        public Material hoverMat;
        public Material selectionMat;



        

        protected readonly HashSet<PointerEventData> _hovers = new HashSet<PointerEventData>();

        protected List<MeshRenderer> MeshRenderers
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

                if (isToggle)
                {
                    var newMat = IsSelected ? hoverMat : selectionMat;

                    foreach (var m in MeshRenderers)
                    {
                        m.material = newMat;
                    }

                    //_isSelected = !_isSelected;

                    threeSel.SetSelection(IsSelected ? SelectionChoice.None : selectionChoice);

                    HandlePointerClick(eventData);        
                }
                else
                {
                    foreach (var m in MeshRenderers)
                    {
                        m.material = selectionMat;
                    }
                    
                    HandlePointerClick(eventData);
                    
                    foreach (var m in MeshRenderers)
                    {
                        m.material = hoverMat;
                    }
                }
            }
            else if (eventData is {button: PointerEventData.InputButton.Left})
            {
                // Standalone button triggered!

                if (isToggle)
                {
                    var newMat = IsSelected ? hoverMat : selectionMat;

                    foreach (var m in MeshRenderers)
                    {
                        m.material = newMat;
                    }

                    //_isSelected = !_isSelected;    
                    
                    threeSel.SetSelection(IsSelected ? SelectionChoice.None : selectionChoice);
                    
                    HandlePointerClick(eventData);        
                }
                else
                {
                    foreach (var m in MeshRenderers)
                    {
                        m.material = selectionMat;
                    }
                    
                    HandlePointerClick(eventData);
                    
                    foreach (var m in MeshRenderers)
                    {
                        m.material = hoverMat;
                    }
                }
            }
        }

        public new void OnPointerEnter(PointerEventData eventData)
        {
            if (_hovers.Add(eventData) && _hovers.Count == 1)
            {
                if (isToggle && IsSelected) return;
                
                // Debug.Log("IsToggle: " + isToggle);
                // Debug.Log("IsSelected: " + IsSelected);
                
                
                foreach (var m in MeshRenderers)
                {
                    m.material = hoverMat;
                }
                
                HandlePointerEnter(eventData);
            }
        }

        public new void OnPointerExit(PointerEventData eventData)
        {
            if (_hovers.Remove(eventData) && _hovers.Count == 0)
            {
                if (isToggle && IsSelected) return;
                
                foreach (var m in MeshRenderers)
                {
                    m.material = defaultMat;
                }
                
                HandlePointerExit(eventData);
            }
        }
    }
}
