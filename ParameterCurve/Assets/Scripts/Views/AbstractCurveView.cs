using UnityEngine;

public abstract class AbstractCurveView
{
    private readonly LineRenderer _displayLr;
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
        
        PointDataset curve = HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

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

        _displayLr.positionCount = curve.worldPoints.Count;

        var pointArr = curve.worldPoints.ToArray();

        for (var i = 0; i < pointArr.Length; i++)
        {
            pointArr[i] = MapPointPos(pointArr[i]);
        }        
        _displayLr.SetPositions(pointArr);
        
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
        HasCustomDataset = true;
    }

    public void ClearCustomDataset()
    {
        HasCustomDataset = false;
        CustomDataset = null;
    }
}
