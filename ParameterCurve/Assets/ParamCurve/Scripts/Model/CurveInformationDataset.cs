using System.Collections.Generic;
using Model;
using UnityEngine;

namespace ParamCurve.Scripts.Model
{
    public class CurveInformationDataset
    {
        #region Public members
        
        /// <summary>
        /// Dataset identifier
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// Dataset name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Display string used in GUI
        /// </summary>
        public string DisplayString { get; set; } = string.Empty;
        
        /// <summary>
        /// URL to notebook containing dataset information
        /// </summary>
        public string NotebookURL = string.Empty;

        /// <summary>
        /// View type
        /// </summary>
        public string View = string.Empty;
    
        /// <summary>
        /// Icon texture
        /// </summary>
        public Texture2D MenuButtonImage { get; set; }

        /// <summary>
        /// Signals whether this curve is 2D or 3D
        /// </summary>
        public bool Is3DCurve { get; set; }

        /// <summary>
        /// Scaling factor for world curve
        /// </summary>
        public float WorldScalingFactor { get; set; }
        
        /// <summary>
        /// Scaling factor for table curve
        /// </summary>
        public float TableScalingFactor { get; set; }
        
        /// <summary>
        /// Scaling factor for curves displayed inside exercise pillars
        /// </summary>
        public float SelectExercisePillarScalingFactor { get; set; }

        /// <summary>
        /// Curve line color
        /// </summary>
        public Color CurveLineColor { get; set; }
    
        /// <summary>
        /// Travel object color
        /// </summary>
        public Color TravelObjColor { get; set; }
    
        /// <summary>
        /// Arc travel object color
        /// </summary>
        public Color ArcTravelObjColor { get; set; }

        /// <summary>
        /// Curve points
        /// </summary>
        public List<Vector3> Points { get; } = new List<Vector3>();

        /// <summary>
        /// Curve world points
        /// </summary>
        public List<Vector3> WorldPoints { get; } = new List<Vector3>();
        
        /// <summary>
        /// Curve parameter values (t)
        /// </summary>
        public List<float> ParamValues { get; set; } = new List<float>();

        /// <summary>
        /// Time-distance plot points
        /// </summary>
        public List<Vector2> TimeDistancePoints { get; set; } = new List<Vector2>();
        
        /// <summary>
        /// Time-velocity plot points
        /// </summary>
        public List<Vector2> TimeVelocityPoints { get; set; } = new List<Vector2>();

        /// <summary>
        /// Curve fresnet apparatuses
        /// </summary>
        public List<FresnetSerretApparatus> FresnetApparatuses { get; set; } = new List<FresnetSerretApparatus>();

        /// <summary>
        /// Curve arc length
        /// </summary>
        public float ArcLength { get; set; }
        
        /// <summary>
        /// Points of arc length parametrization
        /// </summary>
        public List<Vector3> ArcLenghtPoints { get; } = new List<Vector3>();
        
        /// <summary>
        /// World points of arc length parametrization
        /// </summary>
        public List<Vector3> ArcLengthWorldPoints { get; } = new List<Vector3>();
        
        /// <summary>
        /// Parameter values (t) of arc length parametrization
        /// </summary>
        public List<float> ArcLengthParamValues { get; set; } = new List<float>();

        /// <summary>
        /// Fresnet apparatuses of arc length parametrization
        /// </summary>
        public List<FresnetSerretApparatus> ArcLengthFresnetApparatuses { get; } = new List<FresnetSerretApparatus>();

        #endregion Public members

        #region Public functions
        
        /// <summary>
        /// Calculates the world points for the current scene based on root transform
        /// </summary>
        public void CalculateWorldPoints()
        {
            // Calculate world points
            WorldPoints.Clear();
            for(var i = 0; i < Points.Count; i++)
            {
                var point = Points[i];
                var swapYZCoordinates = Is3DCurve;
                var newPoint = swapYZCoordinates ?
                    new Vector3(point.x, point.z, point.y) * GlobalDataModel.PointScaleFactor :
                    new Vector3(point.x, point.y, point.z) * GlobalDataModel.PointScaleFactor;

                WorldPoints.Add(newPoint);
            }
        
            // Calculate arc world points
            ArcLengthWorldPoints.Clear();
            for(var i = 0; i < ArcLenghtPoints.Count; i++)
            {
                var arcPoint = ArcLenghtPoints[i];
                var swapYZCoordinates = Is3DCurve;
            
                ArcLengthWorldPoints.Add(swapYZCoordinates ?
                    new Vector3(arcPoint.x, arcPoint.z, arcPoint.y) * GlobalDataModel.PointScaleFactor :
                    new Vector3(arcPoint.x, arcPoint.y, arcPoint.z) * GlobalDataModel.PointScaleFactor);
            }
        }
        
        #endregion Public functions
    }
}