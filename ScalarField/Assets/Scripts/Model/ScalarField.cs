using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class ScalarField
    {
        // General information
        public string id;
        public string colorMapId;
        public string colorMapDataClassesCount;
        public Texture2D meshTexture;
        
        
        // Geometric data
        public Tuple<float, float> parameterRangeX;
        public Tuple<float, float> parameterRangeY;
        public int sampleCount = GlobalDataModel.NumberOfSamples;

        //public delegate float CalculatePoint(float x, float y);

        public Func<float, float, float> CalculatePoint;

        public Vector3 MinRawValues = new Vector3();
        public Vector3 MaxRawValues = new Vector3();
        
        
        // ToDo: Replace this with single collection of PointData class
        public List<Vector3> rawPoints { get; } = new List<Vector3>();
        public List<Vector3> displayPoints { get; set; } = new List<Vector3>();
    }
}