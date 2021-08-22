using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCurveView
{
    public LineRenderer DisplayLR;
    public Transform Root;
    public float ScalingFactor;
    //public Transform TravelObject;
    //public Transform ArcLengthTravelObject;

    public bool HasTravelPoint;
    public bool HasArcLengthPoint;
    
    
    public AbstractCurveView(LineRenderer displayLR, Transform root, float scalingFactor)
    {
        DisplayLR = displayLR;
        Root = root;
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
        return Root.position + point * ScalingFactor;
    }

    private void UpdateWorldObjects()
    {
        //UpdateLineRenderers();
        SetTravelPointAndDisplay();
    }

    //public void UpdateLineRenderers()
    //{
    //    //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;

    //    if (DisplayLR is null)
    //        Debug.Log("Failed to get line renderer component");

    //    DisplayLR.positionCount = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count;
    //    DisplayLR.SetPositions(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.ToArray());

    //    //InfoWall.UpdatePlotLineRenderers();
    //}

    private void SetTravelPointAndDisplay()
    {        
        ++GlobalData.CurrentPointIndex;
    }    
}
