using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param56CurveCalc : AbstractCurveCalc
{
    public Param56CurveCalc()
    {
        Name = "param56";
        ParameterIntervall = new List<float>(linspace(-4f * Mathf.PI, 4f * Mathf.PI, 200));
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = Mathf.Cos(t);
        float y = Mathf.Atan(t);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = -Mathf.Sin(t);
        float y = 1f / (t * t) + 1f;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = -Mathf.Cos(t);
        float y = -2f / (t * t * t);
        return new Vector3(x, y, 0f).normalized;
    }
}
