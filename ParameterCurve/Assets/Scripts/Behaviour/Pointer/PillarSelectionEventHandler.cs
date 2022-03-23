using System.Collections.Generic;
using System.Linq;
using Controller;
using HTC.UnityPlugin.Vive;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour
{
    /// <summary>
    /// Handles the Behaviour of pillars on selection of them using the VR controller
    ///
    /// Class based on VIU developer pdf
    /// </summary>
    public class PillarSelectionEventHandler : AbstractVisualChangeSelectionEventHandler
    {
        //private ExerciseViewController threeSel;
        
        
        private void Update()
        {
            // if (IsSelected) return;
            //
            // if (_hovers.Any()) return;
            

            if (!IsSelected)
            {
                var newMat = Hovers.Any() ? hoverMat : defaultMat;
                
                //Debug.Log(_hovers.Count);
                foreach (var m in MeshRenderers)
                {
                    //m.material =  newMat;
                }    
            }
            
        }

        protected override void HandlePointerClick(PointerEventData eventData)
        {
            //Debug.Log("choice: " + selectionChoice);
            //GlobalDataModel.ExerciseCurveController.SetSelection(IsSelected ? -1 : selectionChoice);
        }

        protected override void HandlePointerEnter(PointerEventData eventData){}

        protected override void HandlePointerExit(PointerEventData eventData){}
    }
}
