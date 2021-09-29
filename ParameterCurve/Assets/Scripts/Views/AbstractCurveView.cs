using UnityEngine;
using Views;

public abstract class AbstractCurveView : IView
{
    public readonly LineRenderer _displayLr;
    private readonly Vector3 _rootPos;

    public float ScalingFactor;

    public bool HasTravelPoint;
    public bool HasArcLengthPoint;

    protected bool HasCustomDataset;
    protected PointDataset CustomDataset;

    protected AbstractCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor)
    {
        _displayLr = displayLR;
        _rootPos = rootPos;
        ScalingFactor = scalingFactor;
    }

    public virtual void UpdateView()
    {
        // if(_displayLr is null)
        // {
        //     Debug.Log("Failed to get line renderer component");
        // }
        
        // Debug.Log("HasCustomDataset: " + (HasCustomDataset ? "true" : "false"));
        
        PointDataset curve = HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        // Debug.Log("CustomDatasetPointCount: " + curve.points.Count);   
        
        // if(curve is null)
        // {
        //     Debug.Log("Failed to get curve object at current curve index");
        // }
        //
        // if(curve.worldPoints is null)
        // {
        //     Debug.Log("World points collection not initialized in current curve object");
        // }
        //
        // if (curve.worldPoints.Count == 0)
        // {
        //     Debug.Log("World points collection empty in current curve object");
        // }

        
        
        

        var pointArr = curve.worldPoints.ToArray();

        //Debug.Log("pointArrLength01: " + pointArr.Length);
        
        for (var i = 0; i < pointArr.Length; i++)
        {
            pointArr[i] = MapPointPos(pointArr[i]);
        }
        
        //Debug.Log("pointArrLength02: " + pointArr.Length);
        
        _displayLr.positionCount = curve.worldPoints.Count;
        _displayLr.SetPositions(pointArr);
        
        //Debug.Log("displayLR_PointCount: " + _displayLr.positionCount);
        
    }

    protected Vector3 MapPointPos(Vector3 point)
    {
        return _rootPos + point * ScalingFactor;
    }

    private void UpdateWorldObjects()
    {
        SetTravelPointAndDisplay();
    }    

    private void SetTravelPointAndDisplay()
    {        
        ++GlobalData.CurrentPointIndex;
    }

    public void SetCustomDataset(PointDataset pds)
    {
        CustomDataset = pds;
        CustomDataset.CalculateWorldPoints();
        HasCustomDataset = true;
    }

    public void ClearCustomDataset()
    {
        HasCustomDataset = false;
        CustomDataset = null;
    }
}
