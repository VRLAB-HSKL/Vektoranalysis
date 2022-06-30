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
    /// Button Behaviour used to switch to the previous curve in the current dataset
    /// </summary>
    public class ExerciseButtonBehavior : AbstractButtonBehaviour
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
            VR.transform.position = VR.transform.position + new Vector3(20, 0, 0);
            curveControl.SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Exercises);
        }
        #endregion Clicked
    }
}
