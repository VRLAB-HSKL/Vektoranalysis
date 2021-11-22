using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour
{
    /// <summary>
    /// Handles the Behaviour of the ingame navigation objects used to switch between exercises     
    /// </summary>
    public class SwitchExerciseEventHandler : AbstractVisualChangeSelectionEventHandler
    {
        public bool IsIncrement;

        protected override void HandlePointerClick(PointerEventData eventData)
        {
            Debug.Log("SwitchExerciseEventHandler: HandlePointerClick()");
            
            if (IsIncrement)
            {
                GlobalData.ExerciseCurveController.NextSubExercise();
            }
            else
            {
                GlobalData.ExerciseCurveController.PreviousSubExercise();
            }
        }

        protected override void HandlePointerEnter(PointerEventData eventData)
        {
        
        }

        protected override void HandlePointerExit(PointerEventData eventData)
        {
        
        }
    }
}
