using System.Collections.Generic;
using UnityEngine;

namespace Calculation.SelectionExercises
{
    public class TestExercise01ECurveCalc : AbstractCurveCalc
    {
        public TestExercise01ECurveCalc()
        {
            Name = "TestExercise01E";
            NumOfSamples = 2000;
            ParameterRange = new List<float>(Linspace(-2f, 2f, NumOfSamples));
   
            PointCalcFunc = CalculatePoint;
            VelocityCalcFunc = CalculateVelocityPoint;
            AccelerationCalcFunc = CalculateAccelerationPoint;
        }

        protected override Vector3 CalculatePoint(float t)
        {
            float x = Mathf.Sin(t + Mathf.Sin(t));
            float y = Mathf.Cos(t + Mathf.Cos(t));
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = (Mathf.Cos(t) + 1f) * Mathf.Cos(t + Mathf.Sin(t));
            float y = (Mathf.Sin(t) - 1f) * Mathf.Sin(t + Mathf.Cos(t));
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = -Mathf.Sin(t + Mathf.Sin(t)) * Mathf.Pow((Mathf.Cos(t) + 1), 2f) -
                      Mathf.Sin(t) * Mathf.Cos(t + Mathf.Sin(t));
            float y = Mathf.Cos(t) * Mathf.Sin(t + Mathf.Cos(t)) - Mathf.Pow((Mathf.Sin(t) - 1f), 2f) *
                Mathf.Cos(t + Mathf.Cos(t));
            return new Vector3(x, y, 0f).normalized;
        }
    }
}