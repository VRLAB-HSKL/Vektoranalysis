using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRunCurveView : SimpleCurveView
{
    private Transform TravelObject { get; set; }

    public LineRenderer TangentLR;
    public LineRenderer NormalLR;
    public LineRenderer BinormalLR;

    private Vector3 initTravelObjPos;

    private Vector3[] tangentArr = new Vector3[2];
    private Vector3[] normalArr = new Vector3[2];
    private Vector3[] binormalArr = new Vector3[2];

    public SimpleRunCurveView(LineRenderer displayLR, Transform travelObject) : base(displayLR)
    {        

        TravelObject = travelObject;

        if (travelObject is null)
        {
            Debug.Log("SimpleCVConstructor - TravelObjectEmpty");
        }

        HasTravelPoint = true;
        HasArcLengthPoint = false;

        // Setup travel object line renderers        
        initTravelObjPos = TravelObject.position;
        if (TravelObject.childCount > 0)
        {
            GameObject firstChild = TravelObject.GetChild(0).gameObject;
            TangentLR = firstChild.GetComponent<LineRenderer>();
            TangentLR.positionCount = 2;
        }

        if (TravelObject.childCount > 1)
        {
            GameObject secondChild = TravelObject.GetChild(1).gameObject;
            NormalLR = secondChild.GetComponent<LineRenderer>();
            NormalLR.positionCount = 2;
        }

        if (TravelObject.childCount > 2)
        {
            GameObject thirdChild = TravelObject.GetChild(2).gameObject;
            BinormalLR = thirdChild.GetComponent<LineRenderer>();
            BinormalLR.positionCount = 2;
        }
    }

    public override void UpdateView()
    {
        base.UpdateView();
        SetTravelPoint();
        SetMovingFrame();
    }

    private void SetTravelPoint()
    {
        // Null checks
        if (TravelObject is null) return;
        if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
        if (GlobalData.CurrentPointIndex < 0) return;

        // On arrival at the last point, stop driving
        if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
        {
            GlobalData.IsDriving = false;
            return;
        }

        int pointIndex = GlobalData.CurrentPointIndex;
        TravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex];
        //ArcLengthTravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex];

        Vector3 nextPos = Vector3.zero;
        if (pointIndex < GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count - 1)
        {
            nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex + 1];
        }
        else
        {
            nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex];
        }

        TravelObject.transform.LookAt(nextPos, (binormalArr[0] + binormalArr[1]).normalized);
    }

    private void SetMovingFrame()
    {
        tangentArr[0] = TravelObject.position;
        tangentArr[1] = TravelObject.position + 
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[GlobalData.CurrentPointIndex].Tangent;
        TangentLR.SetPositions(tangentArr);
                
        normalArr[0] = TravelObject.position;
        normalArr[1] = TravelObject.position + 
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[GlobalData.CurrentPointIndex].Normal;
        NormalLR.SetPositions(normalArr);
                
        binormalArr[0] = TravelObject.position;
        binormalArr[1] = TravelObject.position + 
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[GlobalData.CurrentPointIndex].Binormal;
        BinormalLR.SetPositions(binormalArr);
    }
}
