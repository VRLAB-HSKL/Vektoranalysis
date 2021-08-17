using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardioidCurveCalc : AbstractCurveCalc
{
    public float A = 1f;

    public CardioidCurveCalc()
    {
        Name = "Cardioid";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(-Mathf.PI, Mathf.PI, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = 2f * A * (1f - Mathf.Cos(t)) * Mathf.Cos(t);
        float y = 2f * A * (1f - Mathf.Cos(t)) * Mathf.Sin(t);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {        
        float x = 2f * A * Mathf.Sin(t) * (2f * Mathf.Cos(t) - 1f);

        float sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;
        float cos2 = (1f + Mathf.Cos(2f * t)) * 0.5f;
        float y = 2f * A * (sin2 - cos2 + Mathf.Cos(t));
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;
        
        float x = 2f * A * Mathf.Cos(t) * (2f * Mathf.Cos(t) - 1f) - 4f * A * sin2;
        float y = -2f * A * Mathf.Sin(t) * (2f * Mathf.Cos(t) + 1);
        return new Vector3(x, y, 0f).normalized;
    }

    public override List<float> CalculateArcLengthParamRange()
    {
        return new List<float>(
            linspace(0f,
            CalculateRawDistance(CalculatePoints()),
            ParameterIntervall.Count));
    }

    public override List<Vector3> CalculateArcLengthParameterizedPoints()
    {
        List<Vector3> retList = new List<Vector3>();
        for (int i = 0; i < NumOfSamples; i++)
        {
            retList.Add(Vector3.up);
        }
        return retList;
    }
}
