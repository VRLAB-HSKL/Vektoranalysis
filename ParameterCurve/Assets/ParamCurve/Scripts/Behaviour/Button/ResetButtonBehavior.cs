using Model;
using ParamCurve.Scripts.Model;

namespace ParamCurve.Scripts.Behaviour.Button
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
            AbstractExercise currentExercise = GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex];
            
            //for selection exercise, update selection indices
            //for tangent normal exercise, reset height adjustment and sphere positions
            if (currentExercise is SelectionExercise)
                GlobalDataModel.ExerciseCurveController.SelectionIndices[GlobalDataModel.CurrentSubExerciseIndex] = -1;
            else if (currentExercise is TangentNormalExercise)
                GlobalDataModel.ExerciseCurveController.TangentNormalReset();
        }
        #endregion Clicked
    }
}
