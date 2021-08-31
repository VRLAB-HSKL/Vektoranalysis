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

    public SimpleRunCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, Transform travelObject)
        : base(displayLR, rootPos, scalingFactor)
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

        int pointIndex = GlobalData.CurrentPointIndex;
        // On arrival at the last point, stop driving
        if (pointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
        {
            //Debug.Log("Stop");
            GlobalData.IsDriving = false;
            return;
        }

        TravelObject.position = MapPointPos(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex]);

        

        ++GlobalData.CurrentPointIndex;

        
    }

    private void SetMovingFrame()
    {
        if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
        {
            GlobalData.IsDriving = false;
            return;
        }

        int pointIndex = GlobalData.CurrentPointIndex;

        tangentArr[0] = TravelObject.position;
        tangentArr[1] = (TravelObject.position + 
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[GlobalData.CurrentPointIndex].Tangent).normalized;
        TangentLR.SetPositions(tangentArr);
                
        normalArr[0] = TravelObject.position;
        normalArr[1] = (TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[GlobalData.CurrentPointIndex].Normal).normalized;
        NormalLR.SetPositions(normalArr);
                
        binormalArr[0] = TravelObject.position;
        binormalArr[1] = (TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[GlobalData.CurrentPointIndex].Binormal);
        BinormalLR.SetPositions(binormalArr);

        Vector3 nextPos;
        if (pointIndex < GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count - 1)
        {
            nextPos = MapPointPos(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex + 1]);
        }
        else
        {
            nextPos = MapPointPos(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex]);
        }

        TravelObject.transform.LookAt(nextPos, (binormalArr[0] + binormalArr[1]).normalized);
    }
}
