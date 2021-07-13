using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param4bCurveCalc : AbstractCurveCalc
{
    public Param4bCurveCalc()
    {
        Name = "param4b";
        ParameterIntervall = new List<float>(linspace(-1.5f, 1.5f, 200));
        Is3DCurve = false;
    }
        

    protected override Vector3 CalculatePoint(float t)
    {
        float t3 = t * t * t;
        float x = t3 * t - 1f;
        float y = t3 + 1f;
        return new Vector3(x, y, 0f);        
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = 4f * (t * t * t);
        float y = 3f * (t * t);
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 12f * (t * t);
        float y = 6f * t;
        return new Vector3(x, y, 0f).normalized;
    }
}
