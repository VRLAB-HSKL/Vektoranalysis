using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCurveView : AbstractCurveView
{
    public SimpleCurveView(LineRenderer displayLR) : base(displayLR) 
    {
        HasTravelPoint = false;
        HasArcLengthPoint = false;
    }
}
