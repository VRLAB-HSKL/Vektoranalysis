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
    public class ExitButtonBehavior : AbstractButtonBehaviour
    {
        /// <summary>
        /// Single world state controller instance <see cref="WorldStateController"/>
        /// </summary>
        public WorldStateController world;

        /// <summary>
        /// Vive Rig to move to other room upon button press
        /// </summary>
        public GameObject VR;

        public CurveSelectionControl curveControl;

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
            //new room is +20 units in x direction from main room
            VR.transform.position = VR.transform.position + new Vector3(0, 0, -20);
            //move menu parent object back as well
            curveControl.curveMenuParent.transform.parent.gameObject.transform.position = curveControl.curveMenuParent.transform.parent.gameObject.transform.position + new Vector3(0, 0, -20);

            curveControl.SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Display);
            GlobalDataModel.ExerciseCurveController.ResetCurrentExercise();
        }
        #endregion Clicked
    }
}
