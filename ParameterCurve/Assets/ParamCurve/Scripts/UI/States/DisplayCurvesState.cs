using ParamCurve.Scripts.Controller;
using UnityEngine;

namespace ParamCurve.Scripts.UI.States
{
    /// <summary>
    /// Curve state used during selection and interaction of display curves
    /// </summary>
    public class DisplayCurvesState : AbstractCurveSelectionState
    {
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="content">GUI content</param>
        /// <param name="prefab">Prefab for instancing</param>
        /// <param name="world">World instance</param>
        public DisplayCurvesState(GameObject content, GameObject prefab, WorldStateController world) 
            : base(content, prefab, world) {}
        
        #endregion Constructors
        
        public override void OnStateUpdate() { }
    }
}
