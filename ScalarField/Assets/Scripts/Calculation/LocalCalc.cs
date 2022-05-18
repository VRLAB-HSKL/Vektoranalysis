using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Calculation
{
    public class ScalarField
    {
        public Tuple<float, float> parameterRangeX;
        public Tuple<float, float> parameterRangeY;
        public int numberOfSamples = GlobalDataModel.NumberOfSamples;

        //public delegate float CalculatePoint(float x, float y);

        public Func<float, float, float> CalculatePoint;

    }
    
    public static class LocalCalc
    {
        public static Tuple<List<Vector3>, List<Vector3>> CalculateField01(Vector3 scalingVector)
        {
            var x_lower = 0f;//-5f * math.PI;
            var x_upper = 1f * math.PI;
            
            // var x_range = x_upper - x_lower;
            // var x_step = x_range / GlobalDataModel.NumberOfSamples;

            var x_values = LinSpace(x_lower, x_upper, GlobalDataModel.NumberOfSamples).ToArray();
                //CreateRange(x_lower, x_upper, GlobalDataModel.NumberOfSamples);

            var y_lower = 0f;//-5f * math.PI;
            var y_upper = 2f * math.PI;
            // var y_range = y_upper - y_lower;
            // var y_step = y_range / GlobalDataModel.NumberOfSamples;

            var y_values = LinSpace(y_lower, y_upper, GlobalDataModel.NumberOfSamples).ToArray(); 
                //CreateRange(y_lower, y_upper, GlobalDataModel.NumberOfSamples);

            var raw_vertices = new List<Vector3>();
            var display_vertices = new List<Vector3>();

            var zmin = float.MaxValue;
            var zmax = float.MinValue;

            
            
            for(int i = 0; i < GlobalDataModel.NumberOfSamples; i++)
            {
                var x = x_values[i];//i * x_step;
                float y;
                for (int j = 0; j < GlobalDataModel.NumberOfSamples; j++)
                {
                    y = y_values[j]; //j * y_step;    
                    var z = -math.sin(x) * math.sin(y);
                    
                    if (z < zmin) zmin = z;
                    if (z > zmax) zmax = z;

                    var calculatedVector = new Vector3(x, y, z);
                
                    raw_vertices.Add(calculatedVector);
                    
                    // Switch axis to create horizontal mesh
                    var displayVector = new Vector3(x, z, y);

                    //var displayVector = new Vector3(y, z, x);
                    
                    // Scale points to 1/10th
                    //displayVector *= ScalingVector;
                    displayVector = Vector3.Scale(displayVector, scalingVector);
                
                    
                    display_vertices.Add(displayVector);
                }

            }

            //Debug.Log("zmin: " + zmin + ", zmax: " + zmax);
            
            return new Tuple<List<Vector3>, List<Vector3>>(raw_vertices, display_vertices);
        }

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
        
        public static Tuple<List<Vector3>, List<Vector3>> CalculateField02(Vector3 scalingVector)
        {
            var x_lower = -2f;
            var x_upper = 2f;
            // var x_range = x_upper - x_lower;
            // var x_step = x_range / GlobalDataModel.NumberOfSamples;

            var x_values = CreateRange(x_lower, x_upper, GlobalDataModel.NumberOfSamples);
            
            
            var y_lower = -2f;
            var y_upper = 2f;
            // var y_range = y_upper - y_lower;
            // var y_step = y_range / GlobalDataModel.NumberOfSamples;

            var y_values = CreateRange(y_lower, y_upper, GlobalDataModel.NumberOfSamples);

            var raw_vertices = new List<Vector3>();
            var display_vertices = new List<Vector3>();

            for(int i = 0; i < GlobalDataModel.NumberOfSamples; i++)
            {
                var x = x_values[i];
                float y;
                for (int j = 0; j < GlobalDataModel.NumberOfSamples; j++)
                {
                    y = y_values[j];
                    //var r = Mathf.Sqrt(x * x + y * y );
                    //var z = (float) (Math.Pow((float)Math.E, r) * Math.Cos(6f * r));

                    var r = Mathf.Sqrt(x * x + y * y);
                    var z = Mathf.Exp(-r) * Mathf.Cos(6f * r); //Mathf.Sin(r) / r;

                    //var z = (Mathf.Cos(x * x + y * y)) / (1 + x * x + y * y);
                    
                    //var z = 100 * Mathf.Pow((y - x * x), 2) + Mathf.Pow((1f - x), 2);
                    
                    //var z = Mathf.Pow(x, 2) + Mathf.Pow(y, 2);
                    var calculatedVector = new Vector3(x, y, z);

                    raw_vertices.Add(calculatedVector);
                    
                    
                    //Debug.Log("calculatedVector: " + calculatedVector);
                    
                    // Switch axis to create horizontal mesh
                    var displayVector = new Vector3(x, z, y);
                    // Scale points to 1/10th
                    //displayVector *= ScalingVector;
                    displayVector = Vector3.Scale(displayVector, scalingVector);
                
                    display_vertices.Add(displayVector);
                }

            }

            return new Tuple<List<Vector3>, List<Vector3>>(raw_vertices, display_vertices);
        }


        public static Tuple<List<Vector3>, List<Vector3>> CalculateField(ScalarField sf, Vector3 scalingVector)
        {
            //var x_lower = -2f;
            //var x_upper = 2f;
            // var x_range = x_upper - x_lower;
            // var x_step = x_range / GlobalDataModel.NumberOfSamples;

            var x_values = CreateRange(
                sf.parameterRangeX.Item1, 
                sf.parameterRangeX.Item2, 
                GlobalDataModel.NumberOfSamples
            );
            
            //var y_lower = -2f;
            //var y_upper = 2f;
            // var y_range = y_upper - y_lower;
            // var y_step = y_range / GlobalDataModel.NumberOfSamples;

            var y_values = CreateRange(
                sf.parameterRangeY.Item1, 
                sf.parameterRangeY.Item2, 
                GlobalDataModel.NumberOfSamples
            );

            var raw_vertices = new List<Vector3>();
            var display_vertices = new List<Vector3>();

            for(var i = 0; i < GlobalDataModel.NumberOfSamples; i++)
            {
                var x = x_values[i];
                for (int j = 0; j < GlobalDataModel.NumberOfSamples; j++)
                {
                    var y = y_values[j];
                    //var r = Mathf.Sqrt(x * x + y * y );
                    //var z = (float) (Math.Pow((float)Math.E, r) * Math.Cos(6f * r));

                    // var r = Mathf.Sqrt(x * x + y * y);
                    // var z = Mathf.Exp(-r) * Mathf.Cos(6f * r); //Mathf.Sin(r) / r;

                    var z = sf.CalculatePoint(x, y);
                    
                    //var z = (Mathf.Cos(x * x + y * y)) / (1 + x * x + y * y);
                    
                    //var z = 100 * Mathf.Pow((y - x * x), 2) + Mathf.Pow((1f - x), 2);
                    
                    //var z = Mathf.Pow(x, 2) + Mathf.Pow(y, 2);
                    var calculatedVector = new Vector3(x, y, z);

                    raw_vertices.Add(calculatedVector);
                    
                    //Debug.Log("calculatedVector: " + calculatedVector);
                    
                    // Switch axis to create horizontal mesh
                    var displayVector = new Vector3(x, z, y);
                    
                    // Scale points based on set scalign vector
                    displayVector = Vector3.Scale(displayVector, scalingVector);
                
                    display_vertices.Add(displayVector);
                }

            }

            return new Tuple<List<Vector3>, List<Vector3>>(raw_vertices, display_vertices);
        }
        
    }
}