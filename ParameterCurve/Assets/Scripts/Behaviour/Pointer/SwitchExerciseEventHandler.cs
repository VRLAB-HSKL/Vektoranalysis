using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.Pointer
{
    /// <summary>
    /// Handles the Behaviour of the in-game navigation objects used to switch between exercises     
    /// </summary>
    public class SwitchExerciseEventHandler : AbstractVisualChangeSelectionEventHandler
    {
        public bool IsIncrement;

        protected override void HandlePointerClick(PointerEventData eventData)
        {
            Debug.Log("SwitchExerciseEventHandler: HandlePointerClick()");
            
            if (IsIncrement)
            {
                GlobalDataModel.ExerciseCurveController.NextSubExercise();
            }
            else
            {
                GlobalDataModel.ExerciseCurveController.PreviousSubExercise();
            }
        }

        protected override void HandlePointerEnter(PointerEventData eventData) {}

        protected override void HandlePointerExit(PointerEventData eventData) {}
    }
}
