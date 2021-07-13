using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param57CurveCalc : AbstractCurveCalc
{
    public static float V0 = 500f;
    public static float ALPHA = 30f;
    public static float PHI = ALPHA * Mathf.Deg2Rad;
    public static float GRAVITY = 9.81f;

    public Param57CurveCalc()
    {
        Name = "param57";

        float rangeEnd = 2f * V0 * Mathf.Sin(ALPHA * Mathf.Deg2Rad) / GRAVITY;
        ParameterIntervall = new List<float>(linspace(0f, rangeEnd, 200));

        Is3DCurve = false;
    }
        

    protected override Vector3 CalculatePoint(float t)
    {
        float x = V0 * Mathf.Cos(PHI) * t;
        float y = V0 * Mathf.Sin(PHI) * t - 0.5f * GRAVITY * (t * t);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = V0 * Mathf.Cos(PHI);
        float y = V0 * Mathf.Sin(PHI) - GRAVITY * t;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 0f;
        float y = -GRAVITY;
        return new Vector3(x, y, 0f).normalized;
    }
}
