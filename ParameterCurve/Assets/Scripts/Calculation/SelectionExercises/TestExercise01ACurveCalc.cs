
using System.Collections.Generic;
using UnityEngine;

public class TestExercise01ACurveClass : AbstractCurveCalc
{
    public TestExercise01ACurveClass()
    {
        Name = "TestExercise01A";
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
        float t2 = t * t;
        float x = t2 * t - 2f * t;
        float y = t2 - t;
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = 3f * t * t - 2f;
        float y = 2f * t - 1f;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 6f * t;
        float y = 2f;
        return new Vector3(x, y, 0f).normalized;
    }
}