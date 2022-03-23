using Controller.Curve;
using UnityEngine;

namespace Views.Display
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
        /// <param name="rootPos">Parent game object root position</param>
        /// <param name="scalingFactor">Point vector scaling factor</param>
        /// <param name="controllerType">Type of parent controller</param>
        public SimpleCurveView(LineRenderer displayLr, Vector3 rootPos, 
            float scalingFactor, AbstractCurveViewController.CurveControllerType controllerType) 
            : base(displayLr, rootPos, scalingFactor, controllerType) 
        {
            // Disable all travel points
            HasTravelPoint = false;
            HasArcLengthTravelPoint = false;
        }

        #endregion Constructors
    }
}
