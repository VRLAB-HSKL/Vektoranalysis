using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param62CurveCalc : AbstractCurveCalc
{
    public Param62CurveCalc()
    {
        Name = "Param62";
        ParameterIntervall = new List<float>(linspace(0f, 2f * Mathf.PI, 200));
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = Mathf.Cos(t);
        float y = Mathf.Sin(t) * Mathf.Cos(t);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;
        float cos2 = (1f + Mathf.Cos(2f * t)) * 0.5f;

        float x = -Mathf.Sin(t);
        float y = cos2 - sin2;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = -Mathf.Cos(t);
        float y = -4f * Mathf.Sin(t) * Mathf.Cos(t);
        return new Vector3(x, y, 0f).normalized;
    }

}
