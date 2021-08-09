using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param61CurveCalc : AbstractCurveCalc
{
    public Param61CurveCalc()
    {
        Name = "Param61";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(-4f, 4f, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float t2 = t * t;
        float x = (t2 + 4f * t - 8f) * t2;
        float y = (2f * t - 1f) * t;
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float t2 = t * t;
        float x = 4f * t * (t2 + 3f * t - 4f);
        float y = 4f * t - 1f;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float t2 = t * t;
        float x = 4f * (3f * t2 + 6f * t - 4f);
        float y = 4f;
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
