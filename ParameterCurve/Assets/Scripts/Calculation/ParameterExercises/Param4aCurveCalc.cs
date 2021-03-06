using System.Collections.Generic;
using UnityEngine;

namespace Calculation.ParameterExercises
{
    /// <summary>
    /// Calculation class for the first curve associated with the parameter4.tex course material file
    /// </summary>
    public class Param4aCurveCalc : AbstractCurveCalc
    {
        public Param4aCurveCalc() : base()
        {
            Name = "Param4a";
            NumOfSamples = 2000;
            ParameterRange = new List<float>(Linspace(-2.5f, 2.5f, NumOfSamples));

            PointCalcFunc = CalculatePoint;
            VelocityCalcFunc = CalculateVelocityPoint;
            AccelerationCalcFunc = CalculateAccelerationPoint;
        }    

        protected override Vector3 CalculatePoint(float t)
        {
            float t2 = t * t;
            float x = 3f * (t2 - 3f);
            float y = t2 * t - 3f * t;
            return new Vector3(x, y, 0f);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = 6f * t;
            float y = 3f * (t * t) - 3f;
            return new Vector3(x, y, 0f).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = 6f;
            float y = 6f * t;
            return new Vector3(x, y, 0f).normalized;
        }
    }
}