using System.Collections.Generic;
using UnityEngine;

namespace Calculation.ParameterExercises
{
    /// <summary>
    /// Calculation class for the first curve associated with the parameter56.tex course material file
    /// </summary>
    public class Param56CurveCalc : AbstractCurveCalc
    {
        public Param56CurveCalc()
        {
            Name = "Param56";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(-4f * Mathf.PI, 4f * Mathf.PI, NumOfSamples));
        }

        protected override Vector3 CalculatePoint(float t)
        {
            float x = Mathf.Cos(t);
            float y = Mathf.Atan(t);
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = -Mathf.Sin(t);
            float y = 1f / (t * t) + 1f;
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = -Mathf.Cos(t);
            float y = -2f / (t * t * t);
            return new Vector3(x, y, 0f).normalized;
        }

    }
}
