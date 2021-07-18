using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param61CurveCalc : AbstractCurveCalc
{
    public Param61CurveCalc()
    {
        Name = "Param61";
        ParameterIntervall = new List<float>(linspace(-4f, 4f, 200));
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float t2 = t * t;
        float x = (t2 + 4f * t - 8f) * t2;
        float y = (2f * t - 1f) * t;
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float t2 = t * t;
        float x = 4f * t * (t2 + 3f * t - 4f);
        float y = 4f * t - 1f;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float t2 = t * t;
        float x = 4f * (3f * t2 + 6f * t - 4f);
        float y = 4f;
        return new Vector3(x, y, 0f).normalized;
    }
}
