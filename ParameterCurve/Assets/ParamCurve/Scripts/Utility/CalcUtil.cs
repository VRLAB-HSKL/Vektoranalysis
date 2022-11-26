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

        public static List<Vector3> CalculateCircleFacingDirection(
            Vector3 origin, Vector3 direction, Vector3 perpendicularVector, int stepCount)
        {
            var degreeStepSize = 360f / stepCount;
            var circlePoints = new List<Vector3>();
            for (var i = 0; i < stepCount; i++)
            {
                var quaternionRot = Quaternion.AngleAxis(i * degreeStepSize, direction);
                var rotatedVector = (quaternionRot * perpendicularVector);
                var circlePoint = origin + rotatedVector;    
                circlePoints.Add(circlePoint);
            }
            //Debug.Log("count: " + circlePoints.Count);
            return circlePoints;
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
        
        
        // Source: raylib.h math utility c header
        // https://github.com/raysan5/raylib/blob/master/src/raymath.h
        public static Vector3 Vector3Perpendicular(Vector3 v)
        {
            Vector3 result = Vector3.zero;

            float min = Mathf.Abs(v.x);
            Vector3 cardinalAxis = new Vector3(1.0f, 0.0f, 0.0f);

            if (Mathf.Abs(v.y) < min)
            {
                min = Mathf.Abs(v.y);
                Vector3 tmp = new Vector3(0.0f, 1.0f, 0.0f);
                cardinalAxis = tmp;
            }

            if (Mathf.Abs(v.z) < min)
            {
                Vector3 tmp = new Vector3(0.0f, 0.0f, 1.0f);
                cardinalAxis = tmp;
            }

            // Cross product between vectors
            result.x = v.y*cardinalAxis.z - v.z*cardinalAxis.y;
            result.y = v.z*cardinalAxis.x - v.x*cardinalAxis.z;
            result.z = v.x*cardinalAxis.y - v.y*cardinalAxis.x;

            return result;
        }
        
    }
}