using Model;
using Controller;
using UnityEngine;

namespace Behaviour.Button
{
    /// <summary>
    /// Button that will reset user choices in current sub exercise
    /// </summary>
    public class ResetButtonBehavior : AbstractButtonBehaviour
    {

        protected new void Start()
        {
            base.Start();
        }

        #region Clicked
        protected override void HandleButtonEvent()
        {
            AbstractExercise CurrentExercise = GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex];
            
            //for selection exercise, update selection indices
            //for tangent normal exercise, reset height adjustment and sphere positions
            if (CurrentExercise is SelectionExercise)
                GlobalDataModel.ExerciseCurveController.SelectionIndices[GlobalDataModel.CurrentSubExerciseIndex] = -1;
            else if (CurrentExercise is TangentNormalExercise)
                GlobalDataModel.ExerciseCurveController.TangentNormalReset();
        }
        #endregion Clicked
    }
}
