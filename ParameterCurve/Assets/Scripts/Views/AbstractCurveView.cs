using UnityEngine;
using Views;

public abstract class AbstractCurveView : AbstractView
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

    public override void UpdateView()
    {
        PointDataset curve = HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        //ScalingFactor =
        
        var pointArr = curve.worldPoints.ToArray();
        for (var i = 0; i < pointArr.Length; i++)
        {
            pointArr[i] = MapPointPos(pointArr[i]);
        }
        
        _displayLr.positionCount = curve.worldPoints.Count;
        _displayLr.SetPositions(pointArr);
    }

    protected Vector3 MapPointPos(Vector3 point)
    {
        return _rootPos + point * ScalingFactor;
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


    //public abstract void StartRun();
}
