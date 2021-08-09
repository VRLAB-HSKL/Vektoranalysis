using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param59CurveCalc : AbstractCurveCalc
{
    public Param59CurveCalc()
    {
        Name = "Param59";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(-2f, 2f, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = t * Mathf.Exp(t);
        float y = t * Mathf.Exp(-t);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = Mathf.Exp(t) * (t + 1f);
        float y = -Mathf.Exp(-t) * (t - 1f);
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = Mathf.Exp(t) * (t + 2f);
        float y = Mathf.Exp(-t) * (t - 2f);
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
