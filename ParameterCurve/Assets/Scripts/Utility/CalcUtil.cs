using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Static class containing utility calculation functions used in the application
    /// </summary>
    public static class CalcUtil
    {
        /// <summary>
        /// Calculates the length of a given polyline
        /// </summary>
        /// <param name="pointList">Polyline as a collection of point vectors</param>
        /// <returns>Calculated distance</returns>
        public static float CalculateRawDistance(List<Vector3> pointList)
        {
            if (pointList.Count < 2) return 0f;

            var distance = 0f;
            for(var i = 1; i < pointList.Count; i++)
            {
                distance += Mathf.Abs(Vector3.Distance(pointList[i - 1], pointList[i]));
            }

            return distance;
        }

        public static Vector3 CalculatePerpendicularVector(Vector3 vec)
        {
            // choose either the unit Up or Forward axis,
            // depending on which one has the smaller dot() with A.
            // ie, which one is more perpendicular to A.
            // one of them is guaranteed to not be parallel (or anti-parallel) with A.
            // any two vectors known to be perpendicular to each other will work fine here.
            float du = Vector3.Dot(vec, Vector3.up);
            float df = Vector3.Dot(vec, Vector3.forward);
            Vector3 v1 = Mathf.Abs(du) < Mathf.Abs(df) ? Vector3.up : Vector3.forward;
 
            // cross v1 with A. the new vector is perpendicular to both v1 and A.
            Vector3 v2 = Vector3.Cross(v1, vec);
 
            // rotate v2 around A by a random amount
            float degrees = Random.Range(0.0f, 360.0f);
            var rot = Quaternion.AngleAxis(degrees, vec.normalized);
            v2 = rot * v2;

            return v2;
        }
        
        /// <summary>
        /// Maps a value form one range to another range.
        ///
        /// Source: https://rosettacode.org/wiki/Map_range#C.23
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inMin"></param>
        /// <param name="inMax"></param>
        /// <param name="outMin"></param>
        /// <param name="outMax"></param>
        /// <returns></returns>
        public static float MapRange(float value, float inMin, float inMax, float outMin, float outMax)
        {
            // b1 + (s - a1) * (b2 - b1) / (a2 - a1);
            return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
        }
    }
}