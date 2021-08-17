using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param18CurveCalc : AbstractCurveCalc
{
    public Param18CurveCalc()
    {
        Name = "Param18";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(1f, Mathf.Epsilon, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = true;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float x = t * t;
        float y = 2f * t;
        float z = Mathf.Log(t);
        return new Vector3(x, y, z);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = 2f * t;
        float y = 2f;
        float z = (float)(1f / t);
        return new Vector3(x, y, z).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 2f;
        float y = 0f;
        float z = (float)(1f / (t * t));
        return new Vector3(x, y, z).normalized;
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
