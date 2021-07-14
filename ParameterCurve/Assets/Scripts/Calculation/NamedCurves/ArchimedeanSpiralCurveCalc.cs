using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchimedeanSpiralCurveCalc : AbstractCurveCalc
{
    public static float A = 0.125f;
    public static float B = 0.25f;

    public ArchimedeanSpiralCurveCalc()
    {
        Name = "ArchimedeanSpiral";
        ParameterIntervall = new List<float>(linspace(0f, 6f * Mathf.PI, 200));
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float r_i = A + B * t;
        float x = r_i * Mathf.Cos(t);
        float y = r_i * Mathf.Sin(t);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float r_i = A + B * t;
        float x = -r_i * Mathf.Sin(t);
        float y = r_i * Mathf.Cos(t);
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float r_i = A + B * t;
        float x = -r_i * Mathf.Cos(t);
        float y = -r_i * Mathf.Sin(t);
        return new Vector3(x, y, 0f).normalized;
    }

}
    

