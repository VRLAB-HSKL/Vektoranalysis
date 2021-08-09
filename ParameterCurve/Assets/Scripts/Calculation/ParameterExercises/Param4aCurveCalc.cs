using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param4aCurveCalc : AbstractCurveCalc
{
    public Param4aCurveCalc() : base()
    {
        Name = "Param4a";
        NumOfSamples = 2000;
        ParameterIntervall = new List<float>(linspace(-2.5f, 2.5f, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;

        PointCalcFunc = CalculatePoint;
        VelocityCalcFunc = CalculateVelocityPoint;
        AccelerationCalcFunc = CalculateAccelerationPoint;
    }    

    protected override Vector3 CalculatePoint(float t)
    {
        float t2 = t * t;
        float x = 3f * (t2 - 3f);
        float y = t2 * t - 3f * t;
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = 6f * t;
        float y = 3f * (t * t) - 3f;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 6f;
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