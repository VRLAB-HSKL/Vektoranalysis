using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Calculation
{
    /// <summary>
    /// Abstract base class for local calculation classes. Subclasses of this class calculate the points of specific
    /// curves.
    ///
    /// </summary>
    public abstract class AbstractCurveCalc
    {
        #region Public members
        
        /// <summary>
        /// Display string used in GUI
        /// </summary>
        public string DisplayString => Name;
        
        #endregion Public members
        
        #region Protected members
        
        /// <summary>
        /// Name of the curve
        /// </summary>
        protected string Name;

        /// <summary>
        /// Number of samples out of the parameter range
        /// </summary>
        protected int NumOfSamples;

        /// <summary>
        /// Collection of parameter values, usually as a range
        /// </summary>
        protected List<float> ParameterRange { get; set; } = new List<float>();

        /// <summary>
        /// Function object to calculate a single curve point based on a float parameter
        /// </summary>
        protected Func<float, Vector3> PointCalcFunc;

        /// <summary>
        /// Function object to calculate a single velocity point based on a float parameter
        /// </summary>
        protected Func<float, Vector3> VelocityCalcFunc;

        /// <summary>
        /// Function object to calculate a single acceleration point based on a float parameter
        /// </summary>
        protected Func<float, Vector3> AccelerationCalcFunc;

        #endregion Protected members
        
        #region Constructors
        
        protected AbstractCurveCalc()
        {
            PointCalcFunc = CalculatePoint;
            VelocityCalcFunc = CalculateVelocityPoint;
            AccelerationCalcFunc = CalculateAccelerationPoint;
        }

        #endregion Constructors
        
        #region Protected functions
        
        /// <summary>
        /// Calculate curve point vectors, based on parameter range set in constructor
        /// </summary>
        /// <returns>Point vectors</returns>
        protected List<Vector3> CalculatePoints()
        {
            return CalculateAllPointsIntoList(PointCalcFunc);
        }

        /// <summary>
        /// Calculate curve velocity vectors, based on parameter range set in constructor
        /// </summary>
        /// <returns>Velocity vectors</returns>
        protected List<Vector3> CalculateVelocity()
        {
            return CalculateAllPointsIntoList(VelocityCalcFunc);
        }

        /// <summary>
        /// Calculate curve acceleration vectors, based on parameter range set in constructor
        /// </summary>
        /// <returns>Acceleration vectors</returns>
        protected List<Vector3> CalculateAcceleration()
        {
            return CalculateAllPointsIntoList(AccelerationCalcFunc);
        }

        /// <summary>
        /// Calculates the length of a given polyline
        /// </summary>
        /// <param name="pointList">Polyline as a collection of point vectors</param>
        /// <returns>Calculated distance</returns>
        protected static float CalculateRawDistance(List<Vector3> pointList)
        {
            if (pointList.Count < 2) return 0f;

            var distance = 0f;
            for(var i = 1; i < pointList.Count; i++)
            {
                distance += Mathf.Abs(Vector3.Distance(pointList[i - 1], pointList[i]));
            }

            return distance;
        }

        /// <summary>
        /// Source:
        /// https://stackoverflow.com/questions/17046293/is-there-a-linspace-like-method-in-math-net/67131017#67131017 
        /// </summary>
        /// <param name="startVal">start of range</param>
        /// <param name="endVal">end of range</param>
        /// <param name="steps">step count</param>
        /// <returns></returns>
        protected static IEnumerable<float> Linspace(float startVal, float endVal, int steps)
        {
            var interval = (endVal / Mathf.Abs(endVal)) * Mathf.Abs(endVal - startVal) / (steps - 1);
            return (from val in Enumerable.Range(0, steps)
                select startVal + (val * interval)).ToArray();
        }

        /// <summary>
        /// Point vector calculation function. Subclasses implement their own calculation by implementing
        /// this function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Point vector</returns>
        protected abstract Vector3 CalculatePoint(float t);
        
        /// <summary>
        /// Velocity vector calculation function. Subclasses implement their own calculation by implementing
        /// this function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Velocity vector</returns>
        protected abstract Vector3 CalculateVelocityPoint(float t);
        
        /// <summary>
        /// Acceleration vector calculation function. Subclasses implement their own calculation by implementing
        /// this function
        /// </summary>
        /// <param name="t">Parameter value</param>
        /// <returns>Acceleration vector</returns>
        protected abstract Vector3 CalculateAccelerationPoint(float t);

        #endregion Protected functions
        
        #region Private functions
        
        private List<Vector3> CalculateAllPointsIntoList(Func<float, Vector3> f)
        {
            return ParameterRange.Select(f).ToList();
        }
        
        #endregion Private functions

    }
}


