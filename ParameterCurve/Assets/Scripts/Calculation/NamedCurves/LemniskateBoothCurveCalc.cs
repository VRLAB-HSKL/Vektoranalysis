using System.Collections.Generic;
using UnityEngine;

namespace Calculation.NamedCurves
{
    public class LemniskateBoothCurveCalc : AbstractCurveCalc
    {
        // ToDo: Implement this class
        public LemniskateBoothCurveCalc()
        {
            Name = "LemniskateBooth";
        }

        protected override Vector3 CalculatePoint(float t)
        {
            throw new System.NotImplementedException();
        }

        protected override Vector3 CalculateVelocityPoint(float t)
        {
            throw new System.NotImplementedException();
        }

        protected override Vector3 CalculateAccelerationPoint(float t)
        {
            throw new System.NotImplementedException();
        }
    }
}
