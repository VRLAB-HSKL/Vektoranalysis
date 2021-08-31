using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCurveView
{
    public LineRenderer DisplayLR;
    public Vector3 RootPos;
    public float ScalingFactor;
    //public Transform TravelObject;
    //public Transform ArcLengthTravelObject;

    public bool HasTravelPoint;
    public bool HasArcLengthPoint;
    
    
    public AbstractCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor)
    {
        DisplayLR = displayLR;
        RootPos = rootPos;
        ScalingFactor = scalingFactor;
        //UpdateView();
    }

    public virtual void UpdateView()
    {
        if(DisplayLR is null)
        {
            Debug.Log("Failed to get line renderer component");
        }
        
        PointDataset curve = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        if(curve is null)
        {
            Debug.Log("Failed to get curve object at current curve index");
        }

        if(curve.worldPoints is null)
        {
            Debug.Log("World points collection not initialized in current curve object");
        }

        if (curve.worldPoints.Count == 0)
        {
            Debug.Log("World points collection empty in current curve object");
        }

        DisplayLR.positionCount = curve.worldPoints.Count;

        Vector3[] pointArr = curve.worldPoints.ToArray();

        for (int i = 0; i < pointArr.Length; i++)
        {
            pointArr[i] = MapPointPos(pointArr[i]);
        }        
        DisplayLR.SetPositions(pointArr);
        
    }

    protected Vector3 MapPointPos(Vector3 point)
    {
        return RootPos + point * ScalingFactor;
    }

    private void UpdateWorldObjects()
    {
        SetTravelPointAndDisplay();
    }    

    private void SetTravelPointAndDisplay()
    {        
        ++GlobalData.CurrentPointIndex;
    }    
}
