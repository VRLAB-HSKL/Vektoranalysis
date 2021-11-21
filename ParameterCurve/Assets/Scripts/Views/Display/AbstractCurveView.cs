using Controller.Curve;
using log4net;
using UnityEngine;

namespace Views.Display
{
    /// <summary>
    /// Abstract base class for all views on curve data
    /// </summary>
    public abstract class AbstractCurveView // : AbstractView
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractCurveView));
    
        /// <summary>
        /// Scaling factor, applied to all points in the view
        /// </summary>
        public float ScalingFactor;
    
        /// <summary>
        /// True if this view has a game object to display runs 
        /// </summary>
        public bool HasTravelPoint { get; protected set; }
    
        /// <summary>
        /// True if this view has a game object to display arc length parametrization based runs
        /// </summary>
        public bool HasArcLengthTravelPoint { get; protected set; }
    
        /// <summary>
        /// Type of parent controller
        /// ToDo: Does this make sense in the pattern ?
        /// </summary>
        protected readonly AbstractCurveViewController.CurveControllerType ControllerType;
    
        /// <summary>
        /// Line renderer to display curve path
        /// </summary>
        protected readonly LineRenderer DisplayLr;
    
        /// <summary>
        /// Position of root object, used to translate point vectors
        /// </summary>
        private readonly Vector3 _rootPos;

        /// <summary>
        /// Cached material property key to change material color of line on startup
        /// </summary>
        protected static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        /// <summary>
        /// Current curve being displayed in the view. This model data is accessed through the static
        /// <see cref="GlobalData"/> class, based on global curve index <see cref="GlobalData.CurrentCurveIndex"/>
        /// </summary>
        protected static CurveInformationDataset CurrentCurve => GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="displayLr">Line renderer to display curve path</param>
        /// <param name="rootPos">Parent game object root position</param>
        /// <param name="scalingFactor">Point vector scaling factor</param>
        /// <param name="controllerType">Type of parent controller</param>
        protected AbstractCurveView(LineRenderer displayLr, Vector3 rootPos, float scalingFactor, AbstractCurveViewController.CurveControllerType controllerType)
        {
            Log.Info("AbstractCurveView.ArgumentConstructor()");
            ControllerType = controllerType;
            DisplayLr = displayLr;
            _rootPos = rootPos;
            ScalingFactor = scalingFactor;
        }

        /// <summary>
        /// Update view with current information
        /// </summary>
        public virtual void UpdateView()
        {
            Log.Info("AbstractCurveView.UpdateView()");
        
            var curve = CurrentCurve; 
        
            // Map points to world space location
            var pointArr = curve.worldPoints.ToArray();
            for (var i = 0; i < pointArr.Length; i++)
            {
                var point = pointArr[i];
                pointArr[i] = MapPointPos(point);//, curve.Is3DCurve);
            }

            // Set line renderer positions
            DisplayLr.positionCount = pointArr.Length;
            DisplayLr.SetPositions(pointArr);
        
            // Update material
            DisplayLr.material.color = curve.CurveLineColor;
            DisplayLr.material.SetColor(EmissionColor, curve.CurveLineColor);
        }

        /// <summary>
        /// Maps a point to its correct world space location, based on parent game object position and scaling factor
        /// </summary>
        /// <param name="point">Source point vector</param>
        /// <returns>Transformed point</returns>
        protected Vector3 MapPointPos(Vector3 point)//, bool is3d)
        {
            // Flip curve upright based on controller type and dimension
            var flip = 
                ControllerType == AbstractCurveViewController.CurveControllerType.Table && !CurrentCurve.Is3DCurve;
        
            // Calculate new point
            var newVector = flip ? new Vector3(point.x, point.z, point.y) : new Vector3(point.x, point.y, point.z);
            newVector = _rootPos + newVector * ScalingFactor;
            return newVector;
        }

        public virtual void StartRun()
        {
               
        }
    }
}
