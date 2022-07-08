using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Data class encapsulating all needed attributes of a scalar field construct
    /// </summary>
    public class ScalarField
    {
        /// <summary>
        /// Unique identifier (name)
        /// </summary>
        public string ID;
        
        /// <summary>
        /// Color map identifier
        /// </summary>
        public string ColorMapId;
        
        /// <summary>
        /// Number of data classes in the color map
        /// </summary>
        public string ColorMapDataClassesCount;
        
        /// <summary>
        /// Texture asset used to texture the scalar field.
        /// Currently being retrieved from local resources based on
        /// <see cref="ColorMapId"/> and <see cref="ColorMapDataClassesCount"/>
        /// </summary>
        public Texture2D MeshTexture;
        
        /// <summary>
        /// Lower and upper bound of the parameter interval of the x parameter
        /// </summary>
        public Tuple<float, float> ParameterRangeX;
        
        /// <summary>
        /// Lower and upper bound of the parameter interval of the y parameter
        /// </summary>
        public Tuple<float, float> ParameterRangeY;
        
        /// <summary>
        /// Amount of points used to sample the function of the scalar field z value
        /// </summary>
        public int SampleCount;

        /// <summary>
        /// Minimum values of the scalar field in each dimension.
        /// Used to map the point values of the scalar field to different value ranges
        /// </summary>
        public Vector3 MinRawValues = new Vector3();
        
        /// <summary>
        /// Maximum values of the scalar field in each dimension.
        /// Used to map the point values of the scalar field to different value ranges
        /// </summary>
        public Vector3 MaxRawValues = new Vector3();
        
        // ToDo: Replace these with single collection of PointData class
        public List<Vector3> RawPoints { get; } = new List<Vector3>();
        public List<Vector3> DisplayPoints { get; set; } = new List<Vector3>();

        public List<Vector3> MeshPoints { get; set; } = new List<Vector3>();


        public List<CriticalPointData> CriticalPoints { get; set; } = new List<CriticalPointData>();
        public List<Vector3> Gradients { get; set; } = new List<Vector3>();

        public List<List<Vector3>> NelderMeadPaths { get; set; } = new List<List<Vector3>>();
        
        /// <summary>
        /// Target values of the imported contour lines
        /// </summary>
        public List<float> ContourLineValues { get; set; } = new List<float>();

        /// <summary>
        /// Point vector collections of the individual contour lines
        /// </summary>
        public List<List<Vector3>> ContourLinePoints { get; set; } = new List<List<Vector3>>();
        
    }
}