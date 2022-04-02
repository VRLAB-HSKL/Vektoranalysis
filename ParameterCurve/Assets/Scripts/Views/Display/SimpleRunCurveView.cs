using Controller.Curve;
using log4net;
using Model;
using UnityEngine;
using VRKL.MBU;

namespace Views.Display
{
    /// <summary>
    /// View on curve data with an attached game object to display runs along the curve line.
    /// The user can start runs on this view. The travel object will then move along the curve line.
    /// </summary>
    public class SimpleRunCurveView : SimpleCurveView
    {
        #region Public members
        
        /// <summary>
        /// Current point index (view local)
        /// </summary>
        public int CurrentPointIndex
        {
            get => _curPointIdx;
            set
            {
                if (value >= CurrentCurve.Points.Count) return;
                _curPointIdx = value;
            }
        }
        
        #endregion
        
        #region Private members
        
        /// <summary>
        /// Used to move the associated game object along the curve line
        /// </summary>
        private Transform TravelObject { get; }

        /// <summary>
        /// Displays the tangent of the current point
        /// </summary>
        private LineRenderer TangentLr { get; }
        
        /// <summary>
        /// Displays the normal of the current point
        /// </summary>
        private LineRenderer NormalLr { get; }
        
        /// <summary>
        /// Displays the binormal of the current point
        /// </summary>
        private LineRenderer BinormalLr { get; }

        /// <summary>
        /// Cached init line renderer width of the tangent vector. Used to scale width based on scaling factor
        /// </summary>
        private readonly float _initTangentLrWidth;
        
        /// <summary>
        /// Cached init line renderer width of the normal vector. Used to scale width based on scaling factor
        /// </summary> 
        private readonly float _initNormalLrWidth;
        
        /// <summary>
        /// Cached init line renderer width of the binormal vector. Used to scale width based on scaling factor
        /// </summary>
        private readonly float _initBinormalLrWidth;
    
        /// <summary>
        /// Local array to store the start and end point of the tangent vector
        /// </summary>
        private readonly Vector3[] _tangentArr = new Vector3[2];
        
        /// <summary>
        /// Local array to store the start and end point of the normal vector
        /// </summary>
        private readonly Vector3[] _normalArr = new Vector3[2];
        
        /// <summary>
        /// Local array to store the start and end point of the binormal vector
        /// </summary>
        private readonly Vector3[] _binormalArr = new Vector3[2];

        /// <summary>
        /// Current point index. Stored locally to enable views running independent of each other
        /// </summary>
        private int _curPointIdx;
    
        public WaypointManager Wpm { get; }
        
        
        /// <summary>
        /// Static log4net logger
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(SimpleRunCurveView));
        
        #endregion Private members
        
        #region Constructors    

        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="displayLr">Line renderer used to display curve line</param>
        /// <param name="rootPos">Origin of curve points</param>
        /// <param name="scalingFactor">Scaling factor used to scale curve line</param>
        /// <param name="travelObject">Game object used to visualize runs along the curve line</param>
        /// <param name="controllerType">Type of curve controller</param>
        public SimpleRunCurveView(LineRenderer displayLr, Vector3 rootPos, float scalingFactor, Transform travelObject,
            AbstractCurveViewController.CurveControllerType controllerType)
            : base(displayLr, rootPos, scalingFactor, controllerType)
        {
            // Set travel object
            TravelObject = travelObject;
            if (travelObject is null)
            {
                Debug.Log("SimpleCVConstructor - TravelObjectEmpty");
            }

            // Allow normal runs
            HasTravelPoint = true;
            HasArcLengthTravelPoint = false;

            // Set travel object color based on init file values
            var curve = CurrentCurve;
            var renderers = TravelObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
            {
                r.material.color = curve.TravelObjColor;
                r.material.SetColor(EmissionColor, curve.TravelObjColor);    
            }
        
            // Setup travel object line renderers        
            if (TravelObject.childCount > 0)
            {
                var firstChild = TravelObject.GetChild(0).gameObject;
                TangentLr = firstChild.GetComponent<LineRenderer>();
                TangentLr.positionCount = 2;
                _initTangentLrWidth = TangentLr.widthMultiplier;
            }

            if (TravelObject.childCount > 1)
            {
                var secondChild = TravelObject.GetChild(1).gameObject;
                NormalLr = secondChild.GetComponent<LineRenderer>();
                NormalLr.positionCount = 2;
                _initNormalLrWidth = NormalLr.widthMultiplier;
            }

            if (TravelObject.childCount <= 2) return;
            
            var thirdChild = TravelObject.GetChild(2).gameObject;
            BinormalLr = thirdChild.GetComponent<LineRenderer>();
            BinormalLr.positionCount = 2;
            _initBinormalLrWidth = BinormalLr.widthMultiplier;


            Wpm = new WaypointManager();  //new Vector3[1], 0.01f, false);
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
            
            if (HasTravelPoint)
            {
                // Map points to world space location
                var pointArr = CurrentCurve.WorldPoints.ToArray();
                for (var i = 0; i < pointArr.Length; i++)
                {
                    var point = pointArr[i];
                    pointArr[i] = MapPointPos(point);//, curve.Is3DCurve);
                }

                Wpm.SetWaypoints(pointArr);
                
                SetTravelObjectWpm();//SetTravelObjectPoint();
                SetMovingFrame();
            }

            if (CurrentPointIndex != CurrentCurve.Points.Count - 1) return;
            
            GlobalDataModel.IsRunning = false;
            Log.Debug("Stopping run...!");
            
            
        }

