using UnityEngine;

namespace Model
{
    public class FresnetSerretApparatus
    {
        /// <summary>
        /// Unit vector tangent to the curve, pointing in the direction of motion
        /// Source: Fresnet-Serret formulas (Wikipedia)
        /// 
        ///           r'(t)
        /// T(t) = -----------
        ///         ||r'(t)||
        /// 
        /// </summary> 
        public Vector3 Tangent = Vector3.zero;

        /// <summary>
        /// Normal unit vector, derivative of Tangent with respect to the arc length
        /// parameter of the curve, divided by its length (normalized)
        /// Source: Fresnet-Serret formulas (Wikipedia)
        /// 
        ///           T'(t)
        /// N(t) = -----------
        ///         ||T'(t)||
        ///
        /// </summary> 
        public Vector3 Normal = Vector3.zero;

        /// <summary>
        /// Binormal unit vector, cross-product of Tangent and Normal
        /// Source: Fresnet-Serret formulas (Wikipedia)
        /// 
        /// B(t) = T x N
        /// 
        /// </summary> 
        public Vector3 Binormal = Vector3.zero;

        /// <summary>
        /// Curvature of the curve
        /// </summary>
        public float Curvature;

        /// <summary>
        /// Rotation around the tangent
        /// </summary>
        public float Torsion;

    }
}