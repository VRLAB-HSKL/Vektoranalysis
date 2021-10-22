using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InformationControl : MonoBehaviour
{
    [Header("Header")] 
    public GameObject headerParent;
    public TextMeshProUGUI SourceLabel;
    public TextMeshProUGUI IndexLabel;

    [Header("PointInfo")] 
    public GameObject PointInfoParent;
    public TextMeshProUGUI TLabel;
    public TextMeshProUGUI XLabel;
    public TextMeshProUGUI YLabel;
    public TextMeshProUGUI ZLabel;

    [Header("ArcLengthPointInfo")]
    public GameObject ArcLengthParent;
    public TextMeshProUGUI ArcLengthLabel;
    public TextMeshProUGUI ArcTLabel;
    public TextMeshProUGUI ArcXLabel;
    public TextMeshProUGUI ArcYLabel;
    public TextMeshProUGUI ArcZLabel;

    [Header("TimeDistance")]
    public GameObject TimeDistanceParent;
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

    public CurveViewController ObserevdViewController;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("initFile is null: " + (GlobalData.initFile is null));
        //Debug.Log("initFile.information is null: " + (GlobalData.initFile.information is null));
        
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot || 
            !GlobalData.initFile.ApplicationSettings.InfoSettings.Activated)
        {
            TimeVelocityParent.SetActive(false);
        }
        else
        {
            var xrenderer = TimeVelocityXAxis.GetComponent<MeshRenderer>();
            var yrenderer = TimeVelocityYAxis.GetComponent<MeshRenderer>();

            DataImport.TimeVelocityXAxisLength = xrenderer.bounds.size.x;
            DataImport.TimeVelocityYAxisLength = yrenderer.bounds.size.y;
            TimeVelocityLR = TimeVelocityStart.GetComponent<LineRenderer>();
            _initTimeVelocityTravelPos = TimeVelocityTravelObject.transform.position;
        }
        
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot || 
            !GlobalData.initFile.ApplicationSettings.InfoSettings.Activated)
        {
            // TimeDistanceStart.SetActive(false);
            // TimeDistanceXAxis.SetActive(false);
            // TimeDistanceYAxis.SetActive(false);
            // TimeDistanceTravelObject.SetActive(false);
            
            TimeDistanceParent.SetActive(false);
        }
        else
        {
            var xrenderer = TimeDistanceXAxis.GetComponent<MeshRenderer>();
            var yrenderer = TimeDistanceYAxis.GetComponent<MeshRenderer>();    
            
            DataImport.TimeDistanceXAxisLength = xrenderer.bounds.size.x;
            DataImport.TimeDistanceYAxisLength = yrenderer.bounds.size.y;
            TimeDistLR = TimeDistanceStart.GetComponent<LineRenderer>();
            _initTimeDistTravelPos = TimeDistanceTravelObject.transform.position;
        }

        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.ShowArcLengthData || 
            !GlobalData.initFile.ApplicationSettings.InfoSettings.Activated)
        {
            // ArcLengthLabel.gameObject.SetActive(false);
            // ArcTLabel.gameObject.SetActive(false);
            // ArcXLabel.gameObject.SetActive(false);
            // ArcYLabel.gameObject.SetActive(false);
            // ArcZLabel.gameObject.SetActive(false);
            
            ArcLengthParent.SetActive(false);
        }

        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.ShowPointData || 
            !GlobalData.initFile.ApplicationSettings.InfoSettings.Activated)
        {
            TLabel.gameObject.SetActive(false);
            XLabel.gameObject.SetActive(false);
            YLabel.gameObject.SetActive(false);
            ZLabel.gameObject.SetActive(false);
            
            PointInfoParent.SetActive(false);
        }
        
        
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.ShowBasicInfo || 
            !GlobalData.initFile.ApplicationSettings.InfoSettings.Activated)
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
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.Activated) return;

        var curve = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        var view = ObserevdViewController.CurrentView;
        int pointIndex = (view as SimpleRunCurveView).CurrentPointIndex;

        if (pointIndex > curve.points.Count) return;
        
        //Debug.Log("pointIndex: " + pointIndex + " / " + curve.points.Count);
        
        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowBasicInfo)
        {
            IndexLabel.text = (pointIndex + 1) +
                              " / " +
                             curve.points.Count;    
        }
        

        string floatFormat = "0.#####";

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowPointData)
        {
            if(pointIndex > curve.points.Count)
                Debug.Log("pointIndex: " + pointIndex + " / " + curve.points.Count);
            
            SourceLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].Name;
            TLabel.text = curve?.paramValues[pointIndex].ToString(floatFormat);
            XLabel.text = curve?.points[pointIndex].x.ToString(floatFormat);
            YLabel.text = curve?.points[pointIndex].y.ToString(floatFormat);
            ZLabel.text = curve?.points[pointIndex].z.ToString(floatFormat);    
        }

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowArcLengthData)
        {
            ArcLengthLabel.text = curve?.arcLength.ToString("0.###");
            ArcTLabel.text = curve?.arcLengthParamValues[pointIndex].ToString(floatFormat);
            ArcXLabel.text = curve?.arcLenghtPoints[pointIndex].x.ToString(floatFormat);
            ArcYLabel.text = curve?.arcLenghtPoints[pointIndex].y.ToString(floatFormat);
            ArcZLabel.text = curve?.arcLenghtPoints[pointIndex].z.ToString(floatFormat);    
        }
    }

    private void UpdatePlotTravelObjects()
    {
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.Activated) return;

        var curve = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        var pointIndex = GlobalData.CurrentPointIndex;

        if (pointIndex > curve.points.Count) return;
        
        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
        {
            // Set info plot travel objects
            Vector2 tdPosVec = curve.timeDistancePoints[pointIndex];
            Vector3 tdVec = new Vector3(tdPosVec.x, tdPosVec.y, 0f);
            TimeDistanceTravelObject.transform.position =
                _initTimeDistTravelPos + tdVec;    
        }

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
        {
            Vector2 tvPosVec = curve.timeVelocityPoints[pointIndex];
            Vector3 tvVec = new Vector3(tvPosVec.x, tvPosVec.y, 0f);
            TimeVelocityTravelObject.transform.position =
                _initTimeVelocityTravelPos + tvVec;    
        }

    }

    private void UpdatePlotLineRenderers()
    {
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.Activated) return;

        var curve = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot)
        {
            TimeDistLR.positionCount = curve.timeDistancePoints.Count;
            for (int i = 0; i < curve.timeDistancePoints.Count; i++)
            {
                Vector2 p = curve.timeDistancePoints[i];
                Vector3 newPos = TimeDistanceStart.transform.position;            
                newPos.x += p.x;
                newPos.y += p.y;
                TimeDistLR.SetPosition(i, newPos);
            }    
        }

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
        {
            TimeVelocityLR.positionCount = curve.timeVelocityPoints.Count;
            for (int i = 0; i < curve.timeVelocityPoints.Count; i++)
            {
                Vector2 p = curve.timeVelocityPoints[i];
                Vector3 newPos = TimeVelocityStart.transform.position;
                newPos.x += p.x;
                newPos.y += p.y;
                TimeVelocityLR.SetPosition(i, newPos);
            }    
        }

    }

}
