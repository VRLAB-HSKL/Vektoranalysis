using System.Collections.Generic;
using UnityEngine;

namespace Calculation.SelectionExercises
{
    /// <summary>
    /// Calculation class for the first curve of the test selection exercise    
    /// </summary>
    public class TestExercise01ACurveClass : AbstractCurveCalc
    {
        public TestExercise01ACurveClass()
        {
            Name = "TestExercise01A";
            NumOfSamples = 2000;
            ParameterRange = new List<float>(Linspace(-2f, 2f, NumOfSamples));

            PointCalcFunc = CalculatePoint;
            VelocityCalcFunc = CalculateVelocityPoint;
            AccelerationCalcFunc = CalculateAccelerationPoint;
        }

        protected override Vector3 CalculatePoint(float t)
        {
            float t2 = t * t;
            float x = t2 * t - 2f * t;
            float y = t2 - t;
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = 3f * t * t - 2f;
            float y = 2f * t - 1f;
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = 6f * t;
            float y = 2f;
            return new Vector3(x, y, 0f).normalized;
        }
    }
}