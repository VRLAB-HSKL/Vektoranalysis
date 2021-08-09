using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycloidCurveCalc : AbstractCurveCalc
{
    public float Radius = 1f;

    public CycloidCurveCalc()
    {
        Name = "Cycloid";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(-2f * Mathf.PI, 2f * Mathf.PI, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = Radius * (t - Mathf.Sin(t));
        float y = Radius * (1f - Mathf.Cos(t));
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = Radius - Radius * Mathf.Cos(t);
        float y = Radius * Mathf.Sin(t);
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = Radius * Mathf.Sin(t);
        float y = Radius * Mathf.Cos(t);
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
