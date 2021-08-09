using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixCurveCalc : AbstractCurveCalc
{
    private float Radius = 2f;
    private float Height = 4f;

    public HelixCurveCalc()
    {
        Name = "Helix";
        NumOfSamples = 200;
        ParameterIntervall = new List<float>(linspace(-1f, 1f, NumOfSamples));

        //float distance = Mathf.Sqrt(Height * Height + 4f * Mathf.PI * Mathf.PI * Radius * Radius);

        ArcLengthParameterIntervall = CalculateArcLengthParamRange(); //new List<float>(new float[NumOfSamples]);

        Is3DCurve = true;

        PointCalcFunc = CalculatePoint;
        VelocityCalcFunc = CalculateVelocityPoint;
        AccelerationCalcFunc = CalculateAccelerationPoint;
    }    

    protected override Vector3 CalculatePoint(float t)
    {
        float x = Radius * Mathf.Cos(2f * Mathf.PI * t);
        float y = Radius * Mathf.Sin(2f * Mathf.PI * t);
        float z = Height * t;
        return new Vector3(x, y, z);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float x = -2f * Mathf.PI * Radius * Mathf.Sin(2f * Mathf.PI * t);
        float y = 2f * Mathf.PI * Radius * Mathf.Cos(2f * Mathf.PI * t);
        float z = Height;
        return new Vector3(x, y, z).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float x = -4 * (Mathf.PI * Mathf.PI) * Radius * Mathf.Cos(2f * Mathf.PI * t);
        float y = -4 * (Mathf.PI * Mathf.PI) * Radius * Mathf.Sin(2f * Mathf.PI * t);
        float z = 1f;
        return new Vector3(x, y, z).normalized;
    }

    

    public float CalculateArcLength()
    {
        float dist = 0f;
        float velMag = Mathf.Sqrt(Height * Height + 4f * Mathf.PI * Mathf.PI * Radius * Radius);
        dist = 1f * velMag - (-1f) * velMag;
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


        //List<float> paramList = new List<float>();
        //float r2 = Radius * Radius;
        //float h2 = Height * Height;
        //float TwoPi = Mathf.PI * 2f;

        //for (int i = 0; i < NumOfSamples; i++)
        //{
        //    float t = ParameterIntervall[i];
        //    float s = t / Mathf.Sqrt(
        //        h2 * t * t + r2 *
        //        Mathf.Sin(TwoPi * t) * Mathf.Sin(TwoPi * t) +
        //        r2 * Mathf.Cos(TwoPi * t) * Mathf.Cos(TwoPi * t)
        //        ); //Mathf.Sqrt(Radius * Radius + 0.25f * Height * Height) * t;
        //    //float s = Mathf.Sqrt(2f) * t; //* Radius;
        //    //float s = t / Mathf.Sqrt(h2 + 4f * Mathf.PI * Mathf.PI * r2);
        //    //float s = t / (2f * Mathf.Sqrt(h2 + 4f * Mathf.PI * Mathf.PI * r2));
        //    paramList.Add(s);
        //}
        //return paramList;
    }

    public override List<Vector3> CalculateArcLengthParameterizedPoints()
    {
        List<Vector3> retList = new List<Vector3>();
        float velMag = Mathf.Sqrt(Height * Height + 4f * Mathf.PI * Mathf.PI * Radius * Radius);
        float dist = CalculateArcLength();
        for (int i = 0; i < NumOfSamples; i++)
        {
            float t = ArcLengthParameterIntervall[i];
            float s = t / velMag; 
            retList.Add(CalculatePoint(s));
        }
        return retList;
    }

}
