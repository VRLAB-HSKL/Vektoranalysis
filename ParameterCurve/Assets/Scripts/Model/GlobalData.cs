using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Calculation.SelectionExercises;
using Controller;
using UnityEngine;

using log4net;
using Newtonsoft.Json.Serialization;

public static class GlobalData
{
    public static readonly ILog Log = LogManager.GetLogger(typeof(GlobalData));
    
    
    public static float PointScaleFactor = 1f;
    public static float RunSpeedFactor = 1f;

    public static ExerciseViewController exerciseController { get; set; }
    
    
    public static bool IsDriving = false;

    public enum CurveDisplayGroup { Named = 0, Parameter = 1, Exercises = 2 }
    public static CurveDisplayGroup CurrentDisplayGroup = CurveDisplayGroup.Named;

    public static List<PointDataset> CurrentDataset
    {
        get
        {
            return CurrentDisplayGroup switch
            {
                CurveDisplayGroup.Parameter => ParamCurveDatasets,
                CurveDisplayGroup.Exercises => ExerciseCurveDatasets,
                _ => NamedCurveDatasets,
            };
        }
    }

    public static List<PointDataset> NamedCurveDatasets = new List<PointDataset>();
    public static List<PointDataset> ParamCurveDatasets = new List<PointDataset>();
    public static List<PointDataset> ExerciseCurveDatasets = new List<PointDataset>();

    public static List<SelectionExercise> SelectionExercises = new List<SelectionExercise>();

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

    private static List<AbstractCurveCalc> LocalExerciseCalcList = new List<AbstractCurveCalc>(NamedDataset)
    {
        new TestExercise01ACurveClass(),
        new TestExercise01BCurveClass(),
        new TestExercise01CCurveClass(),
        new TestExercise01DCurveClass(),
        new TestExercise01ECurveCalc(),
        new TestExercise01FCurveCalc()
    };


    //public static CurveSelectionStateContext CurveSelectionFSM;

    public static int CurrentCurveIndex = 0;
    public static int CurrentPointIndex = 0;

    public static string LocalHTMLResourcePath = Application.dataPath + "/Resources/html/";
    public static string ImageResourcePath = "img/";

    public static InitFileJsonRoot initFile { get; set; }

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
        // CSV Resources
        Object[] csv_resources = Resources.LoadAll("csv/exercises/", typeof(TextAsset));

        for (int i = 0; i < csv_resources.Length; i++)
        {
            TextAsset csvRes = csv_resources[i] as TextAsset;
            //NamedCurveDatasets.Add(DataImport.ImportPointsFromCSVResource(csvRes));
        }

        // JSON Resources
        Object[] json_resources = Resources.LoadAll("json/exercises/", typeof(TextAsset));

        for (int i = 0; i < json_resources.Length; i++)
        {
            //NamedCurveDatasets.Add(DataImport.ImportFromJSONResource(json_resources[i] as TextAsset));
        }
        
        var json_named_curves = Resources.LoadAll("json/curves/named/", typeof(TextAsset) ).Cast<TextAsset>();
        
        foreach (var named_curve in json_named_curves)
        {
            //Debug.Log("Attempting to import named curve: " + (named_curve).name);
            NamedCurveDatasets.Add(DataImport.ImportPointsFromJSONResource(named_curve));
        }
        
        var jsonParamCurves = Resources.LoadAll("json/curves/param/", typeof(TextAsset) ).Cast<TextAsset>();
        foreach (var paramCurve in jsonParamCurves)
        {
            //Debug.Log("Attempting to import param curve: " + paramCurve.name);
            ParamCurveDatasets.Add(DataImport.ImportPointsFromJSONResource(paramCurve));
        }
        

        // Local calculation
        for (int i = 0; i < NamedDataset.Count; i++)
        {
            AbstractCurveCalc calc = NamedDataset[i];
            //NamedCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        }

        for (int i = 0; i < LocalParamCalcList.Count; i++)
        {
            AbstractCurveCalc calc = LocalParamCalcList[i];
            //ParamCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        }

        for (int i = 0; i < LocalExerciseCalcList.Count; i++)
        {
            AbstractCurveCalc calc = LocalExerciseCalcList[i];
            //ExerciseCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        }
        
        // Load test exercise
        Object res = Resources.Load("json/exercises/testExercise01", typeof(TextAsset));
        SelectionExercise selExerc = DataImport.ImportExerciseFromJSONResource(res as TextAsset);
        SelectionExercises.Add((selExerc));
        
        
        //ExerciseCurveDatasets.Add(selExerc);
    }


    private static void ParseIniFile()
    {
        // JSON Resources
        ITraceWriter tr = new MemoryTraceWriter();
        TextAsset json = Resources.Load("json/init/initTemplate", typeof(TextAsset)) as TextAsset;
        initFile = JsonConvert.DeserializeObject<InitFileJsonRoot>(json.text,
            new JsonSerializerSettings()
            {
                // Error = delegate(object sender, ErrorEventArgs args)
                // {
                //     errors.Add(args.ErrorContext.Error.Message);
                //     Debug.Log("ErrorOccuredJSON");
                //     args.ErrorContext.Handled = true;
                // },
                //FloatParseHandling = FloatParseHandling.Double
                TraceWriter = tr
                //,
                //Converters = { new IsoDateTimeConverter()}
            }
            );
        
        //Debug.Log("initFile null: " + (initFile is null));

        Debug.Log(tr);
        
    }





}
