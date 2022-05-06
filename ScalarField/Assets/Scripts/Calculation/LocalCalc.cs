using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Calculation
{
    public static class LocalCalc
    {
        public static List<Vector3> CalculateField01(Vector3 scalingVector)
        {
            var x_lower = -5f * math.PI;
            var x_upper = 5f * math.PI;
            // var x_range = x_upper - x_lower;
            // var x_step = x_range / GlobalDataModel.NumberOfSamples;

            var x_values = CreateRange(x_lower, x_upper, GlobalDataModel.NumberOfSamples);
            
            var y_lower = -5f * math.PI;
            var y_upper = 5f * math.PI;
            // var y_range = y_upper - y_lower;
            // var y_step = y_range / GlobalDataModel.NumberOfSamples;

            var y_values = CreateRange(y_lower, y_upper, GlobalDataModel.NumberOfSamples);
            
            var vertices = new List<Vector3>();

            for(int i = 0; i < GlobalDataModel.NumberOfSamples; i++)
            {
                var x = x_values[i];//i * x_step;
                float y = 0f;
                for (int j = 0; j < GlobalDataModel.NumberOfSamples; j++)
                {
                    y = y_values[j]; //j * y_step;    
                    var z = -math.sin(x) * math.sin(y);

                    var calculatedVector = new Vector3(x, y, z);
                
                    // Switch axis to create horizontal mesh
                    var displayVector = new Vector3(x, z, y);
                    // Scale points to 1/10th
                    //displayVector *= ScalingVector;
                    displayVector = Vector3.Scale(displayVector, scalingVector);
                
                    vertices.Add(displayVector);
                }

            }

            return vertices;
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
        
        public static List<Vector3> CalculateField02(Vector3 scalingVector)
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
            
            var vertices = new List<Vector3>();

            for(int i = 0; i < GlobalDataModel.NumberOfSamples; i++)
            {
                var x = x_values[i];
                float y = 0f;
                for (int j = 0; j < GlobalDataModel.NumberOfSamples; j++)
                {
                    y = y_values[j];
                    //var r = Mathf.Sqrt(x * x + y * y );
                    //var z = (float) (Math.Pow((float)Math.E, r) * Math.Cos(6f * r));

                    //var r = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                    //var z = Mathf.Sin(r) / r;

                    var z = 100 * Mathf.Pow((y - x * x), 2) + Mathf.Pow((1f - x), 2);
                    
                    //var z = Mathf.Pow(x, 2) + Mathf.Pow(y, 2);
                    var calculatedVector = new Vector3(x, y, z);

                    Debug.Log("calculatedVector: " + calculatedVector);
                    
                    // Switch axis to create horizontal mesh
                    var displayVector = new Vector3(x, z, y);
                    // Scale points to 1/10th
                    //displayVector *= ScalingVector;
                    displayVector = Vector3.Scale(displayVector, scalingVector);
                
                    vertices.Add(displayVector);
                }

            }

            return vertices;
        }
    }
}