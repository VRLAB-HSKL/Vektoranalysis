using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCurveView : AbstractCurveView
{
    public SimpleCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, CurveControllerTye type) 
        : base(displayLR, rootPos, scalingFactor, type) 
    {
        HasTravelPoint = false;
        HasArcLengthTravelPoint = false;
    }

    public override void StartRun() {}
}
