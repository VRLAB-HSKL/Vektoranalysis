
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculation;
using DefaultNamespace;
using Model;
using Model.InitFile;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
//using Valve.Newtonsoft.Json;
//using Valve.Newtonsoft.Json.Serialization;

public static class GlobalDataModel
{
    public static int EstimatedIndex = 0;
    
    /// <summary>
    /// Global sample count to make mesh indexing applicable to every mesh
    /// </summary>
    public static int NumberOfSamples = 200;

    public static Vector3 ClosestPointOnMesh = Vector3.zero;

    public static Vector3 MainMeshScalingVector = new Vector3(0.2f, 0.2f, 0.2f);
    public static Vector3 DetailMeshScalingVector = new Vector3(10f, 10f, 10f);
    
    
    public static string InitFileResourcePath = "json/init/sf_initFile";

    public static InitFileRoot InitFile { get; set; }
    
    public static ScalarField CurrentField { get; set; }


    //public static float ZMinValue;
    //public static float ZMaxValue;
    
    
    public static void InitializeData()
    {
        //IronPythonTest.Foo();
        ParseInitFile();
    }

    private static void ParseInitFile()
    {
        var json = Resources.Load(InitFileResourcePath) as TextAsset;
        
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

        //var jsr = JsonUtility.FromJson<InitFileRoot>(json.text);
        
        if(jsr is null)
            Debug.Log("Failed to deserialize json!\n" + jsr);

        InitFile = jsr;

        var sf = new ScalarField
        {
            id = InitFile.Info.id,
            colorMapId = InitFile.Info.color_map_id,
            colorMapDataClassesCount = InitFile.Info.color_map_data_classes_count,
            parameterRangeX = new Tuple<float, float>(InitFile.Info.x_param_range[0], InitFile.Info.x_param_range[1]),
            parameterRangeY = new Tuple<float, float>(InitFile.Info.y_param_range[0], InitFile.Info.y_param_range[1]),
            sampleCount = InitFile.Info.sample_count
        };

        //var sb = new StringBuilder();
        
        for (var i = 0; i < InitFile.Data.mesh.points.Count; i++)
        {
            var point = InitFile.Data.mesh.points[i];
            //sb.AppendLine(point[0] + ", " + point[1] + ", " + point[2]);
            sf.rawPoints.Add(new Vector3(point[0], point[1], point[2]));
            sf.displayPoints.Add(new Vector3(point[0], point[2], point[1]));
        }

        sf.MinRawValues = new Vector3(
                sf.rawPoints.Min(v => v.x),
                sf.rawPoints.Min(v => v.y),
                sf.rawPoints.Min(v => v.z)
        );
        
        sf.MaxRawValues = new Vector3(
            sf.rawPoints.Max(v => v.x),
            sf.rawPoints.Max(v => v.y),
            sf.rawPoints.Max(v => v.z)
        );


        sf.isolineValues = jsr.Data.isolines.Values;

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

        sf.isolinePoints = lst;

        foreach (var line in sf.isolinePoints)
        {
            //Debug.Log("iso line number of points: " + line.Count);
        }

        var cmapId = sf.colorMapId;
        var cmapDataClassesCount = sf.colorMapDataClassesCount;
        var ressPath = "texture_maps/" + cmapId + "/" + cmapDataClassesCount + "/" +
                       cmapId + "_" + cmapDataClassesCount + "_texture";
        
        Debug.Log("Selected texture: " +ressPath);
        
        var texture = Resources.Load(ressPath) as Texture2D;

        if (texture is null)
        {
            Debug.Log("empty texture");
        }
        
        sf.meshTexture = texture;

        CurrentField = sf;

    }
}
