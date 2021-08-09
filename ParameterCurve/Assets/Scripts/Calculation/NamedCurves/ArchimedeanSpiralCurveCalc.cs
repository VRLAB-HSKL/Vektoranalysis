using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchimedeanSpiralCurveCalc : AbstractCurveCalc
{
    public static float A = 0.125f;
    public static float B = 0.25f;

    public new string DisplayString
    {
        get
        {
            return "Archimedean-" + System.Environment.NewLine + "Spiral";
        }
    }


    public ArchimedeanSpiralCurveCalc()
    {
        Name = "ArchimedeanSpiral";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(0f, 6f * Mathf.PI, NumOfSamples));
        ArcLengthParameterIntervall = CalculateArcLengthParamRange();
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float r_i = A + B * t;
        float x = r_i * Mathf.Cos(t);
        float y = r_i * Mathf.Sin(t);
        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float r_i = A + B * t;
        float x = -r_i * Mathf.Sin(t);
        float y = r_i * Mathf.Cos(t);
        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float r_i = A + B * t;
        float x = -r_i * Mathf.Cos(t);
        float y = -r_i * Mathf.Sin(t);
        return new Vector3(x, y, 0f).normalized;
    }

    public float CalculateArcLength()
    {
        
        //float velMag = Mathf.Sqrt(Height * Height + 4f * Mathf.PI * Mathf.PI * Radius * Radius);
        float t = 6f;
        float r_i = A + B * t;       
        float dist = (Mathf.Sqrt( Mathf.Pow((-r_i * Mathf.Sin(t)), 2f) + Mathf.Pow((r_i * Mathf.Cos(t)), 2f)) ) * t;
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
    }

    public override List<Vector3> CalculateArcLengthParameterizedPoints()
    {
        List<Vector3> retList = new List<Vector3>();
        //float dist = CalculateArcLength();
        for (int i = 0; i < NumOfSamples; i++)
        {
            float t = ArcLengthParameterIntervall[i];
            float r_i = A + B * t;//6f;
            float s = t / (Mathf.Sqrt(Mathf.Pow((-r_i * Mathf.Sin(t)), 2f) + Mathf.Pow((r_i * Mathf.Cos(t)), 2f)));
            retList.Add(CalculatePoint(s));
        }
        return retList;
    }
}
    

