using System.Collections.Generic;
using UnityEngine;

namespace Calculation.SelectionExercises
{
    /// <summary>
    /// Calculation class for the sixth curve of the test selection exercise    
    /// </summary>
    public class TestExercise01FCurveCalc : AbstractCurveCalc
    {
            public TestExercise01FCurveCalc()
            {
                Name = "TestExercise01F";
                NumOfSamples = 2000;
                ParameterRange = new List<float>(Linspace(-4f, 3f, NumOfSamples));

                PointCalcFunc = CalculatePoint;
                VelocityCalcFunc = CalculateVelocityPoint;
                AccelerationCalcFunc = CalculateAccelerationPoint;
            }
        
            protected override Vector3 CalculatePoint(float t)
            {
                var x = Mathf.Cos(t);
                var y = Mathf.Sin(t + Mathf.Sin(5f * t));
                return new Vector3(x, y, 0f);
            }
        
            protected override Vector3 CalculateVelocityPoint(float t)
            {
                var x = -Mathf.Sin(t);
                var y = (5f * Mathf.Cos(5f * t) + 1f) * Mathf.Cos(t + Mathf.Sin(5f * t));
                return new Vector3(x, y, 0f).normalized;
            }
        
            protected override Vector3 CalculateAccelerationPoint(float t)
            {
                var x = -Mathf.Cos(t);
                var y = -Mathf.Sin(t + Mathf.Sin(5f * t)) * Mathf.Pow((5f * Mathf.Cos(5f * t) + 1f), 2f)
                        - 25f * Mathf.Sin(5f * t) * Mathf.Cos(t + Mathf.Sin(5f * t));
                return new Vector3(x, y, 0f).normalized;
            }
    }
}