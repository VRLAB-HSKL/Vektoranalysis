using UnityEngine;
using Views;

public abstract class AbstractCurveView : AbstractView
{
    public CurveControllerTye Type;
    public readonly LineRenderer _displayLr;
    private readonly Vector3 _rootPos;

    public float ScalingFactor;

    public bool HasTravelPoint { get; set; }
    public bool HasArcLengthTravelPoint;

    // protected bool HasCustomDataset;
    // protected CurveInformationDataset CustomDataset;
    protected static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");


    public CurveInformationDataset CurrentCurve
    {
        get
        {
            //

            var ret = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
                //GlobalData.SelectionExercises[GlobalData.CurrentExerciseIndex].Datasets;

            return ret;
        }
    }

    protected AbstractCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, CurveControllerTye type)
    {
        Type = type;
        _displayLr = displayLR;
        _rootPos = rootPos;
        ScalingFactor = scalingFactor;
    }

    public override void UpdateView()
    {
        CurveInformationDataset curve = CurrentCurve; 
        
        var pointArr = curve.worldPoints.ToArray();
        for (var i = 0; i < pointArr.Length; i++)
        {
            var point = pointArr[i];

            pointArr[i] = MapPointPos(point, curve.Is3DCurve);
        }

        _displayLr.positionCount = pointArr.Length;
        _displayLr.SetPositions(pointArr);
        
        _displayLr.material.color = curve.CurveLineColor;
        _displayLr.material.SetColor(EmissionColor, curve.CurveLineColor);
    }

    protected Vector3 MapPointPos(Vector3 point, bool is3d)
    {
        if (Type == CurveControllerTye.Table && !is3d)
        {
            Vector3 newVector = new Vector3(point.x, point.z, point.y);
            newVector = _rootPos + newVector * ScalingFactor;
            return newVector;
        }
        else
        {
            Vector3 newVector = new Vector3(point.x, point.y, point.z);
            newVector = _rootPos + newVector * ScalingFactor;
            return newVector;
        }
        
    }

}
