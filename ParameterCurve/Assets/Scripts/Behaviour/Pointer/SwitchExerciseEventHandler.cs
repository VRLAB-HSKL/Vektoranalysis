using Model;
using UnityEngine.EventSystems;

namespace Behaviour.Pointer
{
    /// <summary>
    /// Handles the Behaviour of the in-game navigation objects used to switch between exercises     
    /// </summary>
    public class SwitchExerciseEventHandler : AbstractVisualChangeSelectionEventHandler
    {
        #region Public members
        
        /// <summary>
        /// Signals whether this handler increments or decrements the index
        /// </summary> 
        public bool isIncrement;

        #endregion Public members
        
        #region Protected functions
        
        /// <summary>
        /// Handle click event to switch to next/previous exercise
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected override void HandlePointerClick(PointerEventData eventData)
        {
            //Debug.Log("SwitchExerciseEventHandler: HandlePointerClick()");
            
            if (isIncrement)
            {
                GlobalDataModel.ExerciseCurveController.NextSubExercise();
            }
            else
            {
                GlobalDataModel.ExerciseCurveController.PreviousSubExercise();
            }
        }

        /// <summary>
        /// Handle click event on pointer enter
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected override void HandlePointerEnter(PointerEventData eventData) {}

        /// <summary>
        /// Handle click event on pointer exit
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected override void HandlePointerExit(PointerEventData eventData) {}
        
        #endregion Protected functions
    }
}
