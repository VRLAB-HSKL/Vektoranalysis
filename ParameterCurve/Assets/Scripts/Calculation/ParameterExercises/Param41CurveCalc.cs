using System.Collections;
using System.Collections.Generic;
using Calculation;
using UnityEngine;

public class Param41CurveCalc : AbstractCurveCalc
{
    public Param41CurveCalc()
    {
        Name = "Param41";
        NumOfSamples = 200;
        ParameterRange = new List<float>(Linspace(-2f, 2f, NumOfSamples));
    }    

    protected override Vector3 CalculatePoint(float t)
    {
        float x = (t * t * t * t) - 1f;
        float y = (t * t * t) - 1f;
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = 4f * t * t * t;
        float y = 3f * t * t;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 12f * t * t;
        float y = 6f * t;
        return new Vector3(x, y, 0f).normalized;
    }


}
