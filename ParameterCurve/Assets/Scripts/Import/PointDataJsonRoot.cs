using System;
using System.Collections.Generic;

namespace Import
{
    [Serializable]
    public class PointDataJsonRoot
    {
        public string name { get; set; }
        public int dim { get; set; }
        public float arcLength { get; set; }
        
        public float worldScalingFactor { get; set; }
        public float tableScalingFactor { get; set; }
        public float selectExercisePillarScalingFactor { get; set; }

        public List<PointData> data { get; set; } = new List<PointData>();
        //public System.Data.DataSet pointData { get; set; } = new System.Data.DataSet();
    }
}
