using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Model.InitFile
{
    /// <summary>
    /// Root class for parsing of the JSON initialization file
    /// </summary>
    [Serializable]
    public class InitFileRoot
    {
        [JsonProperty("display_fields")] 
        public List<InitFileScalarField> displayFields;
    }
    
    [Serializable]
    public class InitFileScalarField
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

            sb.AppendLine("id: " + Info.ID);
            sb.AppendLine("color_map_id: " + Info.ColorMapID);
            sb.AppendLine("color_map_data_classes_count: " + Info.ColorMapDataClassesCount);
            sb.AppendLine("x_param_range: " + Info.XParamRange);
            sb.AppendLine("y_param_range: " + Info.YParamRange);
            sb.AppendLine("z_expr: " + Info.ZExpression);
            sb.AppendLine("sample_count: " + Info.SampleCount);
            sb.AppendLine("points: " + Data.mesh.Points);
            sb.AppendLine("isoline_values: " + Data.isolines.Values);
            sb.AppendLine("isoline_line_points: " + Data.isolines.ConvexHulls);
            
            return sb.ToString();
        }
        
    }

    public class InitFileInfo
    {
        [JsonProperty("id")] 
        public string ID { get; set; }
        
        [JsonProperty("color_map_id")] 
        public string ColorMapID { get; set; }
        
        [JsonProperty("color_map_data_classes_count")] 
        public string ColorMapDataClassesCount { get; set; }
        
        [JsonProperty("colors")]
        public List<float[]> Colors { get; set; }
        
        [JsonProperty("x_param_range")] 
        public List<float> XParamRange { get; set; }
        
        [JsonProperty("y_param_range")] 
        public List<float> YParamRange { get; set; }
        
        [JsonProperty("z_expr")] 
        public string ZExpression { get; set; }
        
        [JsonProperty("sample_count")] 
        public int SampleCount { get; set; }
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
        public List<float[]> Points { get; set; }
        
        [JsonProperty("critical_points")]
        public List<string[]> CriticalPoints { get; set; }
        
        [JsonProperty("gradients")]
        public List<InitFileGradient> Gradients { get; set; }
        
        [JsonProperty("paths")]
        public InitFilePaths Paths { get; set; }
    }

    public class InitFileGradient
    {
        [JsonProperty("index")]
        public int Index;

        [JsonProperty("direction")]
        public float[] Direction;
    }
    
    public class InitFilePaths
    {
        [JsonProperty("steepest_descent")]
        public List<List<float[]>> SteepestDescent { get; set; }
        
        [JsonProperty("nelder_mead")]
        public List<List<float[]>> NelderMead { get; set; }
        
        [JsonProperty("newton")]
        public List<List<float[]>> Newton { get; set; }
        
        [JsonProperty("newton_discrete")]
        public List<List<float[]>> NewtonDiscrete { get; set; }
        
        [JsonProperty("newton_trusted")]
        public List<List<float[]>> NewtonTrusted { get; set; }
        
        [JsonProperty("bfgs")]
        public List<List<float[]>> BFGS { get; set; }
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

    public class InitFileArrow {}
    
    public class InitFileSettings {}
}