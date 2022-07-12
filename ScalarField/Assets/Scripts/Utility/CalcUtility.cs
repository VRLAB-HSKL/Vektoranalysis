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
        // public static Tuple<List<Vector3>, List<Vector3>> CalculateField01(Vector3 scalingVector)
        // {
        //     var x_lower = 0f;//-5f * math.PI;
        //     var x_upper = 1f * math.PI;
        //     
        //     // var x_range = x_upper - x_lower;
        //     // var x_step = x_range / GlobalDataModel.NumberOfSamples;
        //
        //     var x_values = LinSpace(x_lower, x_upper, GlobalDataModel.NumberOfSamples).ToArray();
        //         //CreateRange(x_lower, x_upper, GlobalDataModel.NumberOfSamples);
        //
        //     var y_lower = 0f;//-5f * math.PI;
        //     var y_upper = 2f * math.PI;
        //     // var y_range = y_upper - y_lower;
        //     // var y_step = y_range / GlobalDataModel.NumberOfSamples;
        //
        //     var y_values = LinSpace(y_lower, y_upper, GlobalDataModel.NumberOfSamples).ToArray(); 
        //         //CreateRange(y_lower, y_upper, GlobalDataModel.NumberOfSamples);
        //
        //     var raw_vertices = new List<Vector3>();
        //     var display_vertices = new List<Vector3>();
        //
        //     var zmin = float.MaxValue;
        //     var zmax = float.MinValue;
        //
        //     
        //     
        //     for(int i = 0; i < GlobalDataModel.NumberOfSamples; i++)
        //     {
        //         var x = x_values[i];//i * x_step;
        //         float y;
        //         for (int j = 0; j < GlobalDataModel.NumberOfSamples; j++)
        //         {
        //             y = y_values[j]; //j * y_step;    
        //             var z = -math.sin(x) * math.sin(y);
        //             
        //             if (z < zmin) zmin = z;
        //             if (z > zmax) zmax = z;
        //
        //             var calculatedVector = new Vector3(x, y, z);
        //         
        //             raw_vertices.Add(calculatedVector);
        //             
        //             // Switch axis to create horizontal mesh
        //             var displayVector = new Vector3(x, z, y);
        //
        //             //var displayVector = new Vector3(y, z, x);
        //             
        //             // Scale points to 1/10th
        //             //displayVector *= ScalingVector;
        //             displayVector = Vector3.Scale(displayVector, scalingVector);
        //         
        //             
        //             display_vertices.Add(displayVector);
        //         }
        //
        //     }
        //
        //     //Debug.Log("zmin: " + zmin + ", zmax: " + zmax);
        //     
        //     return new Tuple<List<Vector3>, List<Vector3>>(raw_vertices, display_vertices);
        // }

        //private static float e = 2.71828182845904523536028747135266249775724709369995f;

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
        /// Creates collection of flat values, based on the initialization function for numpy arrays in python
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


        public static int NeareastNeighborIndexXY(List<Vector3> points, Vector3 point)
        {
            if (points.Count == 0) return -1;
            if (points.Count == 1) return 0;

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

        public static List<Vector3> MapDisplayVectors(List<Vector3> dVertices, Bounds bounds)
        {
            var x_min = dVertices.Min(v => v.x);
            var x_max = dVertices.Max(v => v.x);
            var y_min = dVertices.Min(v => v.y);
            var y_max = dVertices.Max(v => v.y);
            var z_min = dVertices.Min(v => v.z);
            var z_max = dVertices.Max(v => v.z);   
            
            var bb = bounds.extents;
            var bbCenter = bounds.center;

            var displayVertices = new List<Vector3>();
            
            // Calculate mesh vertices
            for (var i = 0; i < dVertices.Count; i++)
            {
                var displayVector = dVertices[i];
            
                // Add position offset
                var translatedVector = displayVector + bbCenter;

                // Map point values to bounding box size
                var mappedVec = CalcUtility.MapVectorToRange(
                    translatedVector, new Vector3(x_min, y_min, z_min), new Vector3(x_max, y_max, z_max),
                    -bb, bb
                );
                
                displayVertices.Add(mappedVec);
            }

            return displayVertices;
        }
    }
    
    
    
    
}