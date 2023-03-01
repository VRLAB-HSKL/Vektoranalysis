using ParamCurve.Scripts.Controller.Curve;
using UnityEngine;

namespace ParamCurve.Scripts.Views.Display
{
    /// <summary>
    /// Basic view on a display curve
    /// </summary>
    public class SimpleCurveView : AbstractCurveView
    {
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="displayLr">Line renderer to display curve path</param>
        /// <param name="displayMesh">Generated mesh to display curve path</param>
        /// <param name="rootPos">Parent game object root position</param>
        /// <param name="scalingFactor">Point vector scaling factor</param>
        /// <param name="controllerType">Type of parent controller</param>
        public SimpleCurveView(LineRenderer displayLr, TubeMesh displayMesh, Vector3 rootPos, 
            float scalingFactor, AbstractCurveViewController.CurveControllerType controllerType) 
            : base(displayLr, displayMesh, rootPos, scalingFactor, controllerType) 
        {
            // Disable all travel points
            HasTravelPoint = false;
            HasArcLengthTravelPoint = false;
        }

        #endregion Constructors
    }
}
