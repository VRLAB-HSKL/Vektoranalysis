using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static float PointScaleFactor = 1f;
    public static float RunSpeedFactor = 1f;

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

    private static List<AbstractCurveCalc> NamedDataset = new List<AbstractCurveCalc>()
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
    //{
    //    new HelixCurveCalc(),
    //}
    ;


    //public static CurveSelectionStateContext CurveSelectionFSM;

    public static int CurrentCurveIndex = 0;
    public static int CurrentPointIndex = 0;

    public static string LocalHTMLResourcePath = Application.dataPath + "/Resources/html/";
    public static string ImageResourcePath = "img/";

    public static InitFileJsonRoot initFile;

    public static void InitializeData()
    {
        ImportAllResources();
        ParseIniFile();
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

        // Local calculation
        for (int i = 0; i < NamedDataset.Count; i++)
        {
            AbstractCurveCalc calc = NamedDataset[i];
            NamedCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        }

        for (int i = 0; i < LocalParamCalcList.Count; i++)
        {
            AbstractCurveCalc calc = LocalParamCalcList[i];
            ParamCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        }

        for (int i = 0; i < LocalExerciseCalcList.Count; i++)
        {
            AbstractCurveCalc calc = LocalExerciseCalcList[i];
            ExerciseCurveDatasets.Add(DataImport.CreateDatasetFormLocalCalculation(calc));
        }

    }


    private static void ParseIniFile()
    {
        // JSON Resources
        TextAsset json = Resources.Load("json/init/initTemplate.json", typeof(TextAsset)) as TextAsset;
        //initFile = JsonConvert.DeserializeObject<InitFileJsonRoot>(json.text);
    }





}
