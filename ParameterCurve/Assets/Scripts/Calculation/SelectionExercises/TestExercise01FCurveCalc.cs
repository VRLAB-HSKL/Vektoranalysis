using System.Collections.Generic;
using UnityEngine;

namespace Calculation.SelectionExercises
{
    public class TestExercise01FCurveCalc : AbstractCurveCalc
    {
            public TestExercise01FCurveCalc()
            {
                Name = "TestExercise01F";
                NumOfSamples = 2000;
                ParameterIntervall = new List<float>(linspace(-4f, 3f, NumOfSamples));
                ArcLengthParameterIntervall = new List<float>(new float[NumOfSamples]);
                Is3DCurve = false;
        
                PointCalcFunc = CalculatePoint;
                VelocityCalcFunc = CalculateVelocityPoint;
                AccelerationCalcFunc = CalculateAccelerationPoint;
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
        
            protected override Vector3 CalculatePoint(float t)
            {
                float x = Mathf.Cos(t);
                float y = Mathf.Sin(t + Mathf.Sin(5f * t));
                return new Vector3(x, y, 0f);
            }
        
            protected override Vector3 CalculateVelocityPoint(float t)
            {
                float x = -Mathf.Sin(t);
                float y = (5f * Mathf.Cos(5f * t) + 1f) * Mathf.Cos(t + Mathf.Sin(5f * t));
                return new Vector3(x, y, 0f).normalized;
            }
        
            protected override Vector3 CalculateAccelerationPoint(float t)
            {
                float x = -Mathf.Cos(t);
                float y = -Mathf.Sin(t + Mathf.Sin(5f * t)) * Mathf.Pow((5f * Mathf.Cos(5f * t) + 1f), 2f)
                          - 25f * Mathf.Sin(5f * t) * Mathf.Cos(t + Mathf.Sin(5f * t));
                return new Vector3(x, y, 0f).normalized;
            }
    }
}