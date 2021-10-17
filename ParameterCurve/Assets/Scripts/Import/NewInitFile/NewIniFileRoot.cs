using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Import.NewInitFile
{
    [Serializable]
    public class InitFileRoot
    {
        [JsonProperty("displayCurves")]
        public List<Curve> DisplayCurves { get; set; }
        
        [JsonProperty("exercises")]
        public List<Exercise> Exercises { get; set; }
        
        [JsonProperty("applicationSettings")]
        public ApplicationSettings ApplicationSettings { get; set; }
    }

    #region General
    
    [Serializable]
    public class PointDataNew
    {
        [JsonProperty("t")]
        public List<float> T { get; set; }
        
        [JsonProperty("pVec")]
        public List<List<float>> PVec { get; set; }
        
        [JsonProperty("velVec")]
        public List<List<float>> VelVec { get; set; }
        
        [JsonProperty("accVec")]
        public List<List<float>> AccVec { get; set; }
        
        [JsonProperty("arcT")]
        public List<float> ArcT { get; set; }
        
        [JsonProperty("arcPVec")]
        public List<List<float>> ArcPVec { get; set; }
        
        [JsonProperty("arcVelVec")]
        public List<List<float>> ArcVelVec { get; set; }
        
        [JsonProperty("arcAccVec")]
        public List<List<float>> ArcAccVec { get; set; }
    }

    [Serializable]
    public class RGBColor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("rgba")]
        public List<byte> Rgba { get; set; }
        
        [JsonProperty("hex")]
        public string Hex { get; set; }
    }

    
    #endregion General

    
    
    #region DisplayCurves
    
    [Serializable]
    public class Curve
    {
        //public InfoSettings InfoSettings { get; set; }

        [JsonProperty("info")]
        public CurveInfo Info { get; set; }
        
        [JsonProperty("data")]
        public CurveData Data { get; set; }
        
        [JsonProperty("settings")]
        public CurveSettings CurveSettings { get; set; }
    }

    [Serializable]
    public class CurveInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    
    [Serializable]
    public class CurveData
    {
        // [JsonProperty("name")]
        // public string Name { get; set; }
        
        [JsonProperty("dim")]
        public int Dimension { get; set; }
        
        [JsonProperty("arcLength")]
        public float ArcLength { get; set; }
        
        [JsonProperty("worldScalingFactor")]
        public float WorldScalingFactor { get; set; }
        
        [JsonProperty("tableScalingFactor")]
        public float TableScalingFactor { get; set; }
        
        [JsonProperty("selectExercisePillarScalingFactor")]
        public float SelectExercisePillarScalingFactor { get; set; }
        
        [JsonProperty("data")]
        public PointDataNew Data { get; set; }
    }
    
    [Serializable]
    public class CurveSettings
    {
        [JsonProperty("display")]
        public DisplaySettings DisplaySettings { get; set; }
    }
    
    
    [Serializable]
    public class DisplaySettings
    {
        [JsonProperty("view")]
        public string View { get; set; }
        
        [JsonProperty("lineColor")]
        public RGBColor LineColor { get; set; }
        
        [JsonProperty("travelObj")]
        public string TravelObjStr { get; set; }
        
        [JsonProperty("travelObjColor")]
        public RGBColor TravelObjColor { get; set; }
        
        [JsonProperty("arcTravelObj")]
        public string ArcTravelObjStr { get; set; }
        
        [JsonProperty("arcTravelObjColor")]
        public RGBColor ArcTravelObjColor { get; set; }
    }
    
    
    #endregion DisplayCurves
    
    
    #region Exercises
    
    [Serializable]
    public class Exercise
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("subExercises")]
        public List<SubExercise> SubExercises { get; set; }
    }
    
    [Serializable]
    public class SubExercise
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("correctAnswer")]
        public int CorrectAnswer { get; set; }
        
        [JsonProperty("leftCurve")]
        public Curve LeftCurve { get; set; }
        
        [JsonProperty("middleCurve")]
        public Curve MiddleCurve { get; set; }
        
        [JsonProperty("rightCurve")]
        public Curve RightCurve { get; set; }
    }
    
    
    #endregion

    #region ApplicationSettings
    
    [Serializable]
    public class ApplicationSettings
    {
        [JsonProperty("browser")]
        public BrowserSettings BrowserSettings { get; set; }
        
        [JsonProperty("info")]
        public InfoSettings InfoSettings { get; set; }
        
        [JsonProperty("selectMenu")]
        public SelectMenuSettings SelectMenuSettings { get; set; }
        
        [JsonProperty("table")]
        public TableSettings TableSettings { get; set; }
    }
    
    [Serializable]
    public class BrowserSettings
    {
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    
    [Serializable]
    public class InfoSettings
    {
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        [JsonProperty("showBasicInfo")]
        public bool ShowBasicInfo { get; set; }
        
        [JsonProperty("showPointData")]
        public bool ShowPointData { get; set; }
        
        [JsonProperty("showArcLengthData")]
        public bool ShowArcLengthData { get; set; }
        
        [JsonProperty("showTimeDistancePlot")]
        public bool ShowTimeDistancePlot { get; set; }
        
        [JsonProperty("showTimeVelocityPlot")]
        public bool ShowTimeVelocityPlot { get; set; }
    }
    
    [Serializable]
    public class SelectMenuSettings
    {
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        [JsonProperty("showDisplayCurves")]
        public bool ShowDisplayCurves { get; set; }
        
        [JsonProperty("showExercises")]
        public bool ShowExercises { get; set; }
    }
    
    
    [Serializable]
    public class TableSettings
    {
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        [JsonProperty("showNavButtons")]
        public bool ShowNavButtons { get; set; }
        
        [JsonProperty("showRunButton")]
        public bool ShowRunButton { get; set; }
        
        [JsonProperty("showViewButtons")]
        public bool ShowViewButtons { get; set; }
        
        [JsonProperty("showSlider")]
        public bool ShowSlider { get; set; }
        
        [JsonProperty("allowRangeModify")]
        public bool AllowRangeModify { get; set; }
        
        [JsonProperty("allowBookmarks")]
        public bool AllowBookmarks { get; set; }
    }
    
    #endregion ApplicationSettings
    
}