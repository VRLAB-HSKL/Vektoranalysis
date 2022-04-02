using Controller.Curve;
using Import;
using Model;
using TMPro;
using UnityEngine;
using Views.Display;

namespace UI
{
    /// <summary>
    /// Control class for the information wall
    /// </summary>
    public class InformationControl : MonoBehaviour
    {
        #region Public members
        
        /// <summary>
        /// Parent game object containing all header elements
        /// </summary>
        [Header("Header")] 
        public GameObject headerParent;
        
        /// <summary>
        /// Source information label
        /// </summary>
        public TextMeshProUGUI sourceLabel;
        
        /// <summary>
        /// Current point index information label
        /// </summary>
        public TextMeshProUGUI indexLabel;

        /// <summary>
        /// Parent game object containing all point information related elements
        /// </summary>
        [Header("PointInfo")] 
        public GameObject pointInfoParent;
        
        /// <summary>
        /// Current t parameter value label
        /// </summary>
        public TextMeshProUGUI tLabel;
        
        /// <summary>
        /// Current x coordinate value label
        /// </summary>
        public TextMeshProUGUI xLabel;
        
        /// <summary>
        /// Current y coordinate value label
        /// </summary>
        public TextMeshProUGUI yLabel;
        
        /// <summary>
        /// Current y coordinate value label
        /// </summary>
        public TextMeshProUGUI zLabel;

        /// <summary>
        /// Parent game object containing all arc point information related elements
        /// </summary>
        [Header("ArcLengthPointInfo")]
        public GameObject arcLengthParent;
        
        /// <summary>
        /// Curve arc length value label
        /// </summary>
        public TextMeshProUGUI arcLengthLabel;
        
        /// <summary>
        /// Current arc t parameter value label
        /// </summary>
        public TextMeshProUGUI arcTLabel;
        
        /// <summary>
        /// Current arc x coordinate value label
        /// </summary>
        public TextMeshProUGUI arcXLabel;
        
        /// <summary>
        /// Current arc y coordinate value label
        /// </summary>
        public TextMeshProUGUI arcYLabel;
        
        /// <summary>
        /// Current arc z coordinate value label
        /// </summary>
        public TextMeshProUGUI arcZLabel;

        /// <summary>
        /// Parent game object containing all time distance plot elements
        /// </summary>
        [Header("TimeDistance")]
        public GameObject timeDistanceParent;
        
        /// <summary>
        /// Start of the time distance plot
        /// </summary>
        public GameObject timeDistanceStart;
        
        /// <summary>
        /// X Axis of the time distance plot
        /// </summary>
        public GameObject timeDistanceXAxis;
        
        /// <summary>
        /// Y Axis of the time distance plot
        /// </summary>
        public GameObject timeDistanceYAxis;
        
        /// <summary>
        /// Travel object of the time distance plot
        /// </summary>
        public GameObject timeDistanceTravelObject;
        
        
    
        /// <summary>
        /// Parent game object containing all time velocity plot elements
        /// </summary>
        [Header("TimeVelocity")]
        public GameObject timeVelocityParent;
        
        /// <summary>
        /// Start of the time velocity plot
        /// </summary>
        public GameObject timeVelocityStart;
        
        /// <summary>
        /// X axis of the time velocity plot
        /// </summary>
        public GameObject timeVelocityXAxis;
        
        /// <summary>
        /// Y axis of the time velocity plot
        /// </summary>
        public GameObject timeVelocityYAxis;
        
        /// <summary>
        /// Travel object of the time velocity plot
        /// </summary>
        public GameObject timeVelocityTravelObject;
        
        #endregion Public members
        
        #region Private members
        
        /// <summary>
        /// Line renderer for the curve on the time distance plot
        /// </summary>
        private LineRenderer _timeDistLr;
        
        /// <summary>
        /// Line renderer for the curve of the time velocity plot
        /// </summary>
        private LineRenderer _timeVelocityLr;
    
        /// <summary>
        /// Cached initial position of the time distance plot travel object, used to reset travel object
        /// </summary>
        private Vector3 _initTimeDistTravelPos;
        
        /// <summary>
        /// Cached initial position of the time velocity plot travel object, used to reset travel object
        /// </summary>
        private Vector3 _initTimeVelocityTravelPos;

        /// <summary>
        /// Number format for displaying float values
        /// </summary>
        private const string FloatFormat = "0.#####";

        /// <summary>
        /// Local wrapper for static world instance
        /// </summary>
        private static CurveViewController ObservedCurveViewController => GlobalDataModel.WorldCurveViewController;

        #endregion Private members
        
        #region Public functions
        
        /// <summary>
        /// Initializes length parameters based on rendered axis lengths 
        /// </summary>
        public void InitPlotLengths()
        {
            var xRenderer = timeDistanceXAxis.GetComponent<MeshRenderer>();
            var yRenderer = timeDistanceYAxis.GetComponent<MeshRenderer>();    
            
            DataImport.TimeDistanceXAxisLength = xRenderer.bounds.size.x;
            DataImport.TimeDistanceYAxisLength = yRenderer.bounds.size.y;
            xRenderer = timeVelocityXAxis.GetComponent<MeshRenderer>();
            yRenderer = timeVelocityYAxis.GetComponent<MeshRenderer>();
            DataImport.TimeVelocityXAxisLength = xRenderer.bounds.size.x;
            DataImport.TimeVelocityYAxisLength = yRenderer.bounds.size.y;
        }

