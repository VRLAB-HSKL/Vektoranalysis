using System;
using System.Collections.Generic;
using UnityEngine;

public class TestExercise01ECurveCalc : AbstractCurveCalc
{
    public TestExercise01ECurveCalc()
    {
        Name = "TestExercise01E";
        NumOfSamples = 2000;
        ParameterIntervall = new List<float>(linspace(-2f, 2f, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;

        PointCalcFunc = CalculatePoint;
        VelocityCalcFunc = CalculateVelocityPoint;
        AccelerationCalcFunc = CalculateAccelerationPoint;
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

    protected override Vector3 CalculatePoint(float t)
    {
        float x = Mathf.Sin(t + Mathf.Sin(t));
        float y = Mathf.Cos(t + Mathf.Cos(t));
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = (Mathf.Cos(t) + 1f) * Mathf.Cos(t + Mathf.Sin(t));
        float y = (Mathf.Sin(t) - 1f) * Mathf.Sin(t + Mathf.Cos(t));
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = -Mathf.Sin(t + Mathf.Sin(t)) * Mathf.Pow((Mathf.Cos(t) + 1), 2f) -
                  Mathf.Sin(t) * Mathf.Cos(t + Mathf.Sin(t));
        float y = Mathf.Cos(t) * Mathf.Sin(t + Mathf.Cos(t)) - Mathf.Pow((Mathf.Sin(t) - 1f), 2f) *
                    Mathf.Cos(t + Mathf.Cos(t));
        return new Vector3(x, y, 0f).normalized;
    }
}