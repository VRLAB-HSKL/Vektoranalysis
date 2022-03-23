using System.Collections.Generic;
using UnityEngine;

namespace Calculation.SelectionExercises
{
    /// <summary>
    /// Calculation class for the third curve of the test selection exercise    
    /// </summary>
    public class TestExercise01CCurveClass : AbstractCurveCalc
    {
        public TestExercise01CCurveClass()
        {
            Name = "TestExercise01B";
            NumOfSamples = 2000;
            ParameterRange = new List<float>(Linspace(-3f, 3f, NumOfSamples));

            PointCalcFunc = CalculatePoint;
            VelocityCalcFunc = CalculateVelocityPoint;
            AccelerationCalcFunc = CalculateAccelerationPoint;
        }

        protected override Vector3 CalculatePoint(float t)
        {
            float x = Mathf.Sin(3f*t);
            float y = Mathf.Sin(4f*t);
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = 3f * Mathf.Cos(3f * t);
            float y = 4f * Mathf.Cos(4f * t);
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = -9f * Mathf.Sin(3f * t);
            float y = -16f * Mathf.Sin(4f * t);
            return new Vector3(x, y, 0f).normalized;
        }
    }
}