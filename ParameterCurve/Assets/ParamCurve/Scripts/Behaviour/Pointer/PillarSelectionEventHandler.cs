using System.Linq;
using Behaviour;
using UnityEngine.EventSystems;

namespace ParamCurve.Scripts.Behaviour.Pointer
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
            if (!IsSelected)
            {
                var newMat = Hovers.Any() ? hoverMat : defaultMat;
                foreach (var m in MeshRenderers)
                {
                    m.material =  newMat;
                }    
            } else
            {
                foreach (var m in MeshRenderers)
                {
                    m.material = selectionMat;
                }
            }
            
        }

        /// <summary>
        /// Handles pointer click event
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected override void HandlePointerClick(PointerEventData eventData) {}

        /// <summary>
        /// Handles pointer enter event
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected override void HandlePointerEnter(PointerEventData eventData) {}

        /// <summary>
        /// Handles pointer exit event
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected override void HandlePointerExit(PointerEventData eventData) {}
    }
}
