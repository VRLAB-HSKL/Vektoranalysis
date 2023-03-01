using Model;
using ParamCurve.Scripts.Controller;
using ParamCurve.Scripts.UI;
using UnityEngine;

namespace ParamCurve.Scripts.Behaviour.Button
{
    /// <summary>
    /// Button Behaviour used to return from exercise room to main display room
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
        }

        #region Clicked
        protected override void HandleButtonEvent()
        {
            //new room is +20 units in z direction from main room
            VR.transform.position += new Vector3(0, 0, -20);
            //move menu parent object back as well
            curveControl.curveMenuParent.transform.parent.gameObject.transform.position += new Vector3(0, 0, -20);

            curveControl.SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Display);
            GlobalDataModel.ExerciseCurveController.ResetCurrentExercise();
        }
        #endregion Clicked
    }
}
