using System.Collections.Generic;
using UnityEngine;

namespace Calculation.NamedCurves
{
    /// <summary>
    /// Calculation class for a cycloid curve
    /// 
    /// https://en.wikipedia.org/wiki/Cycloid
    /// </summary>
    public class CycloidCurveCalc : AbstractCurveCalc
    {
        #region Private members
        
        /// <summary>
        /// Factor specifying the radius of the rolling circle
        /// </summary>
        private const float Radius = 1f;

        #endregion Private members
        
        #region Constructors
        
        public CycloidCurveCalc()
        {
            Name = "Cycloid";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(-2f * Mathf.PI, 2f * Mathf.PI, NumOfSamples));
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
            var x = Radius * (t - Mathf.Sin(t));
            var y = Radius * (1f - Mathf.Cos(t));
            return new Vector3(x, y, 0f);
        }

        /// <summary>
        /// Velocity vector calculation function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Velocity vector</returns>
        protected override Vector3 CalculateVelocityPoint(float t)
        {
            var x = Radius - Radius * Mathf.Cos(t);
            var y = Radius * Mathf.Sin(t);
            return new Vector3(x, y, 0f).normalized;
        }

        /// <summary>
        /// Acceleration vector calculation function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Acceleration vector</returns>
        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            var x = Radius * Mathf.Sin(t);
            var y = Radius * Mathf.Cos(t);
            return new Vector3(x, y, 0f).normalized;
        }
    
        #endregion Protected functions
    }
}
