using System.Collections.Generic;
using UnityEngine;

namespace Calculation.NamedCurves
{
    public class HelixCurveCalc : AbstractCurveCalc
    {
        private float Radius = 2f;
        private float Height = 4f;

        public HelixCurveCalc()
        {
            Name = "Helix";
            NumOfSamples = 200;
            ParameterRange = new List<float>(Linspace(-1f, 1f, NumOfSamples));

            //float distance = Mathf.Sqrt(Height * Height + 4f * Mathf.PI * Mathf.PI * Radius * Radius);

        
            PointCalcFunc = CalculatePoint;
            VelocityCalcFunc = CalculateVelocityPoint;
            AccelerationCalcFunc = CalculateAccelerationPoint;
        }    

        protected override Vector3 CalculatePoint(float t)
        {
            float x = Radius * Mathf.Cos(2f * Mathf.PI * t);
            float y = Radius * Mathf.Sin(2f * Mathf.PI * t);
            float z = Height * t;
            return new Vector3(x, y, z);
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            float x = -2f * Mathf.PI * Radius * Mathf.Sin(2f * Mathf.PI * t);
            float y = 2f * Mathf.PI * Radius * Mathf.Cos(2f * Mathf.PI * t);
            float z = Height;
            return new Vector3(x, y, z).normalized;
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            float x = -4 * (Mathf.PI * Mathf.PI) * Radius * Mathf.Cos(2f * Mathf.PI * t);
            float y = -4 * (Mathf.PI * Mathf.PI) * Radius * Mathf.Sin(2f * Mathf.PI * t);
            float z = 1f;
            return new Vector3(x, y, z).normalized;
        }

    


    }
}
