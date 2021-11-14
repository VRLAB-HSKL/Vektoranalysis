using log4net;
using UnityEngine;

namespace Views.Display
{
    public class SimpleRunCurveView : SimpleCurveView
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SimpleRunCurveView));

        private Transform TravelObject { get; }

        private readonly LineRenderer _tangentLr;
        private readonly LineRenderer _normalLr;
        private readonly LineRenderer _binormalLr;

        private Vector3 _initTravelObjPos;
        private readonly float _initTangentLrWidth;
        private readonly float _initNormalLrWidth;
        private readonly float _initBinormalLrWidth;
    
        private readonly Vector3[] _tangentArr = new Vector3[2];
        private readonly Vector3[] _normalArr = new Vector3[2];
        private readonly Vector3[] _binormalArr = new Vector3[2];

        private int _curPointIdx;
    
        /// <summary>
        /// Current point index (view local)
        /// </summary>
        public int CurrentPointIndex
        {
            get => _curPointIdx;
            set
            {
                if (value >= CurrentCurve.points.Count) return;
                _curPointIdx = value;
            }
        }
    

        public SimpleRunCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, Transform travelObject,
            CurveControllerTye controllerType)
            : base(displayLR, rootPos, scalingFactor, controllerType)
        {        
            TravelObject = travelObject;

            if (travelObject is null)
            {
                Debug.Log("SimpleCVConstructor - TravelObjectEmpty");
            }

            HasTravelPoint = true;
            HasArcLengthTravelPoint = false;

            CurveInformationDataset curve = CurrentCurve;
            var renderers = TravelObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
            {
                r.material.color = curve.TravelObjColor;
                r.material.SetColor(EmissionColor, curve.TravelObjColor);    
            }
        
            // Setup travel object line renderers        
            _initTravelObjPos = TravelObject.position;
            if (TravelObject.childCount > 0)
            {
                GameObject firstChild = TravelObject.GetChild(0).gameObject;
                _tangentLr = firstChild.GetComponent<LineRenderer>();
                _tangentLr.positionCount = 2;
                _initTangentLrWidth = _tangentLr.widthMultiplier;
            }

            if (TravelObject.childCount > 1)
            {
                GameObject secondChild = TravelObject.GetChild(1).gameObject;
                _normalLr = secondChild.GetComponent<LineRenderer>();
                _normalLr.positionCount = 2;
                _initNormalLrWidth = _normalLr.widthMultiplier;
            }

            if (TravelObject.childCount > 2)
            {
                GameObject thirdChild = TravelObject.GetChild(2).gameObject;
                _binormalLr = thirdChild.GetComponent<LineRenderer>();
                _binormalLr.positionCount = 2;
                _initBinormalLrWidth = _binormalLr.widthMultiplier;
            }
        }

        public override void UpdateView()
        {
            base.UpdateView();

            if (!GlobalData.IsRunning) return;
            
            if (HasTravelPoint)
            {
                SetTravelPoint();
                SetMovingFrame();
            }

            if (CurrentPointIndex > CurrentCurve.points.Count)
            {
                GlobalData.IsRunning = false;
            }
        
        }

        public void StartRun()
        {
            CurrentPointIndex = 0;
            GlobalData.IsRunning = true;
        }
    
        public void SetTravelPoint()
        {
            // Null checks
            if (!HasTravelPoint) return;
            if (TravelObject is null) return;
            if (CurrentPointIndex < 0) return;

            
            //CurveInformationDataset curve = CurrentCurve;
        
            
        
            // On arrival at the last point, stop driving
            if (CurrentPointIndex >= CurrentCurve.worldPoints.Count)
            {
                GlobalData.IsRunning = false; 
                return;
            }

            TravelObject.position = MapPointPos(CurrentCurve.worldPoints[CurrentPointIndex]);
            ++CurrentPointIndex;
        }

        public void SetMovingFrame()
        {
            if (!HasTravelPoint) return;
        
            CurveInformationDataset curve = CurrentCurve;
            if (CurrentPointIndex >= curve.worldPoints.Count)
            {
                GlobalData.IsRunning = false;
                return;
            }
        
            var travelObjPosition = TravelObject.position;
            var tangent = (curve.fresnetApparatuses[CurrentPointIndex].Tangent).normalized;
            var normal = (curve.fresnetApparatuses[CurrentPointIndex].Normal).normalized;
            var binormal = curve.fresnetApparatuses[CurrentPointIndex].Binormal.normalized;
        
            // Prevent z-buffer fighting
            tangent.z += 0.001f;
            normal.z += 0.001f;
            binormal.z += 0.001f;
        
            if (ControllerType == CurveControllerTye.Table && !curve.Is3DCurve)
            {
                //travelObjPosition = new Vector3(travelObjPosition.x, travelObjPosition.z, travelObjPosition.y);
                tangent = new Vector3(tangent.x, tangent.z, tangent.y);
                normal = new Vector3(normal.x, normal.z, normal.y);
                binormal = new Vector3(binormal.x, binormal.z, binormal.y);
            }
        
            tangent *= ScalingFactor;
            normal *= ScalingFactor;
            binormal *= ScalingFactor;

            _tangentArr[0] = travelObjPosition;
            _tangentArr[1] = travelObjPosition + tangent;
            _tangentLr.SetPositions(_tangentArr);
            _tangentLr.widthMultiplier = _initTangentLrWidth * ScalingFactor;
                
            _normalArr[0] = travelObjPosition;
            _normalArr[1] = (travelObjPosition + normal); 
            _normalLr.SetPositions(_normalArr);
            _normalLr.widthMultiplier = _initNormalLrWidth * ScalingFactor;
                
            _binormalArr[0] = travelObjPosition;
            _binormalArr[1] = travelObjPosition + binormal;
            _binormalLr.SetPositions(_binormalArr);
            _binormalLr.widthMultiplier = _initBinormalLrWidth * ScalingFactor;
       

        
            Log.Debug("objPos: " + travelObjPosition +
                      " jsonTangentPoint: [" + curve.fresnetApparatuses[CurrentPointIndex].Tangent + "] " +
                      " tangentArr: [" + _tangentArr[0] + ", " + _tangentArr[1] + "]" +
                      " length: " + (_tangentArr[1] - _tangentArr[0]).magnitude + "\n" + 
                      " normalArr: [" + _normalArr[0] + ", " + _normalArr[1] + "]" +
                      " length: " + (_normalArr[1] - _normalArr[0]).magnitude + "\n" + 
                      " jsonBinormalPoint: [" + curve.fresnetApparatuses[CurrentPointIndex].Binormal + "] " +
                      " binormalArr: [" + _binormalArr[0] + ", " + _binormalArr[1] + "]" +
                      " length: " + (_binormalArr[1] - _binormalArr[0]).magnitude);
        
        
        
            Vector3 nextPos;
            if (CurrentPointIndex < curve.worldPoints.Count - 1)
            {
                nextPos = MapPointPos(curve.worldPoints[CurrentPointIndex + 1]); //, curve.Is3DCurve);
            }
            else
            {
                nextPos = MapPointPos(curve.worldPoints[CurrentPointIndex]);//, curve.Is3DCurve);
            }

            // Make sure object is facing in the correct direction
            var worldUp = ControllerType == CurveControllerTye.World ? new Vector3(0f, 0f, -1f) : Vector3.up;
        
            //Debug.Log("worldUp: " + worldUp);
        
            //(binormalArr[0] + binormalArr[1]).normalized);
            TravelObject.transform.LookAt(nextPos, worldUp);
        }
    }
}
