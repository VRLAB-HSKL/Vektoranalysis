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
        [JsonProperty("info")]
        public InitFileInfo Info;

        [JsonProperty("data")]
        public InitFileData Data;

        [JsonProperty("settings")]
        public InitFileSettings Settings;
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("id: " + Info.id);
            sb.AppendLine("color_map_id: " + Info.color_map_id);
            sb.AppendLine("color_map_data_classes_count: " + Info.color_map_data_classes_count);
            sb.AppendLine("x_param_range: " + Info.x_param_range);
            sb.AppendLine("y_param_range: " + Info.y_param_range);
            sb.AppendLine("z_expr: " + Info.z_expression);
            sb.AppendLine("sample_count: " + Info.sample_count);
            sb.AppendLine("points: " + Data.mesh.points);
            sb.AppendLine("isoline_values: " + Data.isolines.Values);
            sb.AppendLine("isoline_line_points: " + Data.isolines.ConvexHulls);
            
            return sb.ToString();
        }
        
    }

    public class InitFileInfo
    {
        [JsonProperty("id")] 
        public string id { get; set; }
        
        [JsonProperty("color_map_id")] 
        public string color_map_id { get; set; }
        
        [JsonProperty("color_map_data_classes_count")] 
        public string color_map_data_classes_count { get; set; }
        
        [JsonProperty("colors")]
        public List<float[]> Colors { get; set; }
        
        [JsonProperty("x_param_range")] 
        public List<float> x_param_range { get; set; }
        
        [JsonProperty("y_param_range")] 
        public List<float> y_param_range { get; set; }
        
        [JsonProperty("z_expr")] 
        public string z_expression { get; set; }
        
        [JsonProperty("sample_count")] 
        public int sample_count { get; set; }
    }

    public class InitFileData
    {
        [JsonProperty("mesh")] 
        public InitFileMesh mesh;

        [JsonProperty("isolines")] 
        public InitFileIsoLines isolines;

        [JsonProperty("gizmos")] 
        public InitFileGizmos gizmos;
    }

    public class InitFileMesh
    {
        [JsonProperty("points")]
        public List<float[]> points { get; set; }
    }

    public class InitFileIsoLines
    {
        [JsonProperty("values")]
        public List<float> Values { get; set; }
        
        [JsonProperty("convex_hulls")]
        public List<List<float[]>> ConvexHulls { get; set; }
        
    }

    public class InitFileGizmos
    {
        [JsonProperty("points")] 
        public List<float[]> Points;

        [JsonProperty("arrows")] 
        public List<InitFileArrow> Arrows;
    }

    public class InitFileArrow
    {
        public float[] Origin;
        public float[] Direction;

    }
    
    
    public class InitFileSettings
    {
        
    }

}