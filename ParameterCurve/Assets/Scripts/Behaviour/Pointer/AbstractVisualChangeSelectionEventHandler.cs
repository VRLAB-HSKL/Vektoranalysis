using System.Collections.Generic;
using Behaviour.Pointer;
using Controller;
using HTC.UnityPlugin.Vive;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using Views;

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
        public GameObject visualTarget;
        public bool updateChildrenRenderers;
        public bool isToggle;


        //public ExerciseViewController threeSel;
        
        public int selectionChoice = -1;

        /// <summary>
        /// Signals if a pillar was selected or not
        /// </summary>
        protected bool IsSelected
        {
            get
            {
                //Debug.Log("globalExerciseController is null: " + (GlobalData.exerciseController is null));
                
                bool isSel = selectionChoice ==
                    GlobalData.ExerciseCurveController?.SelectionIndices[GlobalData.CurrentSubExerciseIndex]; //currentExerciseIndex;    

                return isSel;
            }
        }
            

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
        
        /// <summary>
        /// Updates the visual state of the target game object if a click event was registered
        /// </summary>
        /// <param name="eventData"></param>
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

                    //_isSelected = !_isSelected;

                    GlobalData.ExerciseCurveController.SetSelection(IsSelected ? -1 : selectionChoice);

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
            // else if (eventData  is {button: PointerEventData.InputButton.Left})
            // {
            //     // Standalone button triggered!
            //
            //     if (isToggle)
            //     {
            //         var newMat = IsSelected ? hoverMat : selectionMat;
            //
            //         foreach (var m in MeshRenderers)
            //         {
            //             m.material = newMat;
            //         }
            //
            //         //_isSelected = !_isSelected;    
            //         
            //         GlobalData.ExerciseController.SetSelection(IsSelected ? -1 : selectionChoice);
            //         
            //         HandlePointerClick(eventData);        
            //     }
            //     else
            //     {
            //         foreach (var m in MeshRenderers)
            //         {
            //             m.material = selectionMat;
            //         }
            //         
            //         HandlePointerClick(eventData);
            //         
            //         foreach (var m in MeshRenderers)
            //         {
            //             m.material = hoverMat;
            //         }
            //     }
            // }
        }

        public new void OnPointerEnter(PointerEventData eventData)
        {
            if (Hovers.Add(eventData) && Hovers.Count == 1)
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
