using System;
using System.Collections.Generic;
using Model;
using Newtonsoft.Json;

namespace Import.InitFile
{
    /// <summary>
    /// Root of the tree like structure the information of the init file is parsed into
    /// <see cref="GlobalDataModel.InitFile"/>
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
        /// Normal vectors
        /// </summary>
        [JsonProperty("norm_vec")]
        public List<List<float>> NormVec { get; set; }
        
        /// <summary>
        /// Binormal vectors
        /// </summary>
        [JsonProperty("binorm_vec")]
        public List<List<float>> BinormVec { get; set; }
        
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
        
        
        [JsonProperty("arc_norm_vec")]
        public List<List<float>> ArcNormVec { get; set; }
        
        [JsonProperty("arc_binorm_vec")]
        public List<List<float>> ArcBinormVec { get; set; }
        
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
        [JsonProperty("arc_length")]
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
        /// Settings related to the in-game display of the curve
        /// </summary>
        [JsonProperty("display")]
        public DisplaySettings DisplaySettings { get; set; }
    }
    
    
    /// <summary>
    /// Node class for settings related to the in-game display of curves
    /// </summary>
    [Serializable]
    public class DisplaySettings
    {
        /// <summary>
        /// View on the curve
        /// - view
        /// - run
        /// - arc
        /// </summary>
        [JsonProperty("view")]
        public string View { get; set; }
        
        /// <summary>
        /// Curve line color
        /// </summary>
        [JsonProperty("lineColor")]
        public RGBColor LineColor { get; set; }
        
        /// <summary>
        /// Model for object representing run travel object
        /// </summary>
        [JsonProperty("travelObj")]
        public string TravelObjStr { get; set; }
        
        /// <summary>
        /// Color of object representing run travel object
        /// </summary>
        [JsonProperty("travelObjColor")]
        public RGBColor TravelObjColor { get; set; }
        
        /// <summary>
        /// Model for object representing arc travel object
        /// </summary>
        [JsonProperty("arcTravelObj")]
        public string ArcTravelObjStr { get; set; }
        
        /// <summary>
        /// Color of object representing arc travel object
        /// </summary>
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
        /// <summary>
        /// Unique identifier
        /// </summary>
        [JsonProperty("identifier")]
        public int Id { get; set; }
        
        /// <summary>
        /// Exercise title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        
        /// <summary>
        /// Type of exercise
        /// - select3
        /// </summary>
        [JsonProperty("exercise_type")]
        public string Type { get; set; }
        
        /// <summary>
        /// Exercise description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        ///// <summary>
        ///// Collection of sub-exercises
        ///// </summary>
        //[JsonProperty("sub_exercises")]
        //public List<SubExercise> SubExercises { get; set; }

        /// <summary>
        /// Collection of select3 exercises for this exercise
        /// </summary>
        [JsonProperty("select_three_exercises")]
        public List<SelectThree> selectThreeExercises { get; set; }

        /// <summary>
        /// Collection of select3 exercises for this exercise
        /// </summary>
        [JsonProperty("tangent_normal_exercises")]
        public List<TangentNormal> tangentNormalExercises { get; set; }
    }

    /// <summary>
    /// Node class representing the type of exercise "Select 3"
    /// </summary>
    [Serializable]
    public class SelectThree
    {
        /// <summary>
        /// Sub-exercise title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Sub-exercise description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Correct answer of sub-exercise
        /// </summary>
        [JsonProperty("correctAnswer")]
        public int CorrectAnswer { get; set; }

        /// <summary>
        /// Curve designated to the left pillar
        /// </summary>
        [JsonProperty("leftCurve")]
        public Curve LeftCurve { get; set; }

        /// <summary>
        /// Curve designated to the middle pillar
        /// </summary>
        [JsonProperty("middleCurve")]
        public Curve MiddleCurve { get; set; }

        /// <summary>
        /// Curve designated to the right pillar
        /// </summary>
        [JsonProperty("rightCurve")]
        public Curve RightCurve { get; set; }
    }

    /// <summary>
    /// Node class representing the type of exercise "Tangent Normal"
    /// </summary>
    [Serializable]
    public class TangentNormal
    {
        /// <summary>
        /// Sub-exercise title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Sub-exercise description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Curve data for tangent normal drawing curve
        /// </summary>
        [JsonProperty("curve")]
        public Curve TangentNormalCurve { get; set; }

        /// <summary>
        /// Index collection of points on the tangent normal drawing curve
        /// </summary>
        [JsonProperty("highlight_points")]
        public List<int> HighlightPoints { get; set; }

        /// <summary>
        /// Collection of correct tangents as float arrays (float[2])
        /// </summary>
        [JsonProperty("correct_tangents")]
        public List<float[]> CorrectTangents { get; set; }

        /// <summary>
        /// Collection of correct normals as float arrays (float[2])
        /// </summary>
        [JsonProperty("correct_normals")]
        public List<float[]> CorrectNormals { get; set; }
    }

    ///// <summary>
    ///// Node class representing a sub-exercise of a given exercise
    ///// </summary>
    //[Serializable]
    //public class SubExercise
    //{
    //    /// <summary>
    //    /// Sub-exercise title
    //    /// </summary>
    //    [JsonProperty("title")]
    //    public string Title { get; set; }

    //    /// <summary>
    //    /// Sub-exercise description
    //    /// </summary>
    //    [JsonProperty("description")]
    //    public string Description { get; set; }

    //    /// <summary>
    //    /// Correct answer of sub-exercise
    //    /// </summary>
    //    [JsonProperty("correctAnswer")]
    //    public int CorrectAnswer { get; set; }

    //    /// <summary>
    //    /// Curve designated to the left pillar
    //    /// </summary>
    //    [JsonProperty("leftCurve")]
    //    public Curve LeftCurve { get; set; }

    //    /// <summary>
    //    /// Curve designated to the middle pillar
    //    /// </summary>
    //    [JsonProperty("middleCurve")]
    //    public Curve MiddleCurve { get; set; }

    //    /// <summary>
    //    /// Curve designated to the right pillar
    //    /// </summary>
    //    [JsonProperty("rightCurve")]
    //    public Curve RightCurve { get; set; }

    //    /// <summary>
    //    /// Curve data for tangent normal drawing curve
    //    /// </summary>
    //    [JsonProperty("curve")]
    //    public Curve TangentNormalCurve { get; set; }

    //    /// <summary>
    //    /// Index collection of points on the tangent normal drawing curve
    //    /// </summary>
    //    [JsonProperty("highlight_points")]
    //    public List<int> HighlightPoints { get; set; }

    //    /// <summary>
    //    /// Collection of correct tangents as float arrays (float[2])
    //    /// </summary>
    //    [JsonProperty("correct_tangents")]
    //    public List<float[]> CorrectTangents { get; set; }

    //    /// <summary>
    //    /// Collection of correct normals as float arrays (float[2])
    //    /// </summary>
    //    [JsonProperty("correct_normals")]
    //    public List<float[]> CorrectNormals { get; set; }
    //}

    #endregion

    #region ApplicationSettings

    /// <summary>
    /// Node class for general, curve-independent application settings
    /// </summary>
    [Serializable]
    public class ApplicationSettings
    {
        /// <summary>
        /// Browser wall settings
        /// </summary>
        [JsonProperty("browser")]
        public BrowserSettings BrowserSettings { get; set; }
        
        /// <summary>
        /// Information wall settings
        /// </summary>
        [JsonProperty("info")]
        public InfoSettings InfoSettings { get; set; }
        
        /// <summary>
        /// Curve selection wall settings
        /// </summary>
        [JsonProperty("selectMenu")]
        public SelectMenuSettings SelectMenuSettings { get; set; }
        
        /// <summary>
        /// In-game table settings
        /// </summary>
        [JsonProperty("table")]
        public TableSettings TableSettings { get; set; }
    }
    
    /// <summary>
    /// Node class for settings related to the in-game browser application
    /// </summary>
    [Serializable]
    public class BrowserSettings
    {
        /// <summary>
        /// Signals whether wall is activated in the scene
        /// </summary>
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        /// <summary>
        /// Initial url displayed by the browser wall
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    
    /// <summary>
    /// Node class for settings related to the in-game information wall
    /// </summary>
    [Serializable]
    public class InfoSettings
    {
        /// <summary>
        /// Signals whether wall is activated in scene
        /// </summary>
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        /// <summary>
        /// Signals whether basic curve information is shown
        /// </summary>
        [JsonProperty("showBasicInfo")]
        public bool ShowBasicInfo { get; set; }
        
        /// <summary>
        /// Signals whether point information is shown
        /// </summary>
        [JsonProperty("showPointData")]
        public bool ShowPointData { get; set; }
        
        /// <summary>
        /// Signals whether arc length parametrization information is shown
        /// </summary>
        [JsonProperty("showArcLengthData")]
        public bool ShowArcLengthData { get; set; }
        
        /// <summary>
        /// Signals whether time distance plot is displayed
        /// </summary>
        [JsonProperty("showTimeDistancePlot")]
        public bool ShowTimeDistancePlot { get; set; }
        
        /// <summary>
        /// Signals whether time velocity plot is displayed
        /// </summary>
        [JsonProperty("showTimeVelocityPlot")]
        public bool ShowTimeVelocityPlot { get; set; }
    }
    
    /// <summary>
    /// Node class for settings related to the in-game selection menu
    /// </summary>
    [Serializable]
    public class SelectMenuSettings
    {
        /// <summary>
        /// Signals whether wall is activated in scene
        /// </summary>
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        /// <summary>
        /// Signals whether display curves can be selected in menu
        /// </summary>
        [JsonProperty("showDisplayCurves")]
        public bool ShowDisplayCurves { get; set; }
        
        /// <summary>
        /// Signals whether exercises can be selected in menu
        /// </summary>
        [JsonProperty("showExercises")]
        public bool ShowExercises { get; set; }
    }
    
    
    /// <summary>
    /// Node class for settings related to the examination table
    /// </summary>
    [Serializable]
    public class TableSettings
    {
        /// <summary>
        /// Signals whether table is activated in the scene
        /// </summary>
        [JsonProperty("activated")]
        public bool Activated { get; set; }
        
        /// <summary>
        /// Signals whether navigation buttons are shown
        /// </summary>
        [JsonProperty("showNavButtons")]
        public bool ShowNavButtons { get; set; }
        
        /// <summary>
        /// Signals whether the run button is shown
        /// </summary>
        [JsonProperty("showRunButton")]
        public bool ShowRunButton { get; set; }
        
        /// <summary>
        /// Signals whether view buttons are shown
        /// </summary>
        [JsonProperty("showViewButtons")]
        public bool ShowViewButtons { get; set; }

        /// <summary>
        /// Signals whether quiz button is shown
        /// </summary>
        [JsonProperty("showQuizButton")]
        public bool ShowQuizButton { get; set; }

        /// <summary>
        /// Signals whether slider is shown
        /// </summary>
        [JsonProperty("showSlider")]
        public bool ShowSlider { get; set; }
        
        /// <summary>
        /// Signals whether parameter range on table curve can be modified
        /// </summary>
        [JsonProperty("allowRangeModify")]
        public bool AllowRangeModify { get; set; }
        
        /// <summary>
        /// Signals whether bookmarks can be placed on the table curve parameter range
        /// </summary>
        [JsonProperty("allowBookmarks")]
        public bool AllowBookmarks { get; set; }
    }
    
    #endregion ApplicationSettings
    
}