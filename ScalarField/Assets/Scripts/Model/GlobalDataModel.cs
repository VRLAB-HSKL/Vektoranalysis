
using System;
using System.Collections.Generic;
using System.Text;
using Calculation;
using Model;
using Model.InitFile;
using UnityEngine;
using Valve.Newtonsoft.Json;
using Valve.Newtonsoft.Json.Serialization;

public static class GlobalDataModel
{
    public static int EstimatedIndex = 0;
    
    /// <summary>
    /// Global sample count to make mesh indexing applicable to every mesh
    /// </summary>
    public static int NumberOfSamples = 200;

    public static Vector3 ClosestPointOnMesh = Vector3.zero;
    public static Vector3 MainMeshScalingVector = Vector3.one;

    public static string InitFileResourcePath = "json/init/sf_initFile";

    public static InitFileRoot InitFile { get; set; }
    
    public static ScalarField CurrentField { get; set; }
    
    
    public static void InitializeData()
    {
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

        InitFile = jsr;

        var sf = new ScalarField()
        {
            id = InitFile.id,
            colorMapId = InitFile.colorMapId,
            colorMapDataClassesCount = InitFile.colorMapDataClassesCount,
            parameterRangeX = new Tuple<float, float>(-2.0f, 2.0f),
            parameterRangeY = new Tuple<float, float>(-2.0f, 2.0f),
            sampleCount = NumberOfSamples
        };

        //var sb = new StringBuilder();
        
        for (var i = 0; i < InitFile.pointVec.Count; i++)
        {
            var point = InitFile.pointVec[i];
            //sb.AppendLine(point[0] + ", " + point[1] + ", " + point[2]);
            sf.pointVectors.Add(new Vector3(point[0], point[1], point[2]));
        }

        //Debug.Log(sb);

        var cmapId = sf.colorMapId;
        var cmapDataClassesCount = sf.colorMapDataClassesCount;
        var ressPath = "texture_maps/" + cmapId + "/" + cmapDataClassesCount + "/" +
                       cmapId + "_" + cmapDataClassesCount + "_texture";
        
        Debug.Log(ressPath);

        
        
        var texture = Resources.Load(ressPath) as Texture2D;

        

        if (texture is null)
        {
            Debug.Log("empty texture");
        }
        
        sf.meshTexture = texture;

        CurrentField = sf;

    }
}
