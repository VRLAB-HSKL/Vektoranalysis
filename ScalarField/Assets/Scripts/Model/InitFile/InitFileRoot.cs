using System;
using System.Collections.Generic;
using Valve.Newtonsoft.Json;

namespace Model.InitFile
{
    [Serializable]
    public class InitFileRoot
    {
        [JsonProperty("id")] 
        public string id { get; set; }
        
        [JsonProperty("color_map_id")] 
        public string colorMapId { get; set; }
        
        [JsonProperty("color_map_data_classes_count")] 
        public string colorMapDataClassesCount { get; set; }
        
        [JsonProperty("x_param_range")] 
        public List<float> xParamRange { get; set; }
        
        [JsonProperty("y_param_range")] 
        public List<float> yParamRange { get; set; }
        
        [JsonProperty("z_expr")] 
        public string zExpression { get; set; }
        
        [JsonProperty("sample_count")] 
        public int sampleCount { get; set; }
        
        [JsonProperty("points")]
        public List<float[]> pointVec { get; set; }
    }
}