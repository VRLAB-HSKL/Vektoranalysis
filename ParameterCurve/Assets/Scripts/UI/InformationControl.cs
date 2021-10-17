using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using TMPro;
using UnityEngine;

public class InformationControl : MonoBehaviour
{
    [Header("Header")] 
    public GameObject HeaderParent;
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
            
            HeaderParent.SetActive(false);
        }
    }


    public void UpdateInfoLabels()
    {
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.Activated) return;

        int pointIndex = GlobalData.CurrentPointIndex;
        
        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowBasicInfo)
        {
            IndexLabel.text = (pointIndex + 1) +
                              " / " +
                              GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].points.Count;    
        }
        

        string floatFormat = "0.#####";

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowPointData)
        {
            SourceLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].Name;
            TLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].paramValues[pointIndex].ToString(floatFormat);
            XLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].points[pointIndex].x.ToString(floatFormat);
            YLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].points[pointIndex].y.ToString(floatFormat);
            ZLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].points[pointIndex].z.ToString(floatFormat);    
        }

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowArcLengthData)
        {
            ArcLengthLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLength.ToString("0.###");
            ArcTLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthParamValues[pointIndex].ToString(floatFormat);
            ArcXLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLenghtPoints[pointIndex].x.ToString(floatFormat);
            ArcYLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLenghtPoints[pointIndex].y.ToString(floatFormat);
            ArcZLabel.text = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLenghtPoints[pointIndex].z.ToString(floatFormat);    
        }
    }

    public void UpdatePlotTravelObjects()
    {
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.Activated) return;
        
        int pointIndex = GlobalData.CurrentPointIndex;

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
        {
            // Set info plot travel objects
            Vector2 tdPosVec = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeDistancePoints[pointIndex];
            Vector3 tdVec = new Vector3(tdPosVec.x, tdPosVec.y, 0f);
            TimeDistanceTravelObject.transform.position =
                _initTimeDistTravelPos + tdVec;    
        }

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
        {
            Vector2 tvPosVec = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeVelocityPoints[pointIndex];
            Vector3 tvVec = new Vector3(tvPosVec.x, tvPosVec.y, 0f);
            TimeVelocityTravelObject.transform.position =
                _initTimeVelocityTravelPos + tvVec;    
        }

    }

    public void UpdatePlotLineRenderers()
    {
        if (!GlobalData.initFile.ApplicationSettings.InfoSettings.Activated) return;

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeDistancePlot)
        {
            TimeDistLR.positionCount = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeDistancePoints.Count;
            for (int i = 0; i < GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeDistancePoints.Count; i++)
            {
                Vector2 p = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeDistancePoints[i];
                Vector3 newPos = TimeDistanceStart.transform.position;            
                newPos.x += p.x;
                newPos.y += p.y;
                TimeDistLR.SetPosition(i, newPos);
            }    
        }

        if (GlobalData.initFile.ApplicationSettings.InfoSettings.ShowTimeVelocityPlot)
        {
            TimeVelocityLR.positionCount = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeVelocityPoints.Count;
            for (int i = 0; i < GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeVelocityPoints.Count; i++)
            {
                Vector2 p = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].timeVelocityPoints[i];
                Vector3 newPos = TimeVelocityStart.transform.position;
                newPos.x += p.x;
                newPos.y += p.y;
                TimeVelocityLR.SetPosition(i, newPos);
            }    
        }

    }

}
