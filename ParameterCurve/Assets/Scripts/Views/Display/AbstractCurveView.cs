using UnityEngine;
using Views;

public abstract class AbstractCurveView : AbstractView
{
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
    
    
    protected AbstractCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor)
    {
        _displayLr = displayLR;
        _rootPos = rootPos;
        ScalingFactor = scalingFactor;
    }

    public override void UpdateView()
    {
        CurveInformationDataset curve = CurrentCurve; // HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        //ScalingFactor =
        
        var pointArr = curve.worldPoints.ToArray();
        for (var i = 0; i < pointArr.Length; i++)
        {
            pointArr[i] = MapPointPos(pointArr[i]);
        }
        
        _displayLr.positionCount = curve.worldPoints.Count;
        _displayLr.SetPositions(pointArr);
        
        _displayLr.material.color = curve.CurveLineColor;
        _displayLr.material.SetColor(EmissionColor, curve.CurveLineColor);

    }

    protected Vector3 MapPointPos(Vector3 point)
    {
        var newVector = _rootPos + point * ScalingFactor;
        //if (!CurrentCurve.Is3DCurve) newVector.z = 0f;
        return newVector;
    }

    // public void SetCustomDataset(CurveInformationDataset pds)
    // {
    //     CustomDataset = pds;
    //     CustomDataset.CalculateWorldPoints();
    //     HasCustomDataset = true;
    // }
    //
    //
    // public void ClearCustomDataset()
    // {
    //     HasCustomDataset = false;
    //     CustomDataset = null;
    // }


    //public abstract void StartRun();
}
