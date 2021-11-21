using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Calculation;
using Calculation.NamedCurves;
using Calculation.ParameterExercises;
using Calculation.SelectionExercises;
using Controller;
using Controller.Curve;
using Controller.Exercise;
using Import;
using Import.NewInitFile;
using UnityEngine;

using log4net;
using Newtonsoft.Json.Serialization;
using UnityEngine.PlayerLoop;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

public static class GlobalData
{
    public static readonly ILog Log = LogManager.GetLogger(typeof(GlobalData));
    
    public static float PointScaleFactor = 1f;

    /// <summary>
    /// Factor to control speed of curve runs. The value specifies the time in seconds that has to pass before a
    /// new point is traveled to. A value of 1.0 means a new point is reached every second. 
    /// </summary>
    public static float RunSpeedFactor = 0.005f;

    public static CurveViewController WorldCurveViewController { get; set; }
    
    public static CurveViewController TableCurveViewController { get; set; }
    public static ExerciseCurveViewController ExerciseCurveController { get; set; }
    
    
    public static bool IsRunning { get; set; }

    public enum CurveDisplayGroup { Display = 0, Exercises = 1 }
    public static CurveDisplayGroup CurrentDisplayGroup { get; set; } = CurveDisplayGroup.Display;

    public static List<CurveInformationDataset> CurrentDataset
    {
        get
        {
            switch(CurrentDisplayGroup)
            {
                //CurveDisplayGroup.Parameter => ParamCurveDatasets,
                case CurveDisplayGroup.Exercises:
                    return ExerciseCurveDatasets;
                
                default:
                    return DisplayCurveDatasets;
            };
        }
    }
    

    public static List<CurveInformationDataset> DisplayCurveDatasets { get; set; } = new List<CurveInformationDataset>();
    public static List<CurveInformationDataset> ParamCurveDatasets = new List<CurveInformationDataset>();
    public static List<CurveInformationDataset> ExerciseCurveDatasets = new List<CurveInformationDataset>();

    

    private static List<AbstractCurveCalc> NamedDataset { get; set; } = new List<AbstractCurveCalc>()
    {
        new HelixCurveCalc(),
        new LogHelixCurveCalc(),
        new ArchimedeanSpiralCurveCalc(),
        new InvoluteCurveCalc(),
        new CardioidCurveCalc(),
        new LemniskateBernoulliCurveCalc(),
        new LemniskateGeronoCurveCalc(),
        new LimaconCurveCalc(),
        new CycloidCurveCalc(),
    };

    private static List<AbstractCurveCalc> LocalParamCalcList = new List<AbstractCurveCalc>()
    {
        new Param4aCurveCalc(),
        new Param4bCurveCalc(),
        new Param18CurveCalc(),
        new Param41CurveCalc(),
        new Param56CurveCalc(),
        new Param57CurveCalc(),
        new Param58CurveCalc(),
        new Param59CurveCalc(),
        new Param60CurveCalc(),
        new Param61CurveCalc(),
        new Param62CurveCalc(),
    };

    // private static List<AbstractCurveCalc> LocalExerciseCalcList = new List<AbstractCurveCalc>(NamedDataset)
    // {
    //     new TestExercise01ACurveClass(),
    //     new TestExercise01BCurveClass(),
    //     new TestExercise01CCurveClass(),
    //     new TestExercise01DCurveClass(),
    //     new TestExercise01ECurveCalc(),
    //     new TestExercise01FCurveCalc()
    // };


    //public static CurveSelectionStateContext CurveSelectionFSM;

    public static int CurrentCurveIndex = 0;
    //public static int CurrentPointIndex { get; set; };

    public static List<SelectionExercise> SelectionExercises = new List<SelectionExercise>();

    public static int CurrentExerciseIndex = 0;
    public static int CurrentSubExerciseIndex = 0;
    
    
    public static string LocalHTMLResourcePath = Application.dataPath + "/Resources/html/";
    public static string ImageResourcePath = "img/";

    public static InitFileRoot initFile { get; set; }

    public static void InitializeData()
    {
        //Log.Debug("testOutputLogger");
        
        ParseIniFile();
        
        ImportAllResources();
        
        
        Log.Debug("Global Data initialized");
    }
    
    
    
    
    
    
    
    
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ConfigureLogging()
    {
        string path = $"{Application.dataPath}/Resources/log/log4netConfig.xml";
        Debug.Log("configPath: " + path);
        var configFile = new FileInfo(path);
        
        log4net.Config.XmlConfigurator.Configure(configFile);
        
        //Debug.Log("log config loaded: " + LogManager.GetRepository().Configured);
    }



