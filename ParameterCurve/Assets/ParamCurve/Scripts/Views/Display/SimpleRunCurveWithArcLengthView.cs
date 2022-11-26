using Controller.Curve;
//using log4net;
using Model;
using UnityEngine;
using VRKL.MBU;

namespace Views.Display
{
    /// <summary>
    /// View on curve data with an attached game object to display runs along the arc length parametrization based
    /// curve line. The user can start runs on this view. The travel object will then travel along the curve line in
    /// addition to the regular travel object. This is used to display the difference between curve parametrization
    /// methods
    /// </summary>
    public class SimpleRunCurveWithArcLength : SimpleRunCurveView
    {
        #region Private members
        
        /// <summary>
        /// Used to move the associated game object along the curve line based on the arc length parametrization
        /// </summary>
        private Transform ArcLengthTravelObject { get; }

        /// <summary>
        /// Displays the arc length parametrization based tangent of the current point
        /// </summary>
        private LineRenderer ArcLengthTangentLr { get; }
        
        /// <summary>
        /// Displays the arc length parametrization based normal of the current point
        /// </summary>
        private LineRenderer ArcLengthNormalLr { get; }
        
        /// <summary>
        /// Displays the arc length parametrization based binormal of the current point
        /// </summary>
        private LineRenderer ArcLengthBinormalLr { get; }

        /// <summary>
        /// Cached init line renderer width of the arc length parametrization based tangent vector.
        /// Used to scale width based on scaling factor
        /// </summary>
        private readonly float _initArcTangentLrWidth;
        
        /// <summary>
        /// Cached init line renderer width of the arc length parametrization based normal vector.
        /// Used to scale width based on scaling factor
        /// </summary>
        private readonly float _initArcNormalLrWidth;
        
        /// <summary>
        /// Cached init line renderer width of the arc length parametrization based binormal vector.
        /// Used to scale width based on scaling factor
        /// </summary>
        private readonly float _initArcBinormalLrWidth;
    
        /// <summary>
        /// Static log4net logger
        /// </summary>
        //private static readonly ILog Log = LogManager.GetLogger(typeof(SimpleRunCurveView));

        public WaypointManager _arcWpm;
        
        /// <summary>
        /// Local array to store the start and end point of the tangent vector
        /// </summary>
        private readonly Vector3[] _arcTangentArr = new Vector3[2];
        
        /// <summary>
        /// Local array to store the start and end point of the normal vector
        /// </summary>
        private readonly Vector3[] _arcNormalArr = new Vector3[2];
        
        /// <summary>
        /// Local array to store the start and end point of the binormal vector
        /// </summary>
        private readonly Vector3[] _arcBinormalArr = new Vector3[2];
        
        #endregion Private members
        
        #region Constructors
        
        public SimpleRunCurveWithArcLength(
            LineRenderer displayLr,
            TubeMesh displayMesh,
            Vector3 rootPos, 
            float scalingFactor,
            Transform travelObject, 
            Transform arcLengthTravelObject,
            AbstractCurveViewController.CurveControllerType controllerType) 
            : base(displayLr, displayMesh, rootPos, scalingFactor, travelObject, controllerType)
        {
            ArcLengthTravelObject = arcLengthTravelObject;

            HasTravelPoint = true;
            HasArcLengthTravelPoint = true;

            CurveInformationDataset curve = CurrentCurve;
            var renderers = ArcLengthTravelObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
            {
                r.material.color = curve.ArcTravelObjColor;
                r.material.SetColor(EmissionColor, curve.ArcTravelObjColor);    
            }

            // Setup arc length travel object
            if (ArcLengthTravelObject.childCount > 0)
            {
                var firstChild = ArcLengthTravelObject.GetChild(0).gameObject;
                ArcLengthTangentLr = firstChild.GetComponent<LineRenderer>();
                ArcLengthTangentLr.positionCount = 2;
                _initArcTangentLrWidth = ArcLengthTangentLr.widthMultiplier;
            }

            if (ArcLengthTravelObject.childCount > 1)
            {
                var secondChild = ArcLengthTravelObject.GetChild(1).gameObject;
                ArcLengthNormalLr = secondChild.GetComponent<LineRenderer>();
                ArcLengthNormalLr.positionCount = 2;
                _initArcNormalLrWidth = ArcLengthNormalLr.widthMultiplier;
            }

            if (ArcLengthTravelObject.childCount <= 2) return;
            
            var thirdChild = ArcLengthTravelObject.GetChild(2).gameObject;
            ArcLengthBinormalLr = thirdChild.GetComponent<LineRenderer>();
            ArcLengthBinormalLr.positionCount = 2;
            _initArcBinormalLrWidth = ArcLengthBinormalLr.widthMultiplier;

            _arcWpm = new WaypointManager();
        }
        
