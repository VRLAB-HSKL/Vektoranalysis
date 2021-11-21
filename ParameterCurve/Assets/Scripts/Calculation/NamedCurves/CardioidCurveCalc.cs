using System.Collections.Generic;
using UnityEngine;

namespace Calculation.NamedCurves
{
    /// <summary>
    /// Calculation class for a cardioid curve
    ///
    /// https://en.wikipedia.org/wiki/Cardioid
    /// 
    /// </summary>
    public class CardioidCurveCalc : AbstractCurveCalc
    {
        #region Private members
        
        /// <summary>
        /// Factor specifying the radius of the two circles
        /// </summary>
        private const float A = 1f;

        #endregion Private members
        
        #region Constructors
        
        public CardioidCurveCalc()
        {
            Name = "Cardioid";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(-Mathf.PI, Mathf.PI, NumOfSamples));
        }

        #endregion Constructors

        #region Protected functions
        
        /// <summary>
        /// Point vector calculation function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Point vector</returns>
        protected override Vector3 CalculatePoint(float t)
        {
            var x = 2f * A * (1f - Mathf.Cos(t)) * Mathf.Cos(t);
            var y = 2f * A * (1f - Mathf.Cos(t)) * Mathf.Sin(t);
            return new Vector3(x, y, 0f);
        }

        /// <summary>
        /// Velocity vector calculation function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Velocity vector</returns>
        protected override Vector3 CalculateVelocityPoint(float t)
        {        
            var x = 2f * A * Mathf.Sin(t) * (2f * Mathf.Cos(t) - 1f);

            var sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;
            var cos2 = (1f + Mathf.Cos(2f * t)) * 0.5f;
            var y = 2f * A * (sin2 - cos2 + Mathf.Cos(t));
            return new Vector3(x, y, 0f).normalized;
        }

        /// <summary>
        /// Acceleration vector calculation function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Acceleration vector</returns>
        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            var sin2 = (1f - Mathf.Cos(2f * t)) * 0.5f;
        
            var x = 2f * A * Mathf.Cos(t) * (2f * Mathf.Cos(t) - 1f) - 4f * A * sin2;
            var y = -2f * A * Mathf.Sin(t) * (2f * Mathf.Cos(t) + 1);
            return new Vector3(x, y, 0f).normalized;
        }
        
        #endregion Protected functions
    }
}
