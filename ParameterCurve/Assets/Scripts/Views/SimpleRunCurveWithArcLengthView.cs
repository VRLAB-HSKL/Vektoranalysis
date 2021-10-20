using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRunCurveWithArcLength : SimpleRunCurveView
{
    public Transform ArcLengthTravelObject;

    public LineRenderer ArcLengthTangentLR;
    public LineRenderer ArcLengthNormalLR;
    public LineRenderer ArcLengthBinormalLR;

    private Vector3 initArcLenghtTravelObjPos;
    private float initArcTangentLRWidth;
    private float initArcNormalLRWidth;
    private float initArcBinormalLRWidth;
    
    public SimpleRunCurveWithArcLength(
        LineRenderer displayLR, 
        Vector3 rootPos, 
        float scalingFactor,
        Transform travelObject, 
        Transform arcLengthTravelObject) : base(displayLR, rootPos, scalingFactor, travelObject)
    {
        ArcLengthTravelObject = arcLengthTravelObject;

        HasTravelPoint = true;
        HasArcLengthPoint = true;

        // Setup arc length travel object
        initArcLenghtTravelObjPos = ArcLengthTravelObject.position;
        if (ArcLengthTravelObject.childCount > 0)
        {
            GameObject firstChild = ArcLengthTravelObject.GetChild(0).gameObject;
            ArcLengthTangentLR = firstChild.GetComponent<LineRenderer>();
            ArcLengthTangentLR.positionCount = 2;
            initArcTangentLRWidth = ArcLengthTangentLR.widthMultiplier;
        }

        if (ArcLengthTravelObject.childCount > 1)
        {
            GameObject secondChild = ArcLengthTravelObject.GetChild(1).gameObject;
            ArcLengthNormalLR = secondChild.GetComponent<LineRenderer>();
            ArcLengthNormalLR.positionCount = 2;
            initArcNormalLRWidth = ArcLengthNormalLR.widthMultiplier;
        }

        if (ArcLengthTravelObject.childCount > 2)
        {
            GameObject thirdChild = ArcLengthTravelObject.GetChild(2).gameObject;
            ArcLengthBinormalLR = thirdChild.GetComponent<LineRenderer>();
            ArcLengthBinormalLR.positionCount = 2;
            initArcBinormalLRWidth = ArcLengthBinormalLR.widthMultiplier;
        }
    }

    public override void UpdateView()
    {
        base.UpdateView();
        if (isRunning)
        {
            SetArcTravelPoint();
            SetArcMovingFrame();    
        }
        
        //Debug.Log("[" + currentPointIndex +"] normalPos: " + TravelObject.position + " ArcPos: " + ArcLengthTravelObject.position);
        //Debug.Log("equal: " + (TravelObject.position == ArcLengthTravelObject.position));
        
    }

    private void SetArcTravelPoint()
    {
        PointDataset curve = HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        // Null checks
        if (ArcLengthTravelObject is null) return;
        //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
        if (currentPointIndex < 0) return;
        
        // if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints.Count)
        // {
        //     //Debug.Log("Stop");
        //     GlobalData.IsDriving = false;
        //     return;
        // }

        // On arrival at the last point, stop driving
        if (currentPointIndex >= curve.worldPoints.Count)
        {
            //Debug.Log("Stop");
            GlobalData.IsDriving = false;
            isRunning = false;   
            return;
        }
        
        ArcLengthTravelObject.position = MapPointPos(curve.worldPoints[currentPointIndex]);
        ++currentPointIndex;

    }

    private void SetArcMovingFrame()
    {
        PointDataset curve = HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        
        if (currentPointIndex >= curve.worldPoints.Count)
        {
            GlobalData.IsDriving = false;
            isRunning = false;
            return;
        }
        
        //int pointIndex = GlobalData.CurrentCurveIndex;
        // ArcLengthTravelObject.position =
        //     MapPointPos(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex]);

        // Debug.Log("curve is null: " + (curve is null));
        // Debug.Log("curve.arcLengthWorldPoints is null: " + (curve.arcLengthWorldPoints is null));
        // Debug.Log("curve.arcLengthWorldPoints Count: " + (curve.arcLengthWorldPoints.Count));
        //
        // Debug.Log("curve.arcLengthFresnetFrames is null: " + (curve.arcLengthFresnetApparatuses is null));
        // Debug.Log("curve.arcLengthFresnetFrames Count: " + (curve.arcLengthFresnetApparatuses.Count));
        
        
        var arcObjPos = ArcLengthTravelObject.position;
        Vector3[] arcTangentArr = new Vector3[2];
        arcTangentArr[0] = arcObjPos;
        arcTangentArr[1] = arcObjPos + 
                           curve.arcLengthFresnetApparatuses[GlobalData.CurrentPointIndex].Tangent.normalized * ScalingFactor;
        ArcLengthTangentLR.SetPositions(arcTangentArr);
        ArcLengthTangentLR.widthMultiplier = initArcTangentLRWidth * ScalingFactor;
        

        Vector3[] arcNormalArr = new Vector3[2];
        arcNormalArr[0] = arcObjPos;
        arcNormalArr[1] = arcObjPos + 
                          curve.arcLengthFresnetApparatuses[GlobalData.CurrentPointIndex].Normal.normalized *
                          ScalingFactor;
        ArcLengthNormalLR.SetPositions(arcNormalArr);
        ArcLengthNormalLR.widthMultiplier = initArcNormalLRWidth * ScalingFactor;

        
        Vector3[] arcBinormalArr = new Vector3[2];
        arcBinormalArr[0] = arcObjPos;
        arcBinormalArr[1] = arcObjPos + 
                            curve.arcLengthFresnetApparatuses[GlobalData.CurrentPointIndex].Binormal.normalized * 
                            ScalingFactor;
        ArcLengthBinormalLR.SetPositions(arcBinormalArr);
        ArcLengthBinormalLR.widthMultiplier = initArcBinormalLRWidth * ScalingFactor;

        Debug.Log("arcObjPos: " + arcObjPos +
                  " arc_jsonTangentPoint: [" + curve.fresnetApparatuses[currentPointIndex].Tangent + "] " +
                  " arc_tangentArr: [" + arcTangentArr[0] + ", " + arcTangentArr[1] + "]" +
                  " length: " + (arcTangentArr[1] - arcTangentArr[0]).magnitude + "\n" + 
                  " arc_normalArr: [" + arcNormalArr[0] + ", " + arcNormalArr[1] + "]" +
                  " length: " + (arcNormalArr[1] - arcNormalArr[0]).magnitude + "\n" + 
                  " arc_jsonBinormalPoint: [" + curve.fresnetApparatuses[currentPointIndex].Binormal + "] " +
                  " arc_binormalArr: [" + arcBinormalArr[0] + ", " + arcBinormalArr[1] + "]" +
                  " length: " + (arcBinormalArr[1] - arcBinormalArr[0]).magnitude);
        
        
        Vector3 nextPos;
        if (currentPointIndex < curve.arcLengthWorldPoints.Count - 1)
        {
            nextPos = MapPointPos(curve.arcLengthWorldPoints[currentPointIndex + 1]);
        }
        else
        {
            nextPos = MapPointPos(curve.arcLengthWorldPoints[currentPointIndex]);
        }

        ArcLengthTravelObject.transform.LookAt(
            nextPos, 
            (arcBinormalArr[0] + arcBinormalArr[1]).normalized);
    }
}