        #endregion Constructors

        #region Public functions
        
        /// <summary>
        /// Update view
        /// </summary>
        public override void UpdateView()
        {
            base.UpdateView();
            if (!GlobalDataModel.IsRunning) return;
        
            var curve = CurrentCurve; 
        
            // Map points to world space location
            var arcPointArr = curve.ArcLengthWorldPoints.ToArray();
            for (var i = 0; i < arcPointArr.Length; i++)
            {
                var point = arcPointArr[i];
                arcPointArr[i] = MapPointPos(point);//, curve.Is3DCurve);
            }
            
            _arcWpm.SetWaypoints(arcPointArr);
            
            SetArcTravelPoint(); //SetArcTravelPointWPM();
            SetArcMovingFrame();
        }

        /// <summary>
        /// Set the travel object to the next point along the arc length parametrization based curve line
        /// </summary>
        public void SetArcTravelPoint()
        {
            var curve = CurrentCurve; 
            
            // Null checks
            if (ArcLengthTravelObject is null) return;
            if (CurrentPointIndex < 0) return;
        
            // On arrival at the last point, stop driving
            if (CurrentPointIndex == curve.ArcLengthWorldPoints.Count - 1)
            {
                GlobalDataModel.IsRunning = false;
                return;
            }
        
            ArcLengthTravelObject.position = MapPointPos(curve.ArcLengthWorldPoints[CurrentPointIndex]);
            ++CurrentPointIndex;
        }

        public void SetArcTravelPointWPM()
        {
            var curve = CurrentCurve; 
            
            // Null checks
            if (ArcLengthTravelObject is null) return;
            if (CurrentPointIndex < 0) return;
        
            // On arrival at the last point, stop driving
            if (CurrentPointIndex == curve.ArcLengthWorldPoints.Count - 1)
            {
                GlobalDataModel.IsRunning = false;
                return;
            }
        
            ArcLengthTravelObject.position = MapPointPos(curve.ArcLengthWorldPoints[CurrentPointIndex]);
            
            
            
            ++CurrentPointIndex;
        }
        
        // /// <summary>
        // /// Start run
        // /// </summary>
        // public override void StartRun()
        // {
        //     base.StartRun();
        //     
        //     CurrentPointIndex = 0;
        //     GlobalDataModel.IsRunning = true;
        // }
        //
        
