using System;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Static class containing polar coordinates based utility calculation functions used in the application
    /// </summary>
    public static class PolarUtil
    {
        public static Tuple<float, float> Polar2Cartesian(float r, float phi)
        {
            var x = r * Mathf.Cos(phi);
            var y = r * Mathf.Sin(phi);

            return new Tuple<float, float>(x, y);
        }

        public static Tuple<float, float> Polar2CartesianFirstDerivative(float r, float phi)
        {
            var x = r * Mathf.Cos(phi) - r * Mathf.Sin(phi);
            var y = r * Mathf.Sin(phi) + r * Mathf.Cos(phi);

            return new Tuple<float, float>(x, y);
        }


        public static Tuple<float, float> PolarHelper(float r, float phi)
        {        
            if (r < 0f)
            {
                r = -r;
                phi += Mathf.PI;
            }

            return new Tuple<float, float>(r, phi);
        }


        // public static Tuple<float[], float[]> PolarHelper(float[] r, float[] phi)
        // {
        //     for(int i = 0; i < r.Length; i++)
        //     {
        //         if(r[i] < 0f)
        //         {
        //             r[i] = -r[i];
        //             phi[i] = phi[i] + Mathf.PI;
        //         }
        //     }
        //
        //     return new Tuple<float[], float[]>(r, phi);
        // }
    }
}
