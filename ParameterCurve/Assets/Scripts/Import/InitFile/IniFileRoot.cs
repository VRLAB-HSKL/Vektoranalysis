using System;
using System.Collections.Generic;
using log4net.Appender;
using Model;
using Newtonsoft.Json;

namespace Import.InitFile
{
    /// <summary>
    /// Root of the tree like structure the information of the init file is parsed into
    /// <see cref="GlobalData.InitFile"/>
    /// </summary>
    [Serializable]
    public class InitFileRoot
    {
        /// <summary>
        /// Collection of display curves
        /// </summary>
        [JsonProperty("displayCurves")]
        public List<Curve> DisplayCurves { get; set; }
        
        /// <summary>
        /// Collection of exercises
        /// </summary>
        [JsonProperty("exercises")]
        public List<Exercise> Exercises { get; set; }
        
        /// <summary>
        /// General application settings
        /// </summary>
        [JsonProperty("applicationSettings")]
        public ApplicationSettings ApplicationSettings { get; set; }
    }

    #region General
    
    /// <summary>
    /// Node class for geometric curve data
    /// </summary>
    [Serializable]
    public class PointData
    {
        /// <summary>
        /// Parameter values
        /// </summary>
        [JsonProperty("t")]
        public List<float> T { get; set; }
        
        /// <summary>
        /// Point vectors
        /// </summary>
        [JsonProperty("point_vec")]
        public List<List<float>> PVec { get; set; }
        
        /// <summary>
        /// Velocity vectors
        /// </summary>
        [JsonProperty("vel_vec")]
        public List<List<float>> VelVec { get; set; }
        
        /// <summary>
        /// Acceleration vectors
        /// </summary>
        [JsonProperty("acc_vec")]
        public List<List<float>> AccVec { get; set; }
        
        /// <summary>
        /// Arc length parametrization based parameter values
        /// </summary>
        [JsonProperty("arc_t")]
        public List<float> ArcT { get; set; }
        
        /// <summary>
        /// Arc length parametrization based point vectors
        /// </summary>
        [JsonProperty("arc_point_vec")]
        public List<List<float>> ArcPVec { get; set; }
        
        /// <summary>
        /// Arc length parametrization based velocity vectors
        /// </summary>
        [JsonProperty("arc_vel_vec")]
        public List<List<float>> ArcVelVec { get; set; }
        
        /// <summary>
        /// Arc length parametrization based acceleration vectors
        /// </summary>
        [JsonProperty("arc_acc_vec")]
        public List<List<float>> ArcAccVec { get; set; }
    }

    /// <summary>
    /// Node class for general color values
    /// </summary>
    [Serializable]
    public class RGBColor
    {
        /// <summary>
        /// Name of the color, to be used for static color constants <see>
        ///     <cref>System.Windows.Media.Colors</cref>
        /// </see>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Float array representing color values
        /// [0]: red
        /// [1]: green
        /// [2]: blue
        /// [3]: alpha
        /// </summary>
        [JsonProperty("rgba")]
        public List<float> Rgba { get; set; }
        
        /// <summary>
        /// Hexadecimal color code
        /// </summary>
        [JsonProperty("hex")]
        public string Hex { get; set; }
    }

    #endregion General
    
    #region DisplayCurves
    
    /// <summary>
    /// Node class representing a single curve
    /// </summary>
    [Serializable]
    public class Curve
    {
        //public InfoSettings InfoSettings { get; set; }

        /// <summary>
        /// Curve information
        /// </summary>
        [JsonProperty("info")]
        public CurveInfo Info { get; set; }
        
        /// <summary>
        /// Curve data, i.e. geometric data
        /// </summary>
        [JsonProperty("data")]
        public CurveData Data { get; set; }
        
        /// <summary>
        /// Custom curve local settings
        /// </summary>
        [JsonProperty("settings")]
        public CurveSettings CurveSettings { get; set; }
    }

    /// <summary>
    /// Node class for general curve information
    /// </summary>
    [Serializable]
    public class CurveInfo
    {
        /// <summary>
        /// Unique curve identifier
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        
        /// <summary>
        /// Curve name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Display string, used in GUI
        /// </summary>
        [JsonProperty("display_text")]
        public string DisplayText { get; set; }
    }
    
    /// <summary>
    /// Node glass for curve data
    /// </summary>
    [Serializable]
    public class CurveData
    {
        // [JsonProperty("name")]
        // public string Name { get; set; }
        
        /// <summary>
        /// Dimension of curve, only 2 or 3
        /// </summary>
        [JsonProperty("dim")]
        public int Dimension { get; set; }
        
        /// <summary>
        /// Length of the curve
        /// </summary>
        [JsonProperty("arcLength")]
        public float ArcLength { get; set; }
        
        /// <summary>
        /// Scaling factor, used to scale the curve on the world display
        /// </summary>
        [JsonProperty("worldScalingFactor")]
        public float WorldScalingFactor { get; set; }
        
        /// <summary>
        /// Scaling factor, used to scale the curve on the examination table
        /// </summary>
        [JsonProperty("tableScalingFactor")]
        public float TableScalingFactor { get; set; }
        
        /// <summary>
        /// Scaling factor, used to scale the curve inside pillars in selection exercises
        /// </summary>
        [JsonProperty("selectExercisePillarScalingFactor")]
        public float SelectExercisePillarScalingFactor { get; set; }
        
        /// <summary>
        /// Geometric point data, i.e. point vectors, velocity vectors, ...
        /// </summary>
        [JsonProperty("data")]
        public PointData Data { get; set; }
    }
    
    /// <summary>
    /// Node class for curve local settings
    /// </summary>
    [Serializable]
    public class CurveSettings
    {
        /// <summary>
        /// Settings related to the ingame display of the curve
        /// </summary>
        [JsonProperty("display")]
        public DisplaySettings DisplaySettings { get; set; }
    }
    
    
    /// <summary>
    /// Node class for settings related to the ingame display of curves
    /// </summary>
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
    
    /// <summary>
    /// Node class representing an exercise
    /// </summary>
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
    
    /// <summary>
    /// Node class representing a sub-exercise of a given exercise
    /// </summary>
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
    
    /// <summary>
    /// Node class for general, curve-independent application settings
    /// </summary>
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
    
    /// <summary>
    /// Node class for settings related to the ingame browser application
    /// </summary>
    [Serializable]
    public class BrowserSettings
    {
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    
    /// <summary>
    /// Node class for settings related to the ingame information wall
    /// </summary>
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
    
    /// <summary>
    /// Node class for settings related to the ingame selection menu
    /// </summary>
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
    
    
    /// <summary>
    /// Node class for settings related to the examination table
    /// </summary>
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