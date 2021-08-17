using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param4bCurveCalc : AbstractCurveCalc
{
    public Param4bCurveCalc()
    {
        Name = "Param4b";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(-1.5f, 1.5f, NumOfSamples));
        ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
        Is3DCurve = false;
    }        

    protected override Vector3 CalculatePoint(float t)
    {
        float t3 = t * t * t;
        float x = t3 * t - 1f;
        float y = t3 + 1f;
        return new Vector3(x, y, 0f);        
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float t2 = t * t;
        float x = 4f * (t2 * t);
        float y = 3f * t2;
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = 12f * (t * t);
        float y = 6f * t;
        return new Vector3(x, y, 0f).normalized;
    }

    //protected float CalculateArcLength()
    //{
    //    float a = ParameterIntervall[0];
    //    float b = ParameterIntervall[ParameterIntervall.Count - 1];


    //    //float t2 = t * t;
    //    //float x = 4f * (t2 * t);
    //    //float y = 3f * t2;

    //    float a2 = a * a;
    //    float b2 = b * b;
    //    //float ax = a2 + Mathf.Sqrt(16f * a2 + 9f);
    //    //float bx = b2 + Mathf.Sqrt(16f * b2 + 9f);
    //    //return bx - ax;

    //    float a_integral = Mathf.Sqrt((4f * (a2 * a)) * (4f * (a2 * a)) + (3f * a2) * (3f * a2));
    //    float b_integral = Mathf.Sqrt((4f * (b2 * b)) * (4f * (b2 * b)) + (3f * b2) * (3f * b2));


    //    return -1f;
    //}

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
