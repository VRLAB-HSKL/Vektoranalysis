using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemniskateGeronoCurveCalc : AbstractCurveCalc
{
    public LemniskateGeronoCurveCalc()
    {
        Name = "LemniskateGerono";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(-Mathf.PI, Mathf.PI, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;
    }


    protected override Vector3 CalculatePoint(float t)
    {
        float t2 = t * t;
        float x = (t2 - 1f) / (t2 + 1f);
        float y = (2f * t * (t2 - 1f)) / Mathf.Pow((t2 + 1f), 2f);

        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = (4f * t) / Mathf.Pow((t * t + 1f), 2);
        float y = (2f * (Mathf.Pow(t, 4) - 6f * (t * t) + 1f ) ) / Mathf.Pow((t * t + 1f), 3);

        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = (4f - 12f * t * t) / Mathf.Pow((t * t + 1f), 3);
        float y = (4f * (Mathf.Pow(t, 4) - 14f * (t * t) + 9f)) / Mathf.Pow((t * t + 1f), 4);

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