using System.Collections.Generic;
using UnityEngine;

namespace Calculation.NamedCurves
{
    /// <summary>
    /// Calculation class for an archimedean spiral
    ///
    /// https://en.wikipedia.org/wiki/Archimedean_spiral
    /// 
    /// </summary>
    public class ArchimedeanSpiralCurveCalc : AbstractCurveCalc
    {
        #region Public members
        
        /// <summary>
        /// Display string in GUI
        /// </summary>
        public new string DisplayString => "Archimedean-" + System.Environment.NewLine + "Spiral";
        
        #endregion Public members
        
        #region Private members
        
        /// <summary>
        /// Factor specifying the starting point of the spiral
        /// </summary>
        private const float A = 0.125f;
        
        /// <summary>
        /// Factor specifying the distance between loops
        /// </summary>
        private const float B = 0.25f;

        #endregion Private members
        
        #region Constructors
        
        public ArchimedeanSpiralCurveCalc()
        {
            Name = "ArchimedeanSpiral";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(0f, 6f * Mathf.PI, NumOfSamples));
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
            var rI = A + B * t;
            var x = rI * Mathf.Cos(t);
            var y = rI * Mathf.Sin(t);
            return new Vector3(x, y, 0f);
        }

        /// <summary>
        /// Velocity vector calculation function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Velocity vector</returns>
        protected override Vector3 CalculateVelocityPoint(float t)
        {
            var rI = A + B * t;
            var x = -rI * Mathf.Sin(t);
            var y = rI * Mathf.Cos(t);
            return new Vector3(x, y, 0f).normalized;
        }

        /// <summary>
        /// Acceleration vector calculation function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Acceleration vector</returns>
        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            var rI = A + B * t;
            var x = -rI * Mathf.Cos(t);
            var y = -rI * Mathf.Sin(t);
            return new Vector3(x, y, 0f).normalized;
        }

        #endregion Protected functions
    }
}
    

