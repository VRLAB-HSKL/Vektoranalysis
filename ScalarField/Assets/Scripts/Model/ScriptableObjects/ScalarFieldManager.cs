using System;
using System.Collections.Generic;
using System.Linq;
using Model.Enums;
using Model.InitFile;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

namespace Model.ScriptableObjects
{
    /// <summary>
    /// Data container for all data related to the imported scalar field dataset. This includes geometric data as well as
    /// additional information for dataset navigation
    /// </summary>
    [CreateAssetMenu(fileName = "ScalarField", menuName = "ScriptableObjects/ScalarFieldManager", order = 0)]
    public class ScalarFieldManager : ScriptableObject
    {
        /// <summary>
        /// Currently selected scalar field that is being visualized in the application
        /// </summary>
        public ScalarField CurrentField => ScalarFields[CurrentFieldIndex];

        private int fIds;

        public int CurrentFieldIndex
        {
            get
            {
                return fIds;
            }
            set
            {
                fIds = value;
                Debug.Log("fIds: " + value);
            }
            
        }

        // Make this scriptable object persistent between scenes
        //private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
        
        public List<ScalarField> ScalarFields { get; } = new List<ScalarField>();

    
        /// <summary>
        /// Tree data structure the init file is parsed into
        /// </summary>
        public InitFileRoot InitFile { get; private set; }


        public void OnEnable()
        {
            ParseInitFile();
        }

        /// <summary>
        /// Parses the JSON init file and creates the tree data structure in <see cref="InitFile"/>
        /// </summary>
        private void ParseInitFile()
        {
            // Load resource
            var json = Resources.Load(PathManager.InitFileResourcePath) as TextAsset;

            if (json is null)
            {
                Debug.LogError("JSON init file resource not found!");
                return;
            }
        
            // Catch errors that occur during parsing
            var errors = new List<ErrorEventArgs>();
            ITraceWriter tr = new MemoryTraceWriter();
            var jsr = JsonConvert.DeserializeObject<InitFileRoot>(json.text,
                new JsonSerializerSettings()
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        errors.Add(args);
                        
                        
                        var obj = args.CurrentObject != null ? args.CurrentObject.ToString() : "empty";
                        var ogObj = args.ErrorContext.OriginalObject;
                        var member = args.ErrorContext.Member;
                        var path = args.ErrorContext.Path;
                        var errorMsg = args.ErrorContext.Error.Message;

                        Debug.Log("obj: " + obj + "\n"
                                  + "ogObj: " + ogObj + "\n"
                                  + "member: " + member + "\n"
                                  + "path: " + path + "\n"
                                  + "errorMsg: " + errorMsg);
                        
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

            foreach (var field in InitFile.displayFields)
            {
                // Create scalar field based on init file
                var sf = new ScalarField
                {
                    ID = field.Info.ID,
                    ColorMapId = field.Info.ColorMapID,
                    ColorMapDataClassesCount = field.Info.ColorMapDataClassesCount,
                    ParameterRangeX = new Tuple<float, float>(field.Info.XParamRange[0], field.Info.XParamRange[1]),
                    ParameterRangeY = new Tuple<float, float>(field.Info.YParamRange[0], field.Info.YParamRange[1]),
                    SampleCount = field.Info.SampleCount
                };
                
                var importedPoints = field.Data.mesh.Points;
                foreach (var point in importedPoints)
                {
                    sf.RawPoints.Add(new Vector3(point[0], point[1], point[2]));
                    sf.DisplayPoints.Add(new Vector3(point[0], point[2], point[1]));
                }

                foreach (var criticalPoint in field.Data.mesh.CriticalPoints)
                {
                    var cp = new CriticalPointData()
                    {
                        PointIndex = int.Parse(criticalPoint[0])
                    };
                
                    var type = criticalPoint[1];
                    switch (type)
                    {
                        default:
                            cp.Type = CriticalPointType.CRITICAL_POINT;
                            break;
                    
                        case "LOCAL_MINIMUM":
                            cp.Type = CriticalPointType.LOCAL_MINIMUM;
                            break;
                    
                        case "LOCAL_MAXIMUM":
                            cp.Type = CriticalPointType.LOCAL_MAXIMUM;
                            break;
                    
                        case "SADDLE_POINT":
                            cp.Type = CriticalPointType.SADDLE_POINT;
                            break;
                    }

                    sf.CriticalPoints.Add(cp);
                }
            
                var grads = field.Data.mesh.Gradients.OrderBy(x => x.Index).ToList();
                
                for(var i = 0; i < grads.Count; i++)
                {
                    // if (i % 27 != 0)
                    //     continue;
                    
                    var gradient = grads[i];
                    var grad = new Gradient()
                    {
                        Index = gradient.Index,
                        Direction = new Vector3(gradient.Direction[0], gradient.Direction[1])
                    };
                    sf.Gradients.Add(grad);
                }

                sf.SteepestDescentPaths = ParsePath(field.Data.mesh.Paths.SteepestDescent);
                sf.NelderMeadPaths = ParsePath(field.Data.mesh.Paths.NelderMead);
                sf.NewtonPaths = ParsePath(field.Data.mesh.Paths.Newton);
                sf.NewtonDiscretePaths = ParsePath(field.Data.mesh.Paths.NewtonDiscrete);
                sf.NewtonTrustedPaths = ParsePath(field.Data.mesh.Paths.NewtonTrusted);
                sf.BFGSPaths = ParsePath(field.Data.mesh.Paths.BFGS);
            
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
            
            //Debug.Log("idx: " + CurrentFieldIndex + ", sf count: " + ScalarFields.Count);
            // Set index to 0 on application start
            //CurrentFieldIndex = 0;

            if (CurrentFieldIndex >= ScalarFields.Count)
            {
                CurrentFieldIndex = 0;
            }
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
}
