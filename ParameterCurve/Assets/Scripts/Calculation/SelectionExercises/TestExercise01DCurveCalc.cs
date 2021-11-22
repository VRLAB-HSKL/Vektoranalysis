using System.Collections.Generic;
using UnityEngine;

namespace Calculation.SelectionExercises
{
    /// <summary>
    /// Calculation class for the fourth curve of the test selection exercise    
    /// </summary>
    public class TestExercise01DCurveClass : AbstractCurveCalc
    {
        public TestExercise01DCurveClass()
        {
            Name = "TestExercise01D";
            NumOfSamples = 2000;
            ParameterRange = new List<float>(Linspace(-6f, 6f, NumOfSamples));

            PointCalcFunc = CalculatePoint;
            VelocityCalcFunc = CalculateVelocityPoint;
            AccelerationCalcFunc = CalculateAccelerationPoint;
        }

        protected override Vector3 CalculatePoint(float t)
        {
            float x = t + Mathf.Sin(2f*t);
            float y = t + Mathf.Sin(3f*t);
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = 2f * Mathf.Cos(2f * t) + 1f;//3f * Mathf.Cos(3f * t);
            float y = 3f * Mathf.Cos(3f * t) + 1f; //4f * Mathf.Cos(4f * t);
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = -4f * Mathf.Sin(2f * t); //-9f * Mathf.Sin(3f * t); 
            float y = 9f * Mathf.Sin(3f * t); //-16f * Mathf.Sin(4f * t);
            return new Vector3(x, y, 0f).normalized;
        }
    }
}