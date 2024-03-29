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
        /// <summary>
        /// Parent game object containing all header elements
        /// </summary>
        [Header("Header")] 
        public GameObject headerParent;
        
        /// <summary>
        /// Source information label
        /// </summary>
        public TextMeshProUGUI SourceLabel;
        
        /// <summary>
        /// Current point index information label
        /// </summary>
        public TextMeshProUGUI IndexLabel;

        /// <summary>
        /// Parent game object containing all point information related elements
        /// </summary>
        [Header("PointInfo")] 
        public GameObject PointInfoParent;
        
        /// <summary>
        /// Current t parameter value label
        /// </summary>
        public TextMeshProUGUI TLabel;
        
        /// <summary>
        /// Current x coordinate value label
        /// </summary>
        public TextMeshProUGUI XLabel;
        
        /// <summary>
        /// Current y coordinate value label
        /// </summary>
        public TextMeshProUGUI YLabel;
        
        /// <summary>
        /// Current y coordinate value label
        /// </summary>
        public TextMeshProUGUI zLabel;

        /// <summary>
        /// Parent game object containing all arc point information related elements
        /// </summary>
        [Header("ArcLengthPointInfo")]
        public GameObject ArcLengthParent;
        
        /// <summary>
        /// Curve arc length value label
        /// </summary>
        public TextMeshProUGUI ArcLengthLabel;
        
        /// <summary>
        /// Current arc t parameter value label
        /// </summary>
        public TextMeshProUGUI ArcTLabel;
        
        /// <summary>
        /// Current arc x coordinate value label
        /// </summary>
        public TextMeshProUGUI ArcXLabel;
        
        /// <summary>
        /// Current arc y coordinate value label
        /// </summary>
        public TextMeshProUGUI ArcYLabel;
        
        /// <summary>
        /// Current arc z coordinate value label
        /// </summary>
        public TextMeshProUGUI ArcZLabel;

        /// <summary>
        /// Parent game object containing all time distance plot elements
        /// </summary>
        [Header("TimeDistance")]
        public GameObject TimeDistanceParent;
        
        /// <summary>
        /// 
        /// </summary>
        public GameObject TimeDistanceStart;
        public GameObject TimeDistanceXAxis;
        public GameObject TimeDistanceYAxis;
        public GameObject TimeDistanceTravelObject;
        private LineRenderer TimeDistLR;
    
        [Header("TimeVelocity")]
        public GameObject TimeVelocityParent;
        public GameObject TimeVelocityStart;
        public GameObject TimeVelocityXAxis;
        public GameObject TimeVelocityYAxis;
        public GameObject TimeVelocityTravelObject;
        private LineRenderer TimeVelocityLR;
    
        private Vector3 _initTimeDistTravelPos;
        private Vector3 _initTimeVelocityTravelPos;

        public CurveViewController ObserevedCurveViewController
        {
            get
            {
                return GlobalDataModel.WorldCurveViewController;
            }
        }

        public void InitPlotLengths()
        {
            var xrenderer = TimeDistanceXAxis.GetComponent<MeshRenderer>();
            var yrenderer = TimeDistanceYAxis.GetComponent<MeshRenderer>();    
            
            //Debug.Log("xRenderer_x: " + xrenderer.bounds.size.x);
            // Debug.Log("xRenderer_y: " + xrenderer.bounds.size.y);
            // Debug.Log("xRenderer_z: " + xrenderer.bounds.size.z);
            // Debug.Log("yRenderer: " + yrenderer.bounds.size);
            
            DataImport.TimeDistanceXAxisLength = xrenderer.bounds.size.x;
            DataImport.TimeDistanceYAxisLength = yrenderer.bounds.size.y;
        
            xrenderer = TimeVelocityXAxis.GetComponent<MeshRenderer>();
            yrenderer = TimeVelocityYAxis.GetComponent<MeshRenderer>();

            DataImport.TimeVelocityXAxisLength = xrenderer.bounds.size.x;
            DataImport.TimeVelocityYAxisLength = yrenderer.bounds.size.y;
        }

        // Start is called before the first frame update
        void Start()
        {
            FindObjectsInScene();
            HideObjectsBasedOnInitFile();
        }


        public void Update()
        {
            UpdateInfoLabels();
            UpdatePlotTravelObjects();
            UpdatePlotLineRenderers();
        }

        private void FindObjectsInScene()
        {
            // Header
            headerParent ??= GameObject.Find("BasicHeader");
            if(SourceLabel is null) GameObject.Find("ValueSource");
            if(IndexLabel is null) GameObject.Find("ValuePoints");
            
            // PointInfo
            PointInfoParent ??= GameObject.Find("PointInfo");
            if(TLabel is null) GameObject.Find("PointValueT");
            if(XLabel is null) GameObject.Find("PointValueX");
            if(YLabel is null) GameObject.Find("PointValueY");
            if(zLabel is null) GameObject.Find("PointValueZ");
            
            // ArcLengthPointInfo
            ArcLengthParent ??= GameObject.Find("ArcLengthInfo");
            if(ArcLengthLabel is null) GameObject.Find("ArcLength");
            if(ArcTLabel is null) GameObject.Find("ArcValueT");
            if(ArcXLabel is null) GameObject.Find("ArcValueX");
            if(ArcYLabel is null) GameObject.Find("ArcValueY");
            if(ArcZLabel is null) GameObject.Find("ArcValueZ");
            
            // TimeDistance
            TimeDistanceParent ??= GameObject.Find("TimeDistanceDiagram");
            TimeDistanceStart ??= GameObject.Find("distancePolyline");
            TimeDistanceXAxis ??= GameObject.Find("distanceXAxis");
            TimeDistanceYAxis ??= GameObject.Find("distanceYAxis");
            TimeDistanceTravelObject ??= GameObject.Find("distancePointer");
            
            // TimeVelocity
            TimeVelocityParent ??= GameObject.Find("TimeVelocityDiagram");
            TimeVelocityStart ??= GameObject.Find("velocityPolyline");
            TimeVelocityXAxis ??= GameObject.Find("velocityXAxis");
            TimeVelocityYAxis ??= GameObject.Find("velocityYAxis");
            TimeVelocityTravelObject ??= GameObject.Find("velocityPointer");
        }

        private void HideObjectsBasedOnInitFile()
        {
            //Debug.Log("initFile is null: " + (GlobalData.initFile is null));
            //Debug.Log("initFile.information is null: " + (GlobalData.initFile.information is null));
        
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                TimeVelocityParent.SetActive(false);
            }
            else
            {
                TimeVelocityLR = TimeVelocityStart.GetComponent<LineRenderer>();
                _initTimeVelocityTravelPos = TimeVelocityTravelObject.transform.position;
            }
        
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {   
                TimeDistanceParent.SetActive(false);
            }
            else
            {   
                TimeDistLR = TimeDistanceStart.GetComponent<LineRenderer>();
                _initTimeDistTravelPos = TimeDistanceTravelObject.transform.position;
            }

            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowArcLengthData || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                ArcLengthParent.SetActive(false);
            }

            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowPointData || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                TLabel.gameObject.SetActive(false);
                XLabel.gameObject.SetActive(false);
                YLabel.gameObject.SetActive(false);
                zLabel.gameObject.SetActive(false);
            
                PointInfoParent.SetActive(false);
            }
        
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowBasicInfo || 
                !GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated)
            {
                SourceLabel.gameObject.SetActive(false);
                IndexLabel.gameObject.SetActive(false);
            
                headerParent.SetActive(false);
            }
        }
        
    
        private void UpdateInfoLabels()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
            
            var view = ObserevedCurveViewController.CurrentView;

            //Debug.Log("test");
            
            var pointIndex = 0;

            bool isRunBasedView = view is SimpleRunCurveView || view is SimpleRunCurveWithArcLength; 
            
            if (isRunBasedView)
            {
                if(view is SimpleRunCurveView)
                {
                    pointIndex = (view as SimpleRunCurveView).CurrentPointIndex;
                }
                
                if(view is SimpleRunCurveWithArcLength)
                {
                    pointIndex = (view as SimpleRunCurveWithArcLength).CurrentPointIndex;
                }
            }
                
            if (pointIndex >= curve.Points.Count) return;
            
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowBasicInfo)
            {
                SourceLabel.text = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].DisplayString;
                
                IndexLabel.text = isRunBasedView ? (pointIndex + 1) + " / " + curve.Points.Count : "0 / 0";
            }

            if (!isRunBasedView) return;
            
            string floatFormat = "0.#####";

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowPointData)
            {
                if(pointIndex > curve.Points.Count)
                    Debug.Log("pointIndex: " + pointIndex + " / " + curve.Points.Count);
            
                TLabel.text = curve?.ParamValues[pointIndex].ToString(floatFormat);
                XLabel.text = curve?.Points[pointIndex].x.ToString(floatFormat);
                YLabel.text = curve?.Points[pointIndex].y.ToString(floatFormat);
                zLabel.text = curve?.Points[pointIndex].z.ToString(floatFormat);    
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowArcLengthData)
            {
                var arcLength = curve.ArcLength.ToString("0.###");
                //Debug.Log("arcLength: " + arcLength);
                ArcLengthLabel.text = arcLength;
                ArcTLabel.text = curve?.ArcLengthParamValues[pointIndex].ToString(floatFormat);
                ArcXLabel.text = curve?.ArcLenghtPoints[pointIndex].x.ToString(floatFormat);
                ArcYLabel.text = curve?.ArcLenghtPoints[pointIndex].y.ToString(floatFormat);
                ArcZLabel.text = curve?.ArcLenghtPoints[pointIndex].z.ToString(floatFormat);    
            }
        }

        private void UpdatePlotTravelObjects()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
            var view = (ObserevedCurveViewController.CurrentView as SimpleRunCurveView);

            if (view is null)
            {
                return;
            }
        
            var pointIndex = view.CurrentPointIndex;

            if (pointIndex < 0) return;
            if (pointIndex >= curve.Points.Count) return;

            //Debug.Log("pointIndex: " + pointIndex);
            
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                if (!(TimeDistanceTravelObject is null))
                {
                    // Set info plot travel objects
                    Vector2 tdPosVec = curve.TimeDistancePoints[pointIndex];
                    Vector3 tdVec = new Vector3(tdPosVec.x, tdPosVec.y, 0f);
                
                    TimeDistanceTravelObject.transform.position =
                        _initTimeDistTravelPos + tdVec;    
                }   
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                if (!(TimeVelocityTravelObject is null))
                {
                    Vector2 tvPosVec = curve.TimeVelocityPoints[pointIndex];
                    Vector3 tvVec = new Vector3(tvPosVec.x, tvPosVec.y, 0f);
                    TimeVelocityTravelObject.transform.position =
                        _initTimeVelocityTravelPos + tvVec;    
                }
            }

        }

        private void UpdatePlotLineRenderers()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
        
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot)
            {
                if (!(TimeDistLR is null))
                {
                    TimeDistLR.positionCount = curve.TimeDistancePoints.Count;
                    for (int i = 0; i < curve.TimeDistancePoints.Count; i++)
                    {
                        Vector2 p = curve.TimeDistancePoints[i];
                        Vector3 newPos = TimeDistanceStart.transform.position;            
                        newPos.x += p.x;
                        newPos.y += p.y;
                        newPos.z -= Random.Range(0f, 0.005f); // 0.0125f;
                        TimeDistLR.SetPosition(i, newPos);
                    }    
                }
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                if (!(TimeVelocityLR is null))
                {
                    TimeVelocityLR.positionCount = curve.TimeVelocityPoints.Count;
                    for (int i = 0; i < curve.TimeVelocityPoints.Count; i++)
                    {
                        Vector2 p = curve.TimeVelocityPoints[i];
                        Vector3 newPos = TimeVelocityStart.transform.position;
                        newPos.x += p.x;
                        newPos.y += p.y;
                        newPos.z -= Random.Range(0f, 0.005f);
                        TimeVelocityLR.SetPosition(i, newPos);
                    }    
                }   
            }

        }

    }
}
