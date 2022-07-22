using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Static utility class containing frequently used functions related to numerical calculations
    /// </summary>
    public static class CalcUtility
    {
        private static List<float> CreateRange(float start, float end, int sampleCount)
        {
            if (start > end) return null;

            var range = Math.Abs(end - start);
            var step = range / sampleCount;

            List<float> values = new List<float>();

            for (float i = start; i < end; i += step)
            {
                values.Add(i);
            }

            return values;
        }
        
        /// <summary>
        /// Creates collection of float values, based on the initialization function for numpy arrays in python
        /// Source: https://gist.github.com/wcharczuk/3948606
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<float> Arange(float start, int count)
        {
            return Enumerable.Range((int)start, count).Select(v => (float)v);
        }
        
        /// <summary>
        /// Creates a collection of float values in a linear range with a specific sample count,
        /// based on the initialization function for numpy arrays in python
        /// Source: https://gist.github.com/wcharczuk/3948606
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="num"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static IEnumerable<float> LinSpace(float start, float stop, int num, bool endpoint = true)
        {
            var result = new List<float>();
            if (num <= 0)
            {
                return result;
            }

            if (endpoint)
            {
                if (num == 1) 
                {
                    return new List<float>() { start };
                }

                var step = (stop - start)/ ((float)num - 1.0f);
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }
            else 
            {
                var step = (stop - start) / (float)num;
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }

            return result;
        }
        
        /// <summary>
        /// Maps a value form one range to another range.
        /// Source: https://rosettacode.org/wiki/Map_range#C.23
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inMin"></param>
        /// <param name="inMax"></param>
        /// <param name="outMin"></param>
        /// <param name="outMax"></param>
        /// <returns></returns>
        public static float MapValueToRange(float value, float inMin, float inMax, float outMin, float outMax)
        {
            // b1 + (s - a1) * (b2 - b1) / (a2 - a1);
            return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
        }

        public static Vector3 MapVectorToRange(Vector3 vec, Vector3 inMin, Vector3 inMax, Vector3 outMin,
            Vector3 outMax)
        {
            return new Vector3(
                MapValueToRange(vec[0], inMin[0], inMax[0], outMin[0], outMax[0]),
                MapValueToRange(vec[1], inMin[1], inMax[1], outMin[1], outMax[1]),
                MapValueToRange(vec[2], inMin[2], inMax[2], outMin[2], outMax[2])
            );
        }

        public static Vector3 MapVectorToBBox(Vector3 vec, Vector3 inMin, Vector3 inMax, Bounds bbox)
        {
            return MapVectorToRange(vec, inMin, inMax, -bbox.extents, bbox.extents);
        }
        
        
        
        public static double Cross(Vector3 O, Vector3 A, Vector3 B)
        {
            return (A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x);
        }

        /// <summary>
        /// ToDo: Probably do this in python also
        /// Custom convex hull algorithm, used to create polygon line around the point cloud
        /// of a specific z value in the scalar field
        /// Source: https://stackoverflow.com/questions/14671206/how-to-compute-convex-hull-in-c-sharp
        /// </summary>
        /// <param name="points"></param>
        /// <param name="epsilonTolerance"></param>
        /// <returns></returns>
        public static List<Vector3> GetConvexHull(List<Vector3> points, float epsilonTolerance = 0.001f)
        {
            if (points == null)
                return null;

            if (points.Count <= 1)
                return points;

            // Order points by x coordinate to improve convex hull approximation (own step added by me)
            points = points.OrderBy(p => p.x).ToList();
            
            
            int n = points.Count(), k = 0;
            var H = new List<Vector3>(new Vector3[2 * n]);

            points.Sort((a, b) =>
                Math.Abs(a.x - b.x) < epsilonTolerance ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x));

            // Build lower hull
            for (int i = 0; i < n; ++i)
            {
                while (k >= 2 && Cross(H[k - 2], H[k - 1], points[i]) <= 0)
                    k--;
                H[k++] = points[i];
            }

            // Build upper hull
            for (int i = n - 2, t = k + 1; i >= 0; i--)
            {
                while (k >= t && Cross(H[k - 2], H[k - 1], points[i]) <= 0)
                    k--;
                H[k++] = points[i];
            }

            return H.Take(k - 1).ToList();
        }


        /// <summary>
        /// Simple nearest neighbour algorithm to find the closest point to a given point in a collection of points.
        /// This function only takes the x and y dimension into account (2D points)
        /// </summary>
        /// <param name="points">List of points (possible neighbours)</param>
        /// <param name="point">Source point</param>
        /// <returns>Index of nearest neighbour in point collection</returns>
        public static int NearestNeighborIndexXY(List<Vector3> points, Vector3 point)
        {
            switch (points.Count)
            {
                case 0:
                    return -1;
                case 1:
                    return 0;
            }

            var smallestDist = float.MaxValue;
            var index = -1;
            for (var i = 0; i < points.Count; i++)
            {
                var p = points[i];
                var dist = Mathf.Sqrt(Mathf.Pow(p.x - point.x, 2) + Mathf.Pow(p.y - point.y, 2));
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// Maps a collection of display vectors to an encapsulation bounding box. The values are linearly interpolated
        /// in each dimension between the minimum and maximum value the bounding box has in each of these dimensions
        /// </summary>
        /// <param name="dVertices"></param>
        /// <param name="bounds"></param>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static List<Vector3> MapDisplayVectors(List<Vector3> dVertices, Bounds bounds, Transform tf)
        {
            var x_min = dVertices.Min(v => v.x);
            var x_max = dVertices.Max(v => v.x);
            var y_min = dVertices.Min(v => v.y);
            var y_max = dVertices.Max(v => v.y);
            var z_min = dVertices.Min(v => v.z);
            var z_max = dVertices.Max(v => v.z);   
            
            var bb = bounds.extents;
            var displayVertices = new List<Vector3>();
            
            // Calculate mesh vertices
            for (var i = 0; i < dVertices.Count; i++)
            {
                // Get display vector
                var displayVector = dVertices[i];
            
                // Map point values to bounding box size
                var mappedVec = MapVectorToRange(
                    displayVector, new Vector3(x_min, y_min, z_min), new Vector3(x_max, y_max, z_max),
                    -bb, bb
                );
            
                // Move coordinate system origin to the center of the bounding box
                //var translatedVec = mappedVec + bbCenter;
                
                displayVertices.Add(mappedVec);
            }

            return displayVertices;
        }
    }
    
    
    
    
}