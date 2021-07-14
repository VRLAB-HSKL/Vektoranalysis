using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvoluteCurveCalc : AbstractCurveCalc
{
    public float Radius = 1f;
    public float Alpha = 0f;

    public InvoluteCurveCalc()
    {
        Name = "Involute";
        ParameterIntervall = new List<float>(linspace(0f, 0.5f * Mathf.PI, 100));
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = Radius * (Mathf.Cos(t) + (t - Alpha) * Mathf.Sin(t));
        float y = Radius * (Mathf.Sin(t) - (t - Alpha) * Mathf.Cos(t));
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = Radius * (t - Alpha) * Mathf.Cos(t);
        float y = Radius * (t - Alpha) * Mathf.Sin(t);
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = Radius * ((Alpha - t) * Mathf.Sin(t) + Mathf.Cos(t));
        float y = Radius * ((t - Alpha) * Mathf.Cos(t) + Mathf.Sin(t));
        return new Vector3(x, y, 0f).normalized;
    }
}