        /// <summary>
        /// Start run
        /// </summary>
        public override void StartRun()
        {
            CurrentPointIndex = 0;
            GlobalDataModel.IsRunning = true;
        }
    
        /// <summary>
        /// Set the travel object to the next point along the curve line
        /// </summary>
        public void SetTravelObjectPoint()
        {
            // Null checks
            if (!HasTravelPoint) return;
            if (TravelObject is null) return;
            if (CurrentPointIndex < 0) return;

            // On arrival at the last point, stop driving
            if (CurrentPointIndex == CurrentCurve.WorldPoints.Count - 1)
            {
                GlobalDataModel.IsRunning = false; 
                return;
            }

            TravelObject.position = MapPointPos(CurrentCurve.WorldPoints[CurrentPointIndex]);
            
            ++CurrentPointIndex;
        }

        public void SetTravelObjectWpm()
        {
            // Null checks
            if (!HasTravelPoint) return;
            if (TravelObject is null) return;
            if (CurrentPointIndex < 0) return;
            
            // On arrival at the last point, stop driving
            if (CurrentPointIndex == CurrentCurve.WorldPoints.Count - 1)
            {
                GlobalDataModel.IsRunning = false; 
                return;
            }
            
            var pos = Wpm.GetWaypoint();
            var target = Wpm.GetFollowupWaypoint();
            var dist = Vector3.Distance(pos, target);
            TravelObject.position = Wpm.Move(pos, dist);

            
            
            //Debug.Log("pos: " + TravelObject.position);
            //Debug.Log("og: " + pos + ", mapped: " + mapPos);
        }

        /// <summary>
        /// Set the fresnet equation based moving frame around the next point along the curve line
        /// </summary>
        public void SetMovingFrame()
        {
            // Only do this if travel object is even allowed
            if (!HasTravelPoint) return;
        
            var curve = CurrentCurve;
            
            // Stop run on last point
            if (CurrentPointIndex == curve.WorldPoints.Count - 1)
            {
                GlobalDataModel.IsRunning = false;
                return;
            }
        
            // Get curve data
            var travelObjPosition = TravelObject.position;
            var tangent = curve.FresnetApparatuses[CurrentPointIndex].Tangent.normalized;
            var normal = curve.FresnetApparatuses[CurrentPointIndex].Normal.normalized;
            var binormal = curve.FresnetApparatuses[CurrentPointIndex].Binormal.normalized;
        
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
            _tangentArr[0] = travelObjPosition;
            _tangentArr[1] = travelObjPosition + tangent;
            TangentLr.SetPositions(_tangentArr);
            TangentLr.widthMultiplier = _initTangentLrWidth * ScalingFactor;
                
            _normalArr[0] = travelObjPosition;
            _normalArr[1] = (travelObjPosition + normal); 
            NormalLr.SetPositions(_normalArr);
            NormalLr.widthMultiplier = _initNormalLrWidth * ScalingFactor;
                
            _binormalArr[0] = travelObjPosition;
            _binormalArr[1] = travelObjPosition + binormal;
            BinormalLr.SetPositions(_binormalArr);
            BinormalLr.widthMultiplier = _initBinormalLrWidth * ScalingFactor;
        
            Log.Debug("objPos: " + travelObjPosition +
                      " jsonTangentPoint: [" + curve.FresnetApparatuses[CurrentPointIndex].Tangent + "] " +
                      " tangentArr: [" + _tangentArr[0] + ", " + _tangentArr[1] + "]" +
                      " length: " + (_tangentArr[1] - _tangentArr[0]).magnitude + "\n" + 
                      " normalArr: [" + _normalArr[0] + ", " + _normalArr[1] + "]" +
                      " length: " + (_normalArr[1] - _normalArr[0]).magnitude + "\n" + 
                      " jsonBinormalPoint: [" + curve.FresnetApparatuses[CurrentPointIndex].Binormal + "] " +
                      " binormalArr: [" + _binormalArr[0] + ", " + _binormalArr[1] + "]" +
                      " length: " + (_binormalArr[1] - _binormalArr[0]).magnitude);

            // Make sure object is facing in the correct direction
            var nextPos = MapPointPos(CurrentPointIndex < curve.WorldPoints.Count - 1
                ? curve.WorldPoints[CurrentPointIndex + 1] 
                : curve.WorldPoints[CurrentPointIndex]);
            
            var worldUp = ControllerType == AbstractCurveViewController.CurveControllerType.World 
                ? new Vector3(0f, 0f, -1f) 
                : Vector3.up;
        
            //(binormalArr[0] + binormalArr[1]).normalized);
            TravelObject.transform.LookAt(nextPos, worldUp);
        }
        
        
        
        
        #endregion Public functions
    }
}
