using System.Collections.Generic;
using UnityEngine;

namespace Calculation.SelectionExercises
{
    public class TestExercise01BCurveClass : AbstractCurveCalc
    {
        public TestExercise01BCurveClass()
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
            float t2 = t * t;
            float x = t2 * t2 - t2;
            float y = t + Mathf.Log(t);
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = 4f * t * t * t - 2f * t;
            float y = 1f / t + 1f;
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = 12f * t * t - 2f;
            float y = 1f / (t * t);
            return new Vector3(x, y, 0f).normalized;
        }
    }
}