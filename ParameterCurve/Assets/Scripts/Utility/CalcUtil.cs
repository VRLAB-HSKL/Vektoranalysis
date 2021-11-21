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
    }
}