        /// <summary>
        /// Unity Update function
        /// =====================
        ///
        /// Core game loop, is called once per frame
        /// </summary>
        public void Update()
        {
            UpdateInfoLabels();
            UpdatePlotTravelObjects();
            UpdatePlotLineRenderers();
        }
        
        #endregion Public functions
        
        #region Private functions
        
        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update, after
        /// <see>
        ///     <cref>Awake</cref>
        /// </see>
        /// </summary>
        private void Start()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                timeVelocityParent.SetActive(false);
            }
            else
            {
                _timeVelocityLr = timeVelocityStart.GetComponent<LineRenderer>();
                _initTimeVelocityTravelPos = timeVelocityTravelObject.transform.position;
            }
        
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {   
                timeDistanceParent.SetActive(false);
            }
            else
            {   
                _timeDistLr = timeDistanceStart.GetComponent<LineRenderer>();
                _initTimeDistTravelPos = timeDistanceTravelObject.transform.position;
            }

            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowArcLengthData || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                arcLengthParent.SetActive(false);
            }

            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowPointData || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                tLabel.gameObject.SetActive(false);
                xLabel.gameObject.SetActive(false);
                yLabel.gameObject.SetActive(false);
                zLabel.gameObject.SetActive(false);
            
                pointInfoParent.SetActive(false);
            }
        
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowBasicInfo || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                sourceLabel.gameObject.SetActive(false);
                indexLabel.gameObject.SetActive(false);
            
                headerParent.SetActive(false);
            }
        }
        
        /// <summary>
        /// Update all activated labels
        /// </summary>
        private void UpdateInfoLabels()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
            var view = ObservedCurveViewController.CurrentView;

            var pointIndex = 0;
            var isRunBasedView = view is SimpleRunCurveView; 
            
            if (isRunBasedView)
            {
                pointIndex = (view as SimpleRunCurveView).CurrentPointIndex;

                if(view is SimpleRunCurveWithArcLength length)
                {
                    pointIndex = length.CurrentPointIndex;
                }
            }
                
            if (pointIndex >= curve.Points.Count) return;
            
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowBasicInfo)
            {
                sourceLabel.text = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].DisplayString;
                
                indexLabel.text = isRunBasedView ? pointIndex + 1 + " / " + curve.Points.Count : "0 / 0";
            }

            if (!isRunBasedView) return;
            
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowPointData)
            {
                if(pointIndex > curve.Points.Count)
                    Debug.Log("pointIndex: " + pointIndex + " / " + curve.Points.Count);
            
                tLabel.text = curve.ParamValues[pointIndex].ToString(FloatFormat);
                xLabel.text = curve.Points[pointIndex].x.ToString(FloatFormat);
                yLabel.text = curve.Points[pointIndex].y.ToString(FloatFormat);
                zLabel.text = curve.Points[pointIndex].z.ToString(FloatFormat);    
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowArcLengthData)
            {
                var arcLength = curve.ArcLength.ToString("0.###");
                arcLengthLabel.text = arcLength;
                arcTLabel.text = curve.ArcLengthParamValues[pointIndex].ToString(FloatFormat);
                arcXLabel.text = curve.ArcLenghtPoints[pointIndex].x.ToString(FloatFormat);
                arcYLabel.text = curve.ArcLenghtPoints[pointIndex].y.ToString(FloatFormat);
                arcZLabel.text = curve.ArcLenghtPoints[pointIndex].z.ToString(FloatFormat);    
            }
        }

        /// <summary>
        /// Update all activated plot travel objects
        /// </summary>
        private void UpdatePlotTravelObjects()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];

            if (!(ObservedCurveViewController.CurrentView is SimpleRunCurveView view))
            {
                return;
            }
        
            var pointIndex = view.CurrentPointIndex;
            if (pointIndex > curve.Points.Count) return;

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                // Set info plot travel objects
                var tdPosVec = curve.TimeDistancePoints[pointIndex];
                var tdVec = new Vector3(tdPosVec.x, tdPosVec.y, 0f);
                timeDistanceTravelObject.transform.position =
                    _initTimeDistTravelPos + tdVec;    
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                var tvPosVec = curve.TimeVelocityPoints[pointIndex];
                var tvVec = new Vector3(tvPosVec.x, tvPosVec.y, 0f);
                timeVelocityTravelObject.transform.position =
                    _initTimeVelocityTravelPos + tvVec;    
            }

        }

        /// <summary>
        /// Update all activated plot line renderers
        /// </summary>
        private void UpdatePlotLineRenderers()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
        
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot)
            {
                _timeDistLr.positionCount = curve.TimeDistancePoints.Count;
                for (var i = 0; i < curve.TimeDistancePoints.Count; i++)
                {
                    var p = curve.TimeDistancePoints[i];
                    var newPos = timeDistanceStart.transform.position;            
                    newPos.x += p.x;
                    newPos.y += p.y;
                    newPos.z -= Random.Range(0f, 0.005f); // 0.0125f;
                    _timeDistLr.SetPosition(i, newPos);
                }    
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                _timeVelocityLr.positionCount = curve.TimeVelocityPoints.Count;
                for (var i = 0; i < curve.TimeVelocityPoints.Count; i++)
                {
                    var p = curve.TimeVelocityPoints[i];
                    var newPos = timeVelocityStart.transform.position;
                    newPos.x += p.x;
                    newPos.y += p.y;
                    newPos.z -= Random.Range(0f, 0.005f);
                    _timeVelocityLr.SetPosition(i, newPos);
                }    
            }

        }
        
        #endregion Private functions

    }
}
