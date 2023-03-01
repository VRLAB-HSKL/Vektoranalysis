using Model;

namespace ParamCurve.Scripts.Behaviour.Button
{
    /// <summary>
    /// Button that will start over current exercise
    /// </summary>
    public class RetryButtonBehavior : AbstractButtonBehaviour
    {

        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update
        /// </summary>
        protected new void Start()
        {
            base.Start();
        }

        #region Clicked
        protected override void HandleButtonEvent()
        {
            GlobalDataModel.ExerciseCurveController.ResetCurrentExercise();
        }
        #endregion Clicked
    }
}
