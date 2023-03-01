using System.Collections.Generic;
using System.Diagnostics;
//using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ParamCurve.Scripts.Controller.Curve;
using ParamCurve.Scripts.Controller.Exercise;
using ParamCurve.Scripts.Import;
using ParamCurve.Scripts.Import.InitFile;
using ParamCurve.Scripts.Model;
using UnityEngine;
using Debug = UnityEngine.Debug;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace Model
{
    // ToDo: Remove this global singleton with scriptable objects architecture
    /// <summary>
    /// Static global data model. This singular model is used as the single access point of imported data and static
    /// application settings.
    /// </summary>
    public static class GlobalDataModel
    {
        #region Public members

        /// <summary>
        /// Global point scale factor, impacting all in game curves in addition to view local scale factors
        /// </summary>
        public const float PointScaleFactor = 1f;

        /// <summary>
        /// Factor to control speed of curve runs. The value specifies the time in seconds that has to pass before a
        /// new point is traveled to. A value of 1.0 means a new point is reached every 1 second. 
        /// </summary>
        public const float RunSpeedFactor = 0.0125f;

        /// <summary>
        /// View controller of the main world curve display in the middle of the room
        /// </summary>
        public static CurveViewController WorldCurveViewController { get; set; }
    
        /// <summary>
        /// View controller of the curve display on the examination table
        /// </summary>
        public static CurveViewController TableCurveViewController { get; set; }
        
        /// <summary>
        /// View controller of the selection exercise 
        /// </summary>
        public static ExerciseCurveViewController ExerciseCurveController { get; set; }

        /// <summary>
        /// Type enum used to differentiate between the currently selected curve dataset. This can be
        /// changed by selection another group on the selection wall (if activated)
        /// </summary>
        public enum CurveDisplayGroup { Display = 0, Exercises = 1 }
        
        /// <summary>
        /// Currently selected curve dataset group
        /// </summary>
        public static CurveDisplayGroup CurrentDisplayGroup { get; set; } = CurveDisplayGroup.Display;

        /// <summary>
        /// Static truth value signaling if runs are currently happening
        /// </summary>
        public static bool IsRunning { get; set; }

        
        /// <summary>
        /// Current dataset based on selected display group <see cref="CurrentDisplayGroup"/>
        /// </summary>
        public static List<CurveInformationDataset> CurrentDataset
        {
            get
            {
                return CurrentDisplayGroup switch
                {
                    //CurveDisplayGroup.Parameter => ParamCurveDatasets,
                    CurveDisplayGroup.Exercises => ExerciseCurveDatasets,
                    _ => DisplayCurveDatasets
                };
            }
        }
    

        /// <summary>
        /// Imported display curves
        /// </summary>
        public static List<CurveInformationDataset> DisplayCurveDatasets { get; } = new List<CurveInformationDataset>();
        
        /// <summary>
        /// Index used to access the current curve in the current curve dataset. This value is modified to switch
        /// between curves in a dataset
        /// </summary>
        public static int CurrentCurveIndex = 0;
        
        /// <summary>
        /// Imported exercise curves
        /// </summary>
        public static List<CurveInformationDataset> ExerciseCurveDatasets { get; } = new List<CurveInformationDataset>();

        /// <summary>
        /// Collection of imported selection exercises
        /// </summary>
        public static readonly List<AbstractExercise> SelectionExercises = new List<AbstractExercise>();

        /// <summary>
        /// Index used to access the current selection exercise. This value is modified to switch between main
        /// exercises, i.e. Exercise 1 -> Exercise 2
        /// </summary>
        public static int CurrentExerciseIndex { get; set; }= 0;
        
        /// <summary>
        /// Index used to access the current sub-exercise in the current selection exercise. This 
        /// </summary>
        public static int CurrentSubExerciseIndex = 0;
    
    
        /// <summary>
        /// Path to HTML resources in unity project file structure. Used to display local html files in in-game
        /// browser plugin
        /// </summary>
        public static string LocalHtmlResourcePath = Application.dataPath + "/Resources/html/";
        
        /// <summary>
        /// Path to image resources in unity project file structure
        /// </summary>
        public static string ImageResourcePath = "img/";

        /// <summary>
        /// Path to init file resource in unity project file structure
        /// </summary>
        private const string InitFileResourcePath = "json/init/initFile";


        /// <summary>
        /// Initialization file parse tree root
        /// </summary>
        public static InitFileRoot InitFile { get; private set; }

        #endregion Public members
        
        #region Private members
        
        /// <summary>
        /// Static log4net logger
        /// </summary>
        //private static readonly ILog Log = LogManager.GetLogger(typeof(GlobalDataModel));
        
        #endregion Private members
        
        #region Public functions
        
        public static void InitializeData()
        {
            ParseIniFile();
            //Log.Debug("Global Data initialized");
        }
        
        #endregion Public functions
    
        #region Private functions
        
        /// <summary>
        /// Setup configuration of the log4net logging framework
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ConfigureLogging()
        {
            // var path = $"{Application.dataPath}/Resources/log/log4netConfig.xml";
            // var configFile = new FileInfo(path);
            //
            // // System.Diagnostics.Trace.Listeners.Add(
            // //     new TL()
            // // );
            //
            // //log4net.Util.LogLog.InternalDebugging = true;
            //
            // log4net.Config.XmlConfigurator.Configure(configFile);
            //
            // var messages = log4net.LogManager.GetRepository().ConfigurationMessages; //.Cast<log4net.Util.LogLog>();
            // foreach(
            //     log4net.Util.LogLog message in messages)
            // {
            //     // evaluate configuration message
            //     //Debug.Log(message.Message);
            // }
        }

        private static void ParseIniFile()
        {
            var json = Resources.Load(InitFileResourcePath, typeof(TextAsset) ) as TextAsset;

            if (json != null)
            {
                // var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // sphere.transform.position = Vector3.zero;
            }
            
            var errors = new List<string>();
            ITraceWriter tr = new MemoryTraceWriter
            {
                LevelFilter = TraceLevel.Off
            };
            var jsr = JsonConvert.DeserializeObject<InitFileRoot>(json.text,
                new JsonSerializerSettings()
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        Debug.Log(args.CurrentObject);
                        Debug.Log(args.ErrorContext.Path);
                        args.ErrorContext.Handled = true;
                    },
                    TraceWriter = tr
                }
            
            );
        
            InitFile = jsr;
        
            for (var i = 0; i < jsr.DisplayCurves.Count; i++)
            {
                var curve = jsr.DisplayCurves[i];
                var pd = DataImport.CreatePointDatasetFromCurve(curve);
                DisplayCurveDatasets.Add(pd);
            }

            if (jsr.Exercises.Count == 0) jsr.ApplicationSettings.TableSettings.ShowQuizButton = false;
            else jsr.ApplicationSettings.TableSettings.ShowQuizButton = true;

            for (var i = 0; i < jsr.Exercises.Count; i++)
            {
                var ex = jsr.Exercises[i];

                //if (ex is null) continue;
                //if (ex.Type is null) continue;
                
                // Select3 exercise
                if (ex.Type.Equals("select3"))
                {
                    var subExercises = new List<SelectionExerciseDataset>();
                    var correctAnswers = new List<SelectionExerciseAnswer>();
                    for (var j = 0; j < ex.SelectThreeExercises.Count; j++)
                    {
                        var subExercise = ex.SelectThreeExercises[j];
                        subExercises.Add(DataImport.CreateExercisePointDatasetFromSubExercise(subExercise));
                        correctAnswers.Add(new SelectionExerciseAnswer(subExercise.CorrectAnswer));
                    }
                
                    var selExercise = new SelectionExercise(
                        ex.Title,
                        ex.Description,
                        subExercises,
                        correctAnswers
                    );
                
                    SelectionExercises.Add(selExercise);
                }

                // tanNormSelect exercise
                if (ex.Type.Equals("tanNormSelect"))
                {
                    var subExerciseData = new List<TangentNormalExerciseDataset>();
                    var correctAnswers = new List<TangentNormalExerciseAnswer>();

                    for (var j = 0; j < ex.TangentNormalExercises.Count; j++)
                    {
                        var subExercise = ex.TangentNormalExercises[j];
                        subExerciseData.Add(DataImport.CreateTangentNormalDataFromSubExercise(subExercise));
                        correctAnswers.Add(new TangentNormalExerciseAnswer(subExercise.CorrectTangents, subExercise.CorrectNormals));
                    }

                    var tanNormExercise = new TangentNormalExercise(
                        ex.Title,
                        ex.Description,
                        subExerciseData,
                        correctAnswers
                    );

                    SelectionExercises.Add(tanNormExercise);
                }

                ExerciseCurveDatasets.Add(new CurveInformationDataset()
                {
                    Name = ex.Title,
                    DisplayString = ex.Title,
                });
            }

            //foreach (var ex in SelectionExercises)
            //{
            //    Debug.Log("exercise: " + ex.Title);
            //    foreach (var sub in ex.Datasets)
            //        Debug.Log("sub ex: " + sub.HeaderText);
            //}
        }
        
        #endregion Private functions
    }


}

// public class TL : System.Diagnostics.TraceListener
// {
//     public override void Write(string message)
//     {
//         Debug.Log(message);
//     }
//
//     public override void WriteLine(string message)
//     {
//         Debug.Log(message + System.Environment.NewLine);
//     }
// }