        /// <summary>
        /// Set the fresnet equation based moving frame around the next point along the arc length parametrization
        /// based curve line
        /// </summary>
        public void SetArcMovingFrame()
        {
            var curve = CurrentCurve; 
            if (CurrentPointIndex == curve.WorldPoints.Count - 1)
            {
                GlobalDataModel.IsRunning = false;
                return;
            }
        
            // Get curve data
            //var travelObjPosition = TravelObject.position;
            var travelObjPosition = ArcLengthTravelObject.position;
            var tangent = curve.ArcLengthFresnetApparatuses[CurrentPointIndex].Tangent.normalized;
            var normal = curve.ArcLengthFresnetApparatuses[CurrentPointIndex].Normal.normalized;
            var binormal = curve.ArcLengthFresnetApparatuses[CurrentPointIndex].Binormal.normalized;
        
            // Prevent z-buffer fighting
            tangent.z += 0.001f;
            normal.z += 0.001f;
            binormal.z += 0.001f;
            
            // Custom axis flip on examination table
            if (ControllerType == AbstractCurveViewController.CurveControllerType.Table && !curve.Is3DCurve)
            {
                tangent = new Vector3(tangent.x, tangent.z, tangent.y);
                normal = new Vector3(normal.x, normal.z, normal.y);
                binormal = new Vector3(binormal.x, binormal.z, binormal.y);
            }

            
            // Scale moving frame points
            tangent *= ScalingFactor;
            normal *= ScalingFactor;
            binormal *= ScalingFactor;
    
            // Calculate moving frame points
            _arcTangentArr[0] = travelObjPosition;
            _arcTangentArr[1] = travelObjPosition + tangent;
            ArcLengthTangentLr.SetPositions(_arcTangentArr);
            ArcLengthTangentLr.widthMultiplier = _initArcTangentLrWidth * ScalingFactor;
                
            _arcNormalArr[0] = travelObjPosition;
            _arcNormalArr[1] = (travelObjPosition + normal); 
            ArcLengthNormalLr.SetPositions(_arcNormalArr);
            ArcLengthNormalLr.widthMultiplier = _initArcNormalLrWidth * ScalingFactor;
                
            _arcBinormalArr[0] = travelObjPosition;
            _arcBinormalArr[1] = travelObjPosition + binormal;
            ArcLengthBinormalLr.SetPositions(_arcBinormalArr);
            ArcLengthBinormalLr.widthMultiplier = _initArcBinormalLrWidth * ScalingFactor;
            
            
            // var arcTangentArr = new Vector3[2];
            // arcTangentArr[0] = arcObjPos;
            // arcTangentArr[1] = arcObjPos + 
            //                    curve.ArcLengthFresnetApparatuses[CurrentPointIndex].Tangent.normalized * ScalingFactor;
            // ArcLengthTangentLr.SetPositions(arcTangentArr);
            // ArcLengthTangentLr.widthMultiplier = _initArcTangentLrWidth * (ScalingFactor * 0.5f);
            //
            //
            // var arcNormalArr = new Vector3[2];
            // arcNormalArr[0] = arcObjPos;
            // arcNormalArr[1] = arcObjPos + 
            //                   curve.ArcLengthFresnetApparatuses[CurrentPointIndex].Normal.normalized *
            //                   ScalingFactor;
            // ArcLengthNormalLr.SetPositions(arcNormalArr);
            // ArcLengthNormalLr.widthMultiplier = _initArcNormalLrWidth * (ScalingFactor * 0.5f);
            //
            // var arcBinormalArr = new Vector3[2];
            // arcBinormalArr[0] = arcObjPos;
            // arcBinormalArr[1] = arcObjPos + 
            //                     curve.ArcLengthFresnetApparatuses[CurrentPointIndex].Binormal.normalized * 
            //                     ScalingFactor;
            // ArcLengthBinormalLr.SetPositions(arcBinormalArr);
            // ArcLengthBinormalLr.widthMultiplier = _initArcBinormalLrWidth * (ScalingFactor * 0.5f);

            // Log.Debug("arcObjPos: " + arcObjPos +
            //               " arc_jsonTangentPoint: [" + curve.FresnetApparatuses[CurrentPointIndex].Tangent + "] " +
            //               " arc_tangentArr: [" + arcTangentArr[0] + ", " + arcTangentArr[1] + "]" +
            //               " length: " + (arcTangentArr[1] - arcTangentArr[0]).magnitude + "\n" + 
            //               " arc_normalArr: [" + arcNormalArr[0] + ", " + arcNormalArr[1] + "]" +
            //               " length: " + (arcNormalArr[1] - arcNormalArr[0]).magnitude + "\n" + 
            //               " arc_jsonBinormalPoint: [" + curve.FresnetApparatuses[CurrentPointIndex].Binormal + "] " +
            //               " arc_binormalArr: [" + arcBinormalArr[0] + ", " + arcBinormalArr[1] + "]" +
            //               " length: " + (arcBinormalArr[1] - arcBinormalArr[0]).magnitude);
            
            var nextPos = MapPointPos(CurrentPointIndex < curve.ArcLengthWorldPoints.Count - 1 
                ? curve.ArcLengthWorldPoints[CurrentPointIndex + 1] 
                : curve.ArcLengthWorldPoints[CurrentPointIndex]);

            var worldUp = ControllerType == AbstractCurveViewController.CurveControllerType.World 
                ? new Vector3(0f, 0f, -1f) 
                : Vector3.up;
            
            ArcLengthTravelObject.transform.LookAt(nextPos, worldUp);
        }
        
        #endregion Public functions
    }
}
