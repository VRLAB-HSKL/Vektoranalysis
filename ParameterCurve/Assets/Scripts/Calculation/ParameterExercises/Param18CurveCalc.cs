using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param18CurveCalc : AbstractCurveCalc
{
    public Param18CurveCalc()
    {
        Name = "Param18";
        ParameterIntervall = new List<float>(linspace(1f, Mathf.Epsilon, 2000));
        Is3DCurve = true;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = t * t;
        float y = 2f * t;
        float z = Mathf.Log(t);
        return new Vector3(x, y, z);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = 2f * t;
        float y = 2f;
        float z = (float)(1f / t);
        return new Vector3(x, y, z).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 2f;
        float y = 0f;
        float z = (float)(1f / (t * t));
        return new Vector3(x, y, z).normalized;
    }
}
