using Model;
using ParamCurve.Scripts.Controller;
using ParamCurve.Scripts.UI;
using UnityEngine;

namespace ParamCurve.Scripts.Behaviour.Button
{
    /// <summary>
    /// Button Behaviour used to travel from main display room to exercise room
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
            gameObject.SetActive(GlobalDataModel.InitFile.ApplicationSettings.TableSettings.ShowQuizButton);
        }

        #region Clicked
        protected override void HandleButtonEvent()
        {
            //new room is +20 units in z direction from main room
            VR.transform.position = VR.transform.position + new Vector3(0, 0, 20);
            //move menu parent object as well (so you can select another exercise)
            curveControl.curveMenuParent.transform.parent.position += new Vector3(0, 0, 20);
            curveControl.SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Exercises);
        }
        #endregion Clicked
    }
}
