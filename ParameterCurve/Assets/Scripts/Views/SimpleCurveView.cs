using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCurveView : AbstractCurveView
{
    public SimpleCurveView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor) 
        : base(displayLR, rootPos, scalingFactor) 
    {
        HasTravelPoint = false;
        HasArcLengthPoint = false;
    }

    public override void StartRun() {}
}
