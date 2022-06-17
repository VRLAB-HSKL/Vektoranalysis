using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

//using Valve.Newtonsoft.Json;

namespace Model.InitFile
{
    [Serializable]
    public class InitFileRoot
    {
        [JsonProperty("id")] 
        public string id { get; set; }
        
        [JsonProperty("color_map_id")] 
        public string color_map_id { get; set; }
        
        [JsonProperty("color_map_data_classes_count")] 
        public string color_map_data_classes_count { get; set; }
        
        [JsonProperty("x_param_range")] 
        public List<float> x_param_range { get; set; }
        
        [JsonProperty("y_param_range")] 
        public List<float> y_param_range { get; set; }
        
        [JsonProperty("z_expr")] 
        public string z_expression { get; set; }
        
        [JsonProperty("sample_count")] 
        public int sample_count { get; set; }
        
        [JsonProperty("points")]
        public List<float[]> points { get; set; }

        [JsonProperty("isoline_values")]
        public List<float> IsolineValues { get; set; }

        [JsonProperty("isoline_points")]
        public List<List<float[]>> IsolinePoints { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("id: " + id);
            sb.AppendLine("color_map_id: " + id);
            sb.AppendLine("color_map_data_classes_count: " + color_map_data_classes_count);
            sb.AppendLine("x_param_range: " + x_param_range);
            sb.AppendLine("y_param_range: " + y_param_range);
            sb.AppendLine("z_expr: " + z_expression);
            sb.AppendLine("sample_count: " + sample_count);
            sb.AppendLine("points: " + points);
            sb.AppendLine("isoline_values: " + IsolineValues);
            sb.AppendLine("isoline_points: " + IsolinePoints);
            
            return sb.ToString();
        }
        
    }
}