using System;
using System.Collections.Generic;
using System.Linq;
using Model.InitFile;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Static global model class containing constants and data that has to be easily accessible to the whole application
    /// </summary>
    public static class GlobalDataModel
    {
        public static int EstimatedIndex = 0;

        public static Vector3 ClosestPointOnMesh = Vector3.zero;

        /// <summary>
        /// Path to the init file resource
        /// </summary>
        private const string InitFileResourcePath = "json/init/sf_initFile";

        /// <summary>
        /// Tree data structure the init file is parsed into
        /// </summary>
        public static InitFileRoot InitFile { get; private set; }
    
        /// <summary>
        /// Currently selected scalar field that is being visualized in the application
        /// </summary>
        public static ScalarField CurrentField { get; private set; }

        /// <summary>
        /// Initializes static global data class
        /// </summary>
        public static void InitializeData()
        {
            //IronPythonTest.Foo();
            ParseInitFile();
        }

        /// <summary>
        /// Parses the JSON init file and creates the tree data structure in <see cref="InitFile"/>
        /// </summary>
        private static void ParseInitFile()
        {
            // Load resource
            var json = Resources.Load(InitFileResourcePath) as TextAsset;

            if (json is null)
            {
                Debug.LogError("JSON init file resource not found!");
                return;
            }
            
            // Catch errors that occur during parsing
            var errors = new List<string>();
            ITraceWriter tr = new MemoryTraceWriter();
            var jsr = JsonConvert.DeserializeObject<InitFileRoot>(json.text,
                new JsonSerializerSettings()
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        Debug.Log("ErrorOccuredJSON");
                        args.ErrorContext.Handled = true;
                    },
                    TraceWriter = tr
                }
            );

            // Log error if init file was not parsed correctly
            if (jsr is null)
            {
                Debug.LogError("Failed to deserialize json!\n");
                return;
            }
        
            InitFile = jsr;

            // Create scalar field based on init file
            var sf = new ScalarField
            {
                ID = InitFile.Info.id,
                ColorMapId = InitFile.Info.color_map_id,
                ColorMapDataClassesCount = InitFile.Info.color_map_data_classes_count,
                ParameterRangeX = new Tuple<float, float>(InitFile.Info.x_param_range[0], InitFile.Info.x_param_range[1]),
                ParameterRangeY = new Tuple<float, float>(InitFile.Info.y_param_range[0], InitFile.Info.y_param_range[1]),
                SampleCount = InitFile.Info.sample_count
            };

            var importedPoints = InitFile.Data.mesh.points;
            var pointList = PythonUtility.CalculatePoints(); //InitFile.Data.mesh.points;
            
            Debug.Log("imported points count: " + importedPoints.Count + ", python point count: " + pointList.Count);
            
            foreach (var point in pointList)
            {
                sf.RawPoints.Add(new Vector3(point[0], point[1], point[2]));
                sf.DisplayPoints.Add(new Vector3(point[0], point[2], point[1]));
            }

            foreach (var gradient in InitFile.Data.mesh.gradients)
            {
                sf.Gradients.Add(new Vector3(gradient[0], gradient[1], gradient[2]));
            }
            
            sf.MinRawValues = new Vector3(
                sf.RawPoints.Min(v => v.x),
                sf.RawPoints.Min(v => v.y),
                sf.RawPoints.Min(v => v.z)
            );
        
            sf.MaxRawValues = new Vector3(
                sf.RawPoints.Max(v => v.x),
                sf.RawPoints.Max(v => v.y),
                sf.RawPoints.Max(v => v.z)
            );

            
            sf.ContourLineValues = jsr.Data.isolines.Values;

            var lst = new List<List<Vector3>>();
            foreach (var line in jsr.Data.isolines.ConvexHulls)
            {
                var vecList = new List<Vector3>();
                foreach (var point in line)
                {
                    vecList.Add(new Vector3(point[0], point[1], point[2]));
                }
                lst.Add(vecList);
            }

            sf.ContourLinePoints = lst;

            // Load texture based on chosen identifiers in init file
            var colorMapId = sf.ColorMapId;
            var colorMapDataClassesCount = sf.ColorMapDataClassesCount;
            var textureResourcePath = "texture_maps/" + colorMapId + "/" + colorMapDataClassesCount + "/" +
                           colorMapId + "_" + colorMapDataClassesCount + "_texture";
        
            var texture = Resources.Load(textureResourcePath) as Texture2D;
            if (texture is null)
            {
                Debug.LogWarning("Unable to find texture map in local resources!");
            }
        
            sf.MeshTexture = texture;

            CurrentField = sf;

        }
    }
}
