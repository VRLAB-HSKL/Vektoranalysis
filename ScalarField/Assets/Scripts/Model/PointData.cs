using UnityEngine;

namespace Model
{
    public class PointData
    {
        /// <summary>
        /// Raw mathematical values that were imported from the init file.
        /// These are used in algorithms and calculations that are independent of their place in the 3d scene
        /// </summary>
        public Vector3 Raw;
        
        /// <summary>
        /// Translated values used for displaying the points in the unity scene.
        /// This vector has usually been altered by translation operations to place it in relation to its
        /// parents origin, fit inside a bounding box, ...
        /// </summary>
        public Vector3 Display;
        
    }
}