using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Import.NewInitFile;
using log4net;
using UnityEngine;

public class SimpleRunCurveView : SimpleCurveView
{
    public static readonly ILog Log = LogManager.GetLogger(typeof(SimpleRunCurveView));
    
    
    protected Transform TravelObject { get; set; }

    public LineRenderer TangentLR;
    public LineRenderer NormalLR;
    public LineRenderer BinormalLR;

    private Vector3 initTravelObjPos;
    private float initTangentLRWidth;
    private float initNormalLRWidth;
    private float initBinormalLRWidth;
    
    
    private Vector3[] tangentArr = new Vector3[2];
    private Vector3[] normalArr = new Vector3[2];
    private Vector3[] binormalArr = new Vector3[2];

    //public int currentPointIndex = 0;
    //protected bool isRunning = false;

    private int _curPointIdx = 0;
    
    public int CurrentPointIndex
    {
        get => _curPointIdx;
        set
        {
            if (value >= CurrentCurve.points.Count) return;
            _curPointIdx = value;
        }
    }
    

    public SimpleRunCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, Transform travelObject,
        CurveControllerTye type)
        : base(displayLR, rootPos, scalingFactor, type)
    {        
        TravelObject = travelObject;

        if (travelObject is null)
        {
            Debug.Log("SimpleCVConstructor - TravelObjectEmpty");
        }

        HasTravelPoint = true;
        HasArcLengthTravelPoint = false;

        CurveInformationDataset curve = CurrentCurve; //HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        var renderers = TravelObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
        {
            r.material.color = curve.TravelObjColor;
            r.material.SetColor(EmissionColor, curve.TravelObjColor);    
        }
        
        // Setup travel object line renderers        
        initTravelObjPos = TravelObject.position;
        if (TravelObject.childCount > 0)
        {
            GameObject firstChild = TravelObject.GetChild(0).gameObject;
            TangentLR = firstChild.GetComponent<LineRenderer>();
            TangentLR.positionCount = 2;
            initTangentLRWidth = TangentLR.widthMultiplier;
        }

        if (TravelObject.childCount > 1)
        {
            GameObject secondChild = TravelObject.GetChild(1).gameObject;
            NormalLR = secondChild.GetComponent<LineRenderer>();
            NormalLR.positionCount = 2;
            initNormalLRWidth = NormalLR.widthMultiplier;
        }

        if (TravelObject.childCount > 2)
        {
            GameObject thirdChild = TravelObject.GetChild(2).gameObject;
            BinormalLR = thirdChild.GetComponent<LineRenderer>();
            BinormalLR.positionCount = 2;
            initBinormalLRWidth = BinormalLR.widthMultiplier;
        }
    }

    public override void UpdateView()
    {
        base.UpdateView();

        if (GlobalData.IsRunning && HasTravelPoint)
        {
            //Debug.Log("UpdateViewSimpleRun_isRunning");
            SetTravelPoint();
            SetMovingFrame();
        }
        
    }

    public override void StartRun()
    {
        CurrentPointIndex = 0;
        GlobalData.IsRunning = true;
    }
    
    public void SetTravelPoint()
    {
        if (!HasTravelPoint) return;
        
        CurveInformationDataset curve = CurrentCurve; // HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        // Null checks
        if (TravelObject is null) return;
        //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
        if (CurrentPointIndex < 0) return;

        // Debug.Log("currentPointIndex: " + currentPointIndex);
        // Debug.Log("worldPointsCount: " + curve.worldPoints.Count);
        
        // On arrival at the last point, stop driving
        if (CurrentPointIndex >= curve.worldPoints.Count)
        {
            //Debug.Log("Stop");
            GlobalData.IsRunning = false; 
            return;
        }

        TravelObject.position = MapPointPos(curve.worldPoints[CurrentPointIndex], curve.Is3DCurve);
        ++CurrentPointIndex;

    }

    public void SetMovingFrame()
    {
        if (!HasTravelPoint) return;
        
        CurveInformationDataset curve = CurrentCurve; // HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        
        if (CurrentPointIndex >= curve.worldPoints.Count)
        {
            GlobalData.IsRunning = false;
            return;
        }

        //int pointIndex = GlobalData.CurrentPointIndex;

        
        
        var travelObjPosition = TravelObject.position;
        var tangent = (curve.fresnetApparatuses[CurrentPointIndex].Tangent).normalized;
        var normal = (curve.fresnetApparatuses[CurrentPointIndex].Normal).normalized;
        var binormal = curve.fresnetApparatuses[CurrentPointIndex].Binormal.normalized;
        
        // Prevent z-buffer fighting
        tangent.z += 0.001f;
        normal.z += 0.001f;
        binormal.z += 0.001f;
        
        if (Type == CurveControllerTye.Table && !curve.Is3DCurve)
        {
            //travelObjPosition = new Vector3(travelObjPosition.x, travelObjPosition.z, travelObjPosition.y);
            tangent = new Vector3(tangent.x, tangent.z, tangent.y);
            normal = new Vector3(normal.x, normal.z, normal.y);
            binormal = new Vector3(binormal.x, binormal.z, binormal.y);
        }
        
        tangent *= ScalingFactor;
        normal *= ScalingFactor;
        binormal *= ScalingFactor;
        
        
        tangentArr[0] = travelObjPosition;
        tangentArr[1] = travelObjPosition + tangent; //.normalized;
        
        //Debug.Log("tan0: " + tangentArr[0] + ", tan1: " + tangentArr[1]);
        
        //tangentArr[1] = MapPointPos(tangentArr[1]);
        
        
        TangentLR.SetPositions(tangentArr);
        TangentLR.widthMultiplier = initTangentLRWidth * ScalingFactor;
                
        
        
        normalArr[0] = travelObjPosition;
        normalArr[1] = (travelObjPosition + normal); //.normalized;
        //normalArr[1] = MapPointPos(normalArr[1]);
        NormalLR.SetPositions(normalArr);
        NormalLR.widthMultiplier = initNormalLRWidth * ScalingFactor;
                
        binormalArr[0] = travelObjPosition;
        binormalArr[1] = travelObjPosition + binormal;
        BinormalLR.SetPositions(binormalArr);
        BinormalLR.widthMultiplier = initBinormalLRWidth * ScalingFactor;
       

        
        // Debug.Log("objPos: " + travelObjPosition +
        //           " jsonTangentPoint: [" + curve.fresnetApparatuses[CurrentPointIndex].Tangent + "] " +
        //           " tangentArr: [" + tangentArr[0] + ", " + tangentArr[1] + "]" +
        //           " length: " + (tangentArr[1] - tangentArr[0]).magnitude + "\n" + 
        //           " normalArr: [" + normalArr[0] + ", " + normalArr[1] + "]" +
        //           " length: " + (normalArr[1] - normalArr[0]).magnitude + "\n" + 
        //           " jsonBinormalPoint: [" + curve.fresnetApparatuses[CurrentPointIndex].Binormal + "] " +
        //           " binormalArr: [" + binormalArr[0] + ", " + binormalArr[1] + "]" +
        //           " length: " + (binormalArr[1] - binormalArr[0]).magnitude);
        
        
        
        Vector3 nextPos;
        if (CurrentPointIndex < curve.worldPoints.Count - 1)
        {
            nextPos = MapPointPos(curve.worldPoints[CurrentPointIndex + 1], curve.Is3DCurve);
        }
        else
        {
            nextPos = MapPointPos(curve.worldPoints[CurrentPointIndex], curve.Is3DCurve);
        }

        // Make sure object is facing in the correct direction
        var worldUp = Type == CurveControllerTye.World ? new Vector3(0f, 0f, -1f) : Vector3.up;
        
        //Debug.Log("worldUp: " + worldUp);
        
        //(binormalArr[0] + binormalArr[1]).normalized);
        TravelObject.transform.LookAt(nextPos, worldUp);
    }
}
