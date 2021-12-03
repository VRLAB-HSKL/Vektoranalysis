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


        public void Update()
        {
            UpdateInfoLabels();
            UpdatePlotTravelObjects();
            UpdatePlotLineRenderers();
        }
    
        private void UpdateInfoLabels()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
            
            var view = ObserevedCurveViewController.CurrentView;

            if ((view as SimpleRunCurveView) is null)
                return;
        
            var pointIndex = (view as SimpleRunCurveView).CurrentPointIndex;

            if (pointIndex >= curve.points.Count) return;
        
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowBasicInfo)
            {
                SourceLabel.text = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].DisplayString;
            
                IndexLabel.text = (pointIndex + 1) +
                                  " / " +
                                  curve.points.Count;    
            }

            string floatFormat = "0.#####";

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowPointData)
            {
                if(pointIndex > curve.points.Count)
                    Debug.Log("pointIndex: " + pointIndex + " / " + curve.points.Count);
            
                TLabel.text = curve?.paramValues[pointIndex].ToString(floatFormat);
                XLabel.text = curve?.points[pointIndex].x.ToString(floatFormat);
                YLabel.text = curve?.points[pointIndex].y.ToString(floatFormat);
                zLabel.text = curve?.points[pointIndex].z.ToString(floatFormat);    
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowArcLengthData)
            {
                var arcLength = curve.arcLength.ToString("0.###");
                //Debug.Log("arcLength: " + arcLength);
                ArcLengthLabel.text = arcLength;
                ArcTLabel.text = curve?.arcLengthParamValues[pointIndex].ToString(floatFormat);
                ArcXLabel.text = curve?.arcLenghtPoints[pointIndex].x.ToString(floatFormat);
                ArcYLabel.text = curve?.arcLenghtPoints[pointIndex].y.ToString(floatFormat);
                ArcZLabel.text = curve?.arcLenghtPoints[pointIndex].z.ToString(floatFormat);    
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

        
        
            if (pointIndex > curve.points.Count) return;
        
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                // Set info plot travel objects
                Vector2 tdPosVec = curve.timeDistancePoints[pointIndex];
                Vector3 tdVec = new Vector3(tdPosVec.x, tdPosVec.y, 0f);
                TimeDistanceTravelObject.transform.position =
                    _initTimeDistTravelPos + tdVec;    
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                Vector2 tvPosVec = curve.timeVelocityPoints[pointIndex];
                Vector3 tvVec = new Vector3(tvPosVec.x, tvPosVec.y, 0f);
                TimeVelocityTravelObject.transform.position =
                    _initTimeVelocityTravelPos + tvVec;    
            }

        }

        private void UpdatePlotLineRenderers()
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.Activated) return;

            var curve = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
        
            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot)
            {
                TimeDistLR.positionCount = curve.timeDistancePoints.Count;
                for (int i = 0; i < curve.timeDistancePoints.Count; i++)
                {
                    Vector2 p = curve.timeDistancePoints[i];
                    Vector3 newPos = TimeDistanceStart.transform.position;            
                    newPos.x += p.x;
                    newPos.y += p.y;
                    newPos.z -= Random.Range(0f, 0.005f); // 0.0125f;
                    TimeDistLR.SetPosition(i, newPos);
                }    
            }

            if (GlobalDataModel.InitFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
            {
                TimeVelocityLR.positionCount = curve.timeVelocityPoints.Count;
                for (int i = 0; i < curve.timeVelocityPoints.Count; i++)
                {
                    Vector2 p = curve.timeVelocityPoints[i];
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
