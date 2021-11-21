// using System;
// using System.Collections.Generic;
// using Newtonsoft.Json;
//
// namespace Import
// {
//     [Serializable]
//     public class PointDataJsonRoot
//     {
//         [JsonProperty("name")]
//         public string Name { get; set; }
//         
//         [JsonProperty("dim")]
//         public int dim { get; set; }
//         
//         [JsonProperty("arcLength")]
//         public float arcLength { get; set; }
//         
//         [JsonProperty("worldScalingFactor")]
//         public float worldScalingFactor { get; set; }
//         
//         [JsonProperty("tableScalingFactor")]
//         public float tableScalingFactor { get; set; }
//         
//         [JsonProperty("selectExercisePillarScalingFactor")]
//         public float selectExercisePillarScalingFactor { get; set; }
//
//         [JsonProperty("data")]
//         public List<PointData> data { get; set; } = new List<PointData>();
//         //public System.Data.DataSet pointData { get; set; } = new System.Data.DataSet();
//     }
// }
