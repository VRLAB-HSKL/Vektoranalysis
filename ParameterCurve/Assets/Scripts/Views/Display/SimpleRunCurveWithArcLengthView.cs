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
        Transform arcLengthTravelObject,
        CurveControllerTye type) : base(displayLR, rootPos, scalingFactor, travelObject, type)
    {
        ArcLengthTravelObject = arcLengthTravelObject;

        HasTravelPoint = true;
        HasArcLengthTravelPoint = true;

        CurveInformationDataset curve = CurrentCurve; //HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        var renderers = ArcLengthTravelObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
        {
            r.material.color = curve.ArcTravelObjColor;
            r.material.SetColor(EmissionColor, curve.ArcTravelObjColor);    
        }
        

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
        if (!GlobalData.IsRunning) return;
        
        SetArcTravelPoint();
        SetArcMovingFrame();

        //Debug.Log("[" + currentPointIndex +"] normalPos: " + TravelObject.position + " ArcPos: " + ArcLengthTravelObject.position);
        //Debug.Log("equal: " + (TravelObject.position == ArcLengthTravelObject.position));
        
    }

    public void SetArcTravelPoint()
    {
        CurveInformationDataset curve = CurrentCurve; //HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        // Null checks
        if (ArcLengthTravelObject is null) return;
        //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
        if (CurrentPointIndex < 0) return;
        
        // if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints.Count)
        // {
        //     //Debug.Log("Stop");
        //     GlobalData.IsDriving = false;
        //     return;
        // }

        // On arrival at the last point, stop driving
        if (CurrentPointIndex >= curve.arcLengthWorldPoints.Count)
        {
            //Debug.Log("Stop");
            GlobalData.IsRunning = false;
            return;
        }
        
        ArcLengthTravelObject.position = MapPointPos(curve.arcLengthWorldPoints[CurrentPointIndex], curve.Is3DCurve);
        ++CurrentPointIndex;

    }

    public void SetArcMovingFrame()
    {
        CurveInformationDataset curve = CurrentCurve; //HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        if (CurrentPointIndex >= curve.worldPoints.Count)
        {
            GlobalData.IsRunning = false;
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
                           curve.arcLengthFresnetApparatuses[CurrentPointIndex].Tangent.normalized * ScalingFactor;
        ArcLengthTangentLR.SetPositions(arcTangentArr);
        ArcLengthTangentLR.widthMultiplier = initArcTangentLRWidth * (ScalingFactor * 0.5f);
        

        Vector3[] arcNormalArr = new Vector3[2];
        arcNormalArr[0] = arcObjPos;
        arcNormalArr[1] = arcObjPos + 
                          curve.arcLengthFresnetApparatuses[CurrentPointIndex].Normal.normalized *
                          ScalingFactor;
        ArcLengthNormalLR.SetPositions(arcNormalArr);
        ArcLengthNormalLR.widthMultiplier = initArcNormalLRWidth * (ScalingFactor * 0.5f);

        
        Vector3[] arcBinormalArr = new Vector3[2];
        arcBinormalArr[0] = arcObjPos;
        arcBinormalArr[1] = arcObjPos + 
                            curve.arcLengthFresnetApparatuses[CurrentPointIndex].Binormal.normalized * 
                            ScalingFactor;
        ArcLengthBinormalLR.SetPositions(arcBinormalArr);
        ArcLengthBinormalLR.widthMultiplier = initArcBinormalLRWidth * (ScalingFactor * 0.5f);

        // Debug.Log("arcObjPos: " + arcObjPos +
        //           " arc_jsonTangentPoint: [" + curve.fresnetApparatuses[CurrentPointIndex].Tangent + "] " +
        //           " arc_tangentArr: [" + arcTangentArr[0] + ", " + arcTangentArr[1] + "]" +
        //           " length: " + (arcTangentArr[1] - arcTangentArr[0]).magnitude + "\n" + 
        //           " arc_normalArr: [" + arcNormalArr[0] + ", " + arcNormalArr[1] + "]" +
        //           " length: " + (arcNormalArr[1] - arcNormalArr[0]).magnitude + "\n" + 
        //           " arc_jsonBinormalPoint: [" + curve.fresnetApparatuses[CurrentPointIndex].Binormal + "] " +
        //           " arc_binormalArr: [" + arcBinormalArr[0] + ", " + arcBinormalArr[1] + "]" +
        //           " length: " + (arcBinormalArr[1] - arcBinormalArr[0]).magnitude);
        
        
        Vector3 nextPos;
        if (CurrentPointIndex < curve.arcLengthWorldPoints.Count - 1)
        {
            nextPos = MapPointPos(curve.arcLengthWorldPoints[CurrentPointIndex + 1], curve.Is3DCurve);
        }
        else
        {
            nextPos = MapPointPos(curve.arcLengthWorldPoints[CurrentPointIndex], curve.Is3DCurve);
        }

        var worldUp = new Vector3(0f, 0f, 1f); //(arcBinormalArr[0] + arcBinormalArr[1]).normalized; 
        ArcLengthTravelObject.transform.LookAt(nextPos, worldUp);
    }
}
