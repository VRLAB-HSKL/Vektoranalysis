using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCurveView : AbstractCurveView
{
    public SimpleCurveView(LineRenderer displayLR, Transform root, float scalingFactor) 
        : base(displayLR, root, scalingFactor) 
    {
        HasTravelPoint = false;
        HasArcLengthPoint = false;
    }
}
