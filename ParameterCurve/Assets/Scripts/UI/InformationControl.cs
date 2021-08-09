using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationControl : MonoBehaviour
{
    [Header("Header")]
    public TextMeshProUGUI SourceLabel;
    public TextMeshProUGUI IndexLabel;

    [Header("PointInfo")]
    public TextMeshProUGUI TLabel;
    public TextMeshProUGUI XLabel;
    public TextMeshProUGUI YLabel;
    public TextMeshProUGUI ZLabel;

    [Header("ArcLengthPointInfo")]
    public TextMeshProUGUI ArcLengthLabel;
    public TextMeshProUGUI ArcTLabel;
    public TextMeshProUGUI ArcXLabel;
    public TextMeshProUGUI ArcYLabel;
    public TextMeshProUGUI ArcZLabel;



    [Header("TimeDistance")]
    public GameObject TimeDistanceStart;
    public GameObject TimeDistanceXAxis;
    public GameObject TimeDistanceYAxis;
    public GameObject TimeDistanceTravelObject;
    private LineRenderer TimeDistLR;
    

    private Vector3 initTimeDistTravelPos;

    [Header("TimeVelocity")]
    public GameObject TimeVelocityStart;
    public GameObject TimeVelocityXAxis;
    public GameObject TimeVelocityYAxis;
    public GameObject TimeVelocityTravelObject;
    private LineRenderer TimeVelocityLR;
    

    private Vector3 initTimeVelocityTravelPos;



    // Start is called before the first frame update
    void Awake()
    {
        MeshRenderer xrenderer = TimeDistanceXAxis.GetComponent<MeshRenderer>();
        MeshRenderer yrenderer = TimeDistanceYAxis.GetComponent<MeshRenderer>();

        DataImport.TimeDistanceXAxisLength = xrenderer.bounds.size.x;
        DataImport.TimeDistanceYAxisLength = yrenderer.bounds.size.y;
        TimeDistLR = TimeDistanceStart.GetComponent<LineRenderer>();
        //Debug.Log("TiDiLRStart");
        initTimeDistTravelPos = TimeDistanceTravelObject.transform.position;

        xrenderer = TimeVelocityXAxis.GetComponent<MeshRenderer>();
        yrenderer = TimeVelocityYAxis.GetComponent<MeshRenderer>();

        DataImport.TimeVelocityXAxisLength = xrenderer.bounds.size.x;
        DataImport.TimeVelocityYAxisLength = yrenderer.bounds.size.y;
        TimeVelocityLR = TimeVelocityStart.GetComponent<LineRenderer>();
        initTimeVelocityTravelPos = TimeVelocityTravelObject.transform.position;
    }


    public void UpdateInfoLabels()
    {
        int pointIndex = GlobalData.CurrentPointIndex;
        IndexLabel.text = (pointIndex + 1) +
            " / " +
            GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points.Count;

        string floatFormat = "0.#####";

        SourceLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].Name;
        TLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].paramValues[pointIndex].ToString(floatFormat);
        XLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points[pointIndex].x.ToString(floatFormat);
        YLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points[pointIndex].y.ToString(floatFormat);
        ZLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points[pointIndex].z.ToString(floatFormat);

        ArcLengthLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLength.ToString("0.###");
        ArcTLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLengthParamValues[pointIndex].ToString(floatFormat);
        ArcXLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLenghtPoints[pointIndex].x.ToString(floatFormat);
        ArcYLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLenghtPoints[pointIndex].y.ToString(floatFormat);
        ArcZLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLenghtPoints[pointIndex].z.ToString(floatFormat);
    }

    public void UpdatePlotTravelObjects()
    {
        int pointIndex = GlobalData.CurrentPointIndex;
        // Set info plot travel objects
        Vector2 tdPosVec = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeDistancePoints[pointIndex];
        Vector3 tdVec = new Vector3(tdPosVec.x, tdPosVec.y, 0f);
        TimeDistanceTravelObject.transform.position =
            initTimeDistTravelPos + tdVec;

        Vector2 tvPosVec = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeVelocityPoints[pointIndex];
        Vector3 tvVec = new Vector3(tvPosVec.x, tvPosVec.y, 0f);
        TimeVelocityTravelObject.transform.position =
            initTimeVelocityTravelPos + tvVec;
    }

    public void UpdatePlotLineRenderers()
    {
        TimeDistLR.positionCount = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeDistancePoints.Count;
        for (int i = 0; i < GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeDistancePoints.Count; i++)
        {
            Vector2 p = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeDistancePoints[i];
            Vector3 newPos = TimeDistanceStart.transform.position;            
            newPos.x += p.x;
            newPos.y += p.y;
            TimeDistLR.SetPosition(i, newPos);
        }

        TimeVelocityLR.positionCount = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeVelocityPoints.Count;
        for (int i = 0; i < GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeVelocityPoints.Count; i++)
        {
            Vector2 p = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeVelocityPoints[i];
            Vector3 newPos = TimeVelocityStart.transform.position;
            newPos.x += p.x;
            newPos.y += p.y;
            TimeVelocityLR.SetPosition(i, newPos);
        }
    }

}
