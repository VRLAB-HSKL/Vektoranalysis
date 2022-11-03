using System.Collections.Generic;
using Behaviour.Pointer;
using HTC.UnityPlugin.Vive;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour
{
    /// <summary>
    /// Abstract raycast handler that implements the basic visual update behaviour expected when interacting with
    /// an interactive game object. On collision with a user controller ray, the target game object changes color
    /// to signal that it can be interacted with. On click, change the color again to signal that the command was
    /// registered
    /// </summary>
    public abstract class AbstractVisualChangeSelectionEventHandler : AbstractCanvasRaycastEventHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        #region Public members
        
        /// <summary>
        /// Target root game object
        /// </summary>
        public GameObject visualTarget;
        
        /// <summary>
        /// Signals if renderer objects of children objects should be visually updated
        /// </summary>
        public bool updateChildrenRenderers;
        
        /// <summary>
        /// Signals whether the game visual state can be toggled on and off, or is simply
        /// a single visual event that triggers before returning to the original visual state 
        /// </summary>
        public bool isToggle;

        #endregion Public members

        /// <summary>
        /// Local value that corresponds to the value that can be selected
        /// using this pillar
        /// </summary>
        public int selectionChoice;

        /// <summary>
        /// Signals if a pillar was selected or not
        /// </summary>
        protected bool IsSelected =>
            selectionChoice ==
            GlobalDataModel.ExerciseCurveController?.SelectionIndices[GlobalDataModel.CurrentSubExerciseIndex];

        /// <summary>
        /// Default material used for the initial, unselected state
        /// </summary>
        public Material defaultMat;
        
        /// <summary>
        /// Material used to signal collision between the ray and a clickable element
        /// </summary>
        public Material hoverMat;
        
        /// <summary>
        /// Material used to signal that the target object was selected
        /// </summary>
        public Material selectionMat;
        
        /// <summary>
        /// Collection of pointers that are hovering over a clickable object. An object should only be displayed
        /// in its initial state if no pointer is currently colliding with it
        /// </summary>
        protected readonly HashSet<PointerEventData> Hovers = new HashSet<PointerEventData>();

        /// <summary>
        /// Collection of all mesh renderers attached to the target game object. This is used to change the
        /// material of all relevant child objects
        /// </summary>
        protected List<MeshRenderer> MeshRenderers
        {
            get
            {
                var mrs = new List<MeshRenderer>();
                
                // Get mesh renderer of the root node
                if (visualTarget.TryGetComponent(out MeshRenderer mr))
                {
                    mrs.Add(mr);
                }

                // Get mesh renderers of all children
                if (updateChildrenRenderers)
                {
                    mrs.AddRange(visualTarget.GetComponentsInChildren<MeshRenderer>());
                }

                return mrs;
            }
        }
        
        /// <summary>
        /// Updates the visual state of the target game object if a click event was registered
        /// </summary>
        /// <param name="eventData">Event data</param>
        public new void OnPointerClick(PointerEventData eventData)
        {
            // On button click with a VR controller
            if (eventData is VivePointerEventData viveEventData)
            {
                // Only react on events triggered by the Trigger button
                if (viveEventData.viveButton != ControllerButton.Trigger) return;

                // Toggle buttons
                if (isToggle)
                {
                    var newMat = IsSelected ? hoverMat : selectionMat;

                    foreach (var m in MeshRenderers)
                    {
                        m.material = newMat;
                    }
                    
                    GlobalDataModel.ExerciseCurveController.SetSelection(IsSelected ? -1 : selectionChoice);

                    HandlePointerClick(eventData);        
                }
                // Single click buttons
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
            else if (eventData  is {button: PointerEventData.InputButton.Left})
            {
                // Standalone button triggered!
                if (isToggle)
                {
                    var newMat = IsSelected ? hoverMat : selectionMat;
            
                    foreach (var m in MeshRenderers)
                    {
                        m.material = newMat;
                    }
                    
                    GlobalDataModel.ExerciseCurveController.SetSelection(IsSelected ? -1 : selectionChoice);
                    
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

        /// <summary>
        /// Updates the visual state of the target game object when a pointer enters the clickable area
        /// </summary>
        /// <param name="eventData">Event data</param>
        public new void OnPointerEnter(PointerEventData eventData)
        {
            if (Hovers.Add(eventData) && Hovers.Count == 1)
            {
                if (isToggle && IsSelected) return;
                
                foreach (var m in MeshRenderers)
                {
                    m.material = hoverMat;
                }
                
                HandlePointerEnter(eventData);
            }
        }

        /// <summary>
        /// Updates the visual state of the target game object when a pointer leaves the clickable area
        /// </summary>
        /// <param name="eventData">Event data</param>
        public new void OnPointerExit(PointerEventData eventData)
        {
            if (Hovers.Remove(eventData) && Hovers.Count == 0)
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
