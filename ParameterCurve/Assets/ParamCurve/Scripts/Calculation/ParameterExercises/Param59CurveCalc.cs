using System.Collections.Generic;
using UnityEngine;

namespace Calculation.ParameterExercises
{
    /// <summary>
    /// Calculation class for the first curve associated with the parameter59.tex course material file
    /// </summary>
    public class Param59CurveCalc : AbstractCurveCalc
    {
        public Param59CurveCalc()
        {
            Name = "Param59";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(-2f, 2f, NumOfSamples));
        }

        protected override Vector3 CalculatePoint(float t)
        {
            float x = t * Mathf.Exp(t);
            float y = t * Mathf.Exp(-t);
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = Mathf.Exp(t) * (t + 1f);
            float y = -Mathf.Exp(-t) * (t - 1f);
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = Mathf.Exp(t) * (t + 2f);
            float y = Mathf.Exp(-t) * (t - 2f);
            return new Vector3(x, y, 0f).normalized;
        }


    }
}
