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

    public int currentPointIndex = 0;
    private bool isRunning = false;
    

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

        if (isRunning)
        {
            SetTravelPoint();
            SetMovingFrame();    
        }
        
    }

    public override void StartRun()
    {
        isRunning = true;
    }
    
    private void SetTravelPoint()
    {
        PointDataset curve = HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        
        
        // Null checks
        if (TravelObject is null) return;
        //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
        if (currentPointIndex < 0) return;

        //int pointIndex = GlobalData.CurrentPointIndex;
        // On arrival at the last point, stop driving
        if (currentPointIndex >= curve.worldPoints.Count)
        {
            //Debug.Log("Stop");
            GlobalData.IsDriving = false;
            return;
        }

        TravelObject.position = MapPointPos(curve.worldPoints[currentPointIndex]);

        

        ++currentPointIndex;

        
    }

    private void SetMovingFrame()
    {
        PointDataset curve = HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        if (currentPointIndex >= curve.worldPoints.Count)
        {
            GlobalData.IsDriving = false;
            return;
        }

        //int pointIndex = GlobalData.CurrentPointIndex;

        var travelObjPosition = TravelObject.position;
        tangentArr[0] = travelObjPosition;
        tangentArr[1] = (travelObjPosition + 
            curve.fresnetApparatuses[currentPointIndex].Tangent).normalized;
        TangentLR.SetPositions(tangentArr);
                
        normalArr[0] = travelObjPosition;
        normalArr[1] = (travelObjPosition + curve.fresnetApparatuses[currentPointIndex].Normal).normalized;
        NormalLR.SetPositions(normalArr);
                
        binormalArr[0] = travelObjPosition;
        binormalArr[1] = (travelObjPosition + curve.fresnetApparatuses[currentPointIndex].Binormal);
        BinormalLR.SetPositions(binormalArr);

        Vector3 nextPos;
        if (currentPointIndex < curve.worldPoints.Count - 1)
        {
            nextPos = MapPointPos(curve.worldPoints[currentPointIndex + 1]);
        }
        else
        {
            nextPos = MapPointPos(curve.worldPoints[currentPointIndex]);
        }

        // Make sure object is facing in the correct direction
        TravelObject.transform.LookAt(nextPos, (binormalArr[0] + binormalArr[1]).normalized);
    }
}
