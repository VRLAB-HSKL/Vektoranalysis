using System.Collections;
using System.Collections.Generic;
using Calculation;
using UnityEngine;

public class Param4bCurveCalc : AbstractCurveCalc
{
    public Param4bCurveCalc()
    {
        Name = "Param4b";
        NumOfSamples = 200;
        ParameterRange = new List<float>(Linspace(-1.5f, 1.5f, NumOfSamples));
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
        float t2 = t * t;
        float x = 4f * (t2 * t);
        float y = 3f * t2;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 12f * (t * t);
        float y = 6f * t;
        return new Vector3(x, y, 0f).normalized;
    }
}
