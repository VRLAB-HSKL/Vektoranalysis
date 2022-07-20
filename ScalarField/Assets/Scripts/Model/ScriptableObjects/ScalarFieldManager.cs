using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.InitFile;
using Model.ScriptableObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "ScalarField", menuName = "ScriptableObjects/ScalarFieldManager", order = 0)]
public class ScalarFieldManager : ScriptableObject
{
    public PathManager PathManager;
    
    /// <summary>
    /// Currently selected scalar field that is being visualized in the application
    /// </summary>
    public ScalarField CurrentField => ScalarFields[CurrentFieldIndex];

    public int CurrentFieldIndex { get; set; }

    public List<ScalarField> ScalarFields { get; set; } = new List<ScalarField>();

    
    /// <summary>
    /// Tree data structure the init file is parsed into
    /// </summary>
    public InitFileRoot InitFile { get; private set; }
    
    
    /// <summary>
    /// Parses the JSON init file and creates the tree data structure in <see cref="InitFile"/>
    /// </summary>
    public void ParseInitFile()
    {
        // Load resource
        var json = Resources.Load(PathManager.InitFileResourcePath) as TextAsset;

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

        foreach (var field in InitFile.DisplayFields)
        {
            // Create scalar field based on init file
            var sf = new ScalarField
            {
                ID = field.Info.id,
                ColorMapId = field.Info.color_map_id,
                ColorMapDataClassesCount = field.Info.color_map_data_classes_count,
                ParameterRangeX = new Tuple<float, float>(field.Info.x_param_range[0], field.Info.x_param_range[1]),
                ParameterRangeY = new Tuple<float, float>(field.Info.y_param_range[0], field.Info.y_param_range[1]),
                SampleCount = field.Info.sample_count
            };

            var importedPoints = field.Data.mesh.points;
            //var pointList = PythonUtility.CalculatePoints(); //field.Data.mesh.points;
            
            //Debug.Log("imported points count: " + importedPoints.Count + ", python point count: " + pointList.Count);
            
            foreach (var point in importedPoints)
            {
                sf.RawPoints.Add(new Vector3(point[0], point[1], point[2]));
                sf.DisplayPoints.Add(new Vector3(point[0], point[2], point[1]));
            }

            foreach (var critPoint in field.Data.mesh.CriticalPoints)
            {
                var cp = new CriticalPointData()
                {
                    PointIndex = int.Parse(critPoint[0])
                };
                
                var type = critPoint[1];
                switch (type)
                {
                    //case "CriticalPointType.CRITICAL_POINT":
                    default:
                        cp.Type = CriticalPointType.CRITICAL_POINT;
                        break;
                    
                    case "CriticalPointType.LOCAL_MINIMUM":
                        cp.Type = CriticalPointType.LOCAL_MINIMUM;
                        break;
                    
                    case "CriticalPointType.LOCAL_MAXIMUM":
                        cp.Type = CriticalPointType.LOCAL_MAXIMUM;
                        break;
                    
                    case "CriticalPointType.SADDLE_POINT":
                        cp.Type = CriticalPointType.SADDLE_POINT;
                        break;
                }
                
                sf.CriticalPoints.Add(cp);
            }
            
            foreach (var gradient in field.Data.mesh.gradients)
            {
                sf.Gradients.Add(new Vector3(gradient[0], gradient[1], gradient[2]));
            }


            sf.SteepestDescentPaths = ParsePath(field.Data.mesh.Paths.SteepestDescent);
            sf.NelderMeadPaths = ParsePath(field.Data.mesh.Paths.NelderMead);
            sf.NewtonPaths = ParsePath(field.Data.mesh.Paths.Newton);
            sf.NewtonDiscretePaths = ParsePath(field.Data.mesh.Paths.NewtonDiscrete);
            sf.NewtonTrustedPaths = ParsePath(field.Data.mesh.Paths.NewtonTrusted);
            sf.BFGSPaths = ParsePath(field.Data.mesh.Paths.BFGS);
            
            // for (var i = 0; i < field.Data.mesh.Paths.NelderMead.Count; i++)
            // {
            //     var nmPath = field.Data.mesh.Paths.NelderMead[i];
            //     var vecList = new List<Vector3>();
            //     for (var j = 0; j < nmPath.Count; j++)
            //     {
            //         var pathPoint = nmPath[j];
            //         var vec = new Vector3(pathPoint[0], pathPoint[1], 0f);
            //         vecList.Add(vec);
            //     }
            //
            //     sf.NelderMeadPaths.Add(vecList);
            // }

            // for (var i = 0; i < sf.NelderMeadPaths.Count; i++)
            // {
            //     Debug.Log(i + " - Count: " + sf.NelderMeadPaths[i].Count);
            // }
            
            
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

            
            sf.ContourLineValues = field.Data.isolines.Values;

            var lst = new List<List<Vector3>>();
            foreach (var line in field.Data.isolines.ConvexHulls)
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
            var textureResourcePath = "texture_maps/" 
                                      + sf.ID + "/"
                                      + colorMapId + "/" 
                                      + colorMapDataClassesCount + "/" 
                                      + colorMapId + "_" + colorMapDataClassesCount + "_texture";
        
            var texture = Resources.Load(textureResourcePath) as Texture2D;
            if (texture is null)
            {
                Debug.LogWarning("Unable to find texture map in local resources!");
            }
        
            sf.MeshTexture = texture;

            //CurrentField = sf;

            ScalarFields.Add(sf);
        }
        
        // Set index to 0 on application start
        CurrentFieldIndex = 0;
    }
    
    private List<List<Vector3>> ParsePath(List<List<float[]>> paths)
    {
        var retList = new List<List<Vector3>>();
        for (var i = 0; i < paths.Count; i++)
        {
            var currPath = paths[i];
            var vecList = new List<Vector3>();
            for (var j = 0; j < currPath.Count; j++)
            {
                var pathPoint = currPath[j];
                var vec = new Vector3(pathPoint[0], pathPoint[1], 0f);
                vecList.Add(vec);
            }

            retList.Add(vecList);
        }

        return retList;
    }
}
