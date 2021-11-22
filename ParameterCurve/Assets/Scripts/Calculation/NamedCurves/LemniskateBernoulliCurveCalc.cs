using System.Collections.Generic;
using UnityEngine;

namespace Calculation.NamedCurves
{
    /// <summary>
    /// Calculation class for a lemniscate  of Bernoulli curve - https://en.wikipedia.org/wiki/Lemniscate
    /// </summary>
    public class LemniskateBernoulliCurveCalc : AbstractCurveCalc
    {
        public float Factor = 1f;

        private static float Sqrt2 = Mathf.Sqrt(2f);

        public LemniskateBernoulliCurveCalc()
        {
            Name = "LemniskateBernoulli";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(-Mathf.PI, Mathf.PI, NumOfSamples));
        }

        protected override Vector3 CalculatePoint(float t)
        {
            float sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;        
     
            float x = (Factor * Sqrt2 * Mathf.Cos(t)) / (sin2 + 1f);
            float y = (Factor * Sqrt2 * Mathf.Cos(t) * Mathf.Sin(t)) / (sin2 + 1f);

            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;
            float cos2 = (1f + Mathf.Cos(2f * t)) * 0.5f;

            float x = -(Sqrt2 * Factor * Mathf.Sin(t) * (sin2 + 2f * cos2 + 1f)) / Mathf.Pow((sin2 + 1f), 2);
            float y = -(Sqrt2 * Factor * ((sin2 * sin2) + sin2 + (sin2 - 1f) * cos2)) / Mathf.Pow((sin2 + 1f), 2);

            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            //float sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;

            float x = (Sqrt2 * Factor * Mathf.Cos(t) * (44f * Mathf.Cos(2f * t) + Mathf.Cos(4f * t) - 21f )) / (Mathf.Pow(Mathf.Cos(2f * t) - 3, 3));
            float y = (8f * Sqrt2 * Factor * Mathf.Sin(t) * Mathf.Cos(t) * (3f * Mathf.Cos(2f * t) + 7f) ) / (Mathf.Pow(Mathf.Cos(2f * t) - 3, 3));


            return new Vector3(x, y, 0f).normalized;
        }

    }
}