    private static void ImportAllResources()
    {
        // // CSV Resources
        // Object[] csv_resources = Resources.LoadAll("csv/exercises/", typeof(TextAsset));
        //
        // for (int i = 0; i < csv_resources.Length; i++)
        // {
        //     TextAsset csvRes = csv_resources[i] as TextAsset;
        //     //NamedCurveDatasets.Add(DataImport.ImportPointsFromCSVResource(csvRes));
        // }
        //
        // // JSON Resources
        // Object[] json_resources = Resources.LoadAll("json/exercises/", typeof(TextAsset));
        //
        // for (int i = 0; i < json_resources.Length; i++)
        // {
        //     //NamedCurveDatasets.Add(DataImport.ImportFromJSONResource(json_resources[i] as TextAsset));
        // }
        //
        // var json_named_curves = Resources.LoadAll("json/curves/named/", typeof(TextAsset) ).Cast<TextAsset>();
        //
        // foreach (var named_curve in json_named_curves)
        // {
        //     //Debug.Log("Attempting to import named curve: " + (named_curve).name);
        //     NamedCurveDatasets.Add(DataImport.ImportPointsFromJSONResource(named_curve));
        // }
        //
        // var jsonParamCurves = Resources.LoadAll("json/curves/param/", typeof(TextAsset) ).Cast<TextAsset>();
        // foreach (var paramCurve in jsonParamCurves)
        // {
        //     //Debug.Log("Attempting to import param curve: " + paramCurve.name);
        //     ParamCurveDatasets.Add(DataImport.ImportPointsFromJSONResource(paramCurve));
        // }
        //
        //
        // // Local calculation
        // for (int i = 0; i < NamedDataset.Count; i++)
        // {
        //     AbstractCurveCalc calc = NamedDataset[i];
        //     //NamedCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        // }
        //
        // for (int i = 0; i < LocalParamCalcList.Count; i++)
        // {
        //     AbstractCurveCalc calc = LocalParamCalcList[i];
        //     //ParamCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        // }
        //
        // for (int i = 0; i < LocalExerciseCalcList.Count; i++)
        // {
        //     AbstractCurveCalc calc = LocalExerciseCalcList[i];
        //     //ExerciseCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        // }
        //
        // // Load test exercise
        // Object res = Resources.Load("json/exercises/testExercise01", typeof(TextAsset));
        // SelectionExercise selExerc = DataImport.ImportExerciseFromJSONResource(res as TextAsset);
        // SelectionExercises.Add((selExerc));
        
        
        // Import everything from single json 
        
        
        
        
        //ExerciseCurveDatasets.Add(selExerc);
    }


    private static void ParseIniFile()
    {
        // JSON Resources
        // ITraceWriter tr = new MemoryTraceWriter();
        // TextAsset json = Resources.Load("json/init/initTemplate", typeof(TextAsset)) as TextAsset;
        // initFile = JsonConvert.DeserializeObject<InitFileJsonRoot>(json.text,
        //     new JsonSerializerSettings()
        //     {
        //         // Error = delegate(object sender, ErrorEventArgs args)
        //         // {
        //         //     errors.Add(args.ErrorContext.Error.Message);
        //         //     Debug.Log("ErrorOccuredJSON");
        //         //     args.ErrorContext.Handled = true;
        //         // },
        //         //FloatParseHandling = FloatParseHandling.Double
        //         TraceWriter = tr
        //         //,
        //         //Converters = { new IsoDateTimeConverter()}
        //     }
        //     );
        
        //Debug.Log("initFile null: " + (initFile is null));

        //Debug.Log(tr);
     
        TextAsset json = Resources.Load("json/init/initv2", typeof(TextAsset) ) as TextAsset;
        
        
        List<string> errors = new List<string>();
        ITraceWriter tr = new MemoryTraceWriter();
        InitFileRoot jsr = JsonConvert.DeserializeObject<InitFileRoot>(json.text,
            new JsonSerializerSettings()
            {
                Error = delegate(object sender, ErrorEventArgs args)
                {
                    errors.Add(args.ErrorContext.Error.Message);
                    Debug.Log("ErrorOccuredJSON");
                    args.ErrorContext.Handled = true;
                },
                //FloatParseHandling = FloatParseHandling.Double
                TraceWriter = tr
                //,
                //Converters = { new IsoDateTimeConverter()}
            }
            
        );
        
        initFile = jsr;
        
        //Debug.Log(tr);

        //var path = "C:\\Users\\saerota\\Desktop\\newtonLog.txt";
        //File.WriteAllText(path, tr.ToString());
        
        
        for (int i = 0; i < jsr.DisplayCurves.Count; i++)
        {
            var curve = jsr.DisplayCurves[i];
            DisplayCurveDatasets.Add(DataImport.CreatePointDatasetFromCurve(curve));
        }

        for (int i = 0; i < jsr.Exercises.Count; i++)
        {
            var ex = jsr.Exercises[i];

            // Select3 exercise
            if (ex.Type.Equals("select3"))
            {
                List<ExercisePointDataset> subexercises = new List<ExercisePointDataset>();
                List<int> correctAnswers = new List<int>();
                for (int j = 0; j < ex.SubExercises.Count; j++)
                {
                    var subExercise = ex.SubExercises[j];
                    subexercises.Add(DataImport.CreateExercisePointDatasetFromSubExercise(subExercise));
                    correctAnswers.Add(subExercise.CorrectAnswer);
                }
                
                SelectionExercise selExerc = new SelectionExercise(
                    ex.Title,
                    ex.Description,
                    subexercises,
                    correctAnswers
                    );
                
                SelectionExercises.Add(selExerc);
            }

            
            ExerciseCurveDatasets.Add(new CurveInformationDataset()
            {
                Name = ex.Title,
                DisplayString = ex.Title,
            });
                
            // SelectionExercises.Add((selExerc));
        }
        
    }
    
}
