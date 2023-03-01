using System;
using UnityEngine;

namespace ParamCurve.Scripts.Utility
{
    /// <summary>
    /// Static class containing polar coordinates based utility calculation functions used in the application
    /// </summary>
    public static class PolarUtil
    {
        #region Public functions
        
        /// <summary>
        /// Maps polar coordinates to cartesian coordinates
        /// </summary>
        /// <param name="r">radius</param>
        /// <param name="phi">angle</param>
        /// <returns>Cartesian coordinates</returns>
        public static Tuple<float, float> Polar2Cartesian(float r, float phi)
        {
            var x = r * Mathf.Cos(phi);
            var y = r * Mathf.Sin(phi);
            return new Tuple<float, float>(x, y);
        }

        /// <summary>
        /// Maps polar coordinates to cartesian coordinates and computes the first derivative 
        /// </summary>
        /// <param name="r">radius</param>
        /// <param name="phi">angle</param>
        /// <returns>First derivative of cartesian coordinates</returns>
        public static Tuple<float, float> Polar2CartesianFirstDerivative(float r, float phi)
        {
            var x = r * Mathf.Cos(phi) - r * Mathf.Sin(phi);
            var y = r * Mathf.Sin(phi) + r * Mathf.Cos(phi);

            return new Tuple<float, float>(x, y);
        }

        /// <summary>
        /// Helper function that corrects negative radius values
        /// </summary>
        /// <param name="r">radius</param>
        /// <param name="phi">angle</param>
        /// <returns>Corrected polar coordinates</returns>
        public static Tuple<float, float> PolarHelper(float r, float phi)
        {
            if (r >= 0f) return new Tuple<float, float>(r, phi);
           
            r = -r;
            phi += Mathf.PI;

            return new Tuple<float, float>(r, phi);
        }

        #endregion Public functions
    }
}
