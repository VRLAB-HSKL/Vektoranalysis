using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixCurveCalc : AbstractCurveCalc
{
    private float Radius = 2f;
    private float Height = 4f;

    public HelixCurveCalc()
    {
        Name = "Helix";
        ParameterIntervall = new List<float>(linspace(-1f, 1f, 200));
        Is3DCurve = true;

        PointCalcFunc = CalculatePoint;
        VelocityCalcFunc = CalculateVelocityPoint;
        AccelerationCalcFunc = CalculateAccelerationPoint;
    }

    

    protected override Vector3 CalculatePoint(float t)
    {
        float x = Radius * Mathf.Cos(2f * Mathf.PI * t);
        float y = Radius * Mathf.Sin(2f * Mathf.PI * t);
        float z = Height * t;
        return new Vector3(x, y, z);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = -2f * Mathf.PI * Radius * Mathf.Sin(2f * Mathf.PI * t);
        float y = 2f * Mathf.PI * Radius * Mathf.Cos(2f * Mathf.PI * t);
        float z = Height;
        return new Vector3(x, y, z).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = -4 * (Mathf.PI * Mathf.PI) * Radius * Mathf.Cos(2f * Mathf.PI * t);
        float y = -4 * (Mathf.PI * Mathf.PI) * Radius * Mathf.Sin(2f * Mathf.PI * t);
        float z = 1f;
        return new Vector3(x, y, z).normalized;
    }

}
