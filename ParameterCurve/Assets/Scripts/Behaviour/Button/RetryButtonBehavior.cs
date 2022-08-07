using Controller;
using Model;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using HTC.UnityPlugin.Vive;
using UI;

namespace Behaviour.Button
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
