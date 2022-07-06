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
    /// ***If exercises are converted to a separate scene, this class should 
    /// not be used and instead use the ReturnToRoomButtonBehavior.cs script***
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
            gameObject.SetActive(GlobalDataModel.InitFile.ApplicationSettings.TableSettings.ShowNavButtons);
        }

        #region Clicked
        protected override void HandleButtonEvent()
        {
            GlobalDataModel.ExerciseCurveController.ResetExercise();
        }
        #endregion Clicked
    }
}
