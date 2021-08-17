using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param60CurveCalc : AbstractCurveCalc
{
    public Param60CurveCalc()
    {
        Name = "Param60";
        NumOfSamples = 100;
        ParameterIntervall = new List<float>(linspace(-1.5f, 2.75f, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;
    }


    protected override Vector3 CalculatePoint(float t)
    {
        float t2 = t * t;
        float x = (t2 - 2f * t - 2f) * t2;
        float y = t * (t2 - 1f);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float t2 = t * t;
        float x = 2f * t * (2f * t2 - 3f * t - 2f);
        float y = 3f * t2 - 1f;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float t2 = t * t;
        float x = 4f * (3f * t2 - 3f * t - 1f);
        float y = 6f * t;
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
