using System;
using System.Linq;
using Controller.Curve;
//using log4net;
using Model;
using UnityEngine;
using VRKL.MBU;

namespace Views.Display
{
    /// <summary>
    /// Abstract base class for all views on curve data
    /// </summary>
    public abstract class AbstractCurveView
    {
        #region Public members
    
        /// <summary>
        /// Scaling factor, applied to all points in the view
        /// </summary>
        public float ScalingFactor = 1f;
    
        /// <summary>
        /// True if this view has a game object to display runs 
        /// </summary>
        public bool HasTravelPoint { get; protected set; }
    
        /// <summary>
        /// True if this view has a game object to display arc length parametrization based runs
        /// </summary>
        public bool HasArcLengthTravelPoint { get; protected set; }
    
        #endregion Public members
        
        #region Protected members
        
        /// <summary>
        /// Type of parent controller
        /// </summary>
        protected readonly AbstractCurveViewController.CurveControllerType ControllerType;
    
        /// <summary>
        /// Current curve being displayed in the view. This model data is accessed through the static
        /// <see cref="GlobalDataModel"/> class, based on global curve index <see cref="GlobalDataModel.CurrentCurveIndex"/>
        /// </summary>
        protected static CurveInformationDataset CurrentCurve => 
            GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
        
        /// <summary>
        /// Line renderer to display curve path
        /// </summary>
        protected readonly LineRenderer DisplayLr;

        protected TubeMesh DisplayMesh;
        
        /// <summary>
        /// Cached material property key to change material color of line on startup
        /// </summary>
        protected static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        
        
        #endregion Protected members
        
        #region Private members
        
        /// <summary>
        /// Position of root object, used to translate point vectors
        /// </summary>
        public readonly Vector3 _rootPos;

        
        
        /// <summary>
        /// Static log4net logger
        /// </summary>
        //private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractCurveView));
        
        
        
        #endregion Private members
        
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="displayLr">Line renderer to display curve path</param>
        /// <param name="rootPos">Parent game object root position</param>
        /// <param name="scalingFactor">Point vector scaling factor</param>
        /// <param name="controllerType">Type of parent controller</param>
        protected AbstractCurveView(LineRenderer displayLr, TubeMesh displayMesh, Vector3 rootPos, 
            float scalingFactor, 
            AbstractCurveViewController.CurveControllerType controllerType)
        {
            //Log.Info("AbstractCurveView.ArgumentConstructor()");
            DisplayMesh = displayMesh;
            //DisplayLr = displayLr;
            _rootPos = rootPos;
            
            
            ScalingFactor = scalingFactor;
            
            ControllerType = controllerType;
        }
        
        #endregion Constructors

        #region Public functions
        
        /// <summary>
        /// Update view with current information
        /// </summary>
        public virtual void UpdateView()
        {
            var curve = CurrentCurve; 
        
            // Map points to world space location
            var pointArr = curve.WorldPoints.ToArray();
            for (var i = 0; i < pointArr.Length; i++)
            {
                var point = pointArr[i];
                pointArr[i] = MapPointPos(point);//, curve.Is3DCurve);
            }

            // Set line renderer positions
            // DisplayLr.positionCount = pointArr.Length;
            // DisplayLr.SetPositions(pointArr);
        
            // Update material
            // DisplayLr.material.color = curve.CurveLineColor;
            // DisplayLr.material.SetColor(EmissionColor, curve.CurveLineColor);

            if(ControllerType == AbstractCurveViewController.CurveControllerType.Table)
                DisplayMesh.SetScalingFactor(ScalingFactor);

            var normals = curve.FresnetApparatuses.Select(point => point.Normal).ToList();
            
            DisplayMesh.GenerateFieldMesh(curve.WorldPoints, normals);

            var rotateCurve
                = ControllerType == AbstractCurveViewController.CurveControllerType.Table && !CurrentCurve.Is3DCurve;

            DisplayMesh.transform.rotation = Quaternion.Euler(rotateCurve ? 90f : 0f, 0f, 0f);
            
            //_wpm = new WaypointManager(curve.worldPoints.ToArray(), 0.01f, false);

        }

        /// <summary>
        /// Starts a run on the view, if a travel object is present
        /// </summary>
        public virtual void StartRun() {}
        
        #endregion Public functions
        
        #region Protected functions
        
        /// <summary>
        /// Maps a point to its correct world space location, based on parent game object position and scaling factor
        /// </summary>
        /// <param name="point">Source point vector</param>
        /// <returns>Transformed point</returns>
        protected Vector3 MapPointPos(Vector3 point)//, bool is3d)
        {
            // Flip curve upright based on controller type and dimension
            var flip = ControllerType == AbstractCurveViewController.CurveControllerType.Table && !CurrentCurve.Is3DCurve;
        
            // Calculate new point
            var newVector = flip 
                ? new Vector3(point.x, point.z, point.y) 
                : new Vector3(point.x, point.y, point.z);


            if (ControllerType == AbstractCurveViewController.CurveControllerType.Table)
            {
                newVector = _rootPos + newVector * ScalingFactor;
            }
            else
            {
                newVector = _rootPos + newVector; //;ScalingFactor;    
            }
            
            return newVector;
        }

        #endregion Protected functions
    }
}
