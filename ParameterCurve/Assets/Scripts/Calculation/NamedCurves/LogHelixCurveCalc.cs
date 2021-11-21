using System.Collections.Generic;
using UnityEngine;

namespace Calculation.NamedCurves
{
    public class LogHelixCurveCalc : AbstractCurveCalc
    {
        public float A = 2f;
        public float B = 0.08f;
        public float Height = 1f;
        
        public LogHelixCurveCalc()
        {
            Name = "LogHelix";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(0f, 8f * Mathf.PI, NumOfSamples));
        }    

        protected override Vector3 CalculatePoint(float t)
        {
            float r = A * Mathf.Exp(B * t);
            float x = r * Mathf.Cos(t);
            float y = r * Mathf.Sin(t);
            float z = Height * t;
            return new Vector3(x, y, z);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float r = A * Mathf.Exp(B * t);
            float x = -r * Mathf.Sin(t);
            float y = r * Mathf.Cos(t);
            float z = Height;
            return new Vector3(x, y, z).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float r = A * Mathf.Exp(B * t);
            float x = -r * Mathf.Cos(t);
            float y = -r * Mathf.Sin(t);
            float z = 1f;
            return new Vector3(x, y, z).normalized;
        }

    }
}
