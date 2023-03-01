using ParamCurve.Scripts.Controller;
using UnityEngine;

namespace ParamCurve.Scripts.UI.States
{
    /// <summary>
    /// Curve state used during selection and interaction of exercises in the exercises dataset
    /// </summary>
    public class ExerciseCurvesState : AbstractCurveSelectionState
    {
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="menuContent">Menu content</param>
        /// <param name="prefab">Prefab for instancing</param>
        /// <param name="world">World instance</param>
        public ExerciseCurvesState(GameObject menuContent, GameObject prefab, WorldStateController world) 
            : base(menuContent, prefab, world) { }

        #endregion Constructors
        
        #region Public functions
        
        public override void OnStateUpdate() {}
        
        #endregion Public functions
    }
}
