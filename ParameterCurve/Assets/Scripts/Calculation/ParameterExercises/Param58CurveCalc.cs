using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param58CurveCalc : AbstractCurveCalc
{
    public float A = 2f;

    public Param58CurveCalc()
    {
        Name = "Param58";
        ParameterIntervall = new List<float>(linspace(-8f, 8f, 100));
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = 2f * A * t;
        float y = (2f * A) / ((t * t) + 1f);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = 2f * A;
        float y = (4 * A * t) / Mathf.Pow(((t * t) + 1), 2); 
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 0f;
        float y = (4 * A * (3 * (t * t) - 1)) / Mathf.Pow(((t * t) + 1), 3);
        return new Vector3(x, y, 0f).normalized;
    }
}
