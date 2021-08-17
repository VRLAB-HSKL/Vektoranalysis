using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogHelixCurveCalc : AbstractCurveCalc
{
    public float A = 2f;
    public float B = 0.08f;
    public float Height = 1f;
        
    public LogHelixCurveCalc()
    {
        Name = "LogHelix";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(0f, 8f * Mathf.PI, NumOfSamples));
        ArcLengthParameterIntervall = CalculateArcLengthParamRange(); //new List<float>(new float[NumOfSamples]);
        Is3DCurve = true;
    }    

    protected override Vector3 CalculatePoint(float t)
    {
        float r = A * Mathf.Exp(B * t);
        float x = r * Mathf.Cos(t);
        float y = r * Mathf.Sin(t);
        float z = Height * t;
        return new Vector3(x, y, z);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float r = A * Mathf.Exp(B * t);
        float x = -r * Mathf.Sin(t);
        float y = r * Mathf.Cos(t);
        float z = Height;
        return new Vector3(x, y, z).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float r = A * Mathf.Exp(B * t);
        float x = -r * Mathf.Cos(t);
        float y = -r * Mathf.Sin(t);
        float z = 1f;
        return new Vector3(x, y, z).normalized;
    }

    public float CalculateArcLength()
    {
        float dist = 0f;
        float sqab = Mathf.Sqrt(A * A + B * B);
        dist = sqab * 8f * Mathf.PI;
        //for (int i = 0; i < NumOfSamples; i++)
        //{
        //    float t = ParameterIntervall[i];
        //    dist += sqab * t;
        //}
        return dist;
    }

    public override List<float> CalculateArcLengthParamRange()
    {
        return new List<float>(
            linspace(0f,
            CalculateArcLength(),//CalculateRawDistance(CalculatePoints()),
            NumOfSamples));



        //List<float> retList = new List<float>();
        //float distance = Mathf.Sqrt()

        //float t = 8f * Mathf.PI;
        //float F_upperBound =
        //    (
        //    (B * (3f * Mathf.Pow(B, 2f) + 1f) * Mathf.Sin(2f * t) + (4f * Mathf.Pow(B, 2f) + 2f)
        //     * Mathf.Cos(2f * t))
        //     * Mathf.Sqrt(
        //         Mathf.Pow(A, 6f) * Height * Height
        //         * Mathf.Exp(6f * B * t) * Mathf.Pow((Mathf.Sin(t) - B * Mathf.Cos(t)), 2f)
        //         * Mathf.Pow((B * Mathf.Sin(t) + Mathf.Cos(t)), 2f)))
        //        /
        //    (2f * (3f * B - 2f * i) * (3f * B + 2f * i) * (B * Mathf.Cos(t) - Mathf.Sin(t)) * (B * Mathf.Sin(t) + Mathf.Cos(t)));



    }

    public override List<Vector3> CalculateArcLengthParameterizedPoints()
    {
        List<Vector3> retList = new List<Vector3>();
        float sqab = Mathf.Sqrt(A * A + B * B);
        float dist = CalculateArcLength();
        for (int i = 0; i < NumOfSamples; i++)
        {
            float t = ArcLengthParameterIntervall[i];
            float s = t / sqab;
            Vector3 p = CalculatePoint(s);
            retList.Add(p);
        }
        return retList;
    }

}
