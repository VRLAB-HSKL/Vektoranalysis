using System.Collections.Generic;
using Import.InitFile;
using Model;
using UnityEngine;
using Utility;

namespace Import
{
    /// <summary>
    /// Import class used to import and save data from the generated .json init file
    /// </summary>
    public static class DataImport
    {
        #region Public members
        
        /// <summary>
        /// Horizontal rendering size of the X axis of the time/distance plot on the information wall
        /// </summary>
        public static float TimeDistanceXAxisLength { get; set; }

        /// <summary>
        /// Vertical rendering size of the Y axis of the time/distance plot on the information wall
        /// </summary>
        public static float TimeDistanceYAxisLength { get; set; }

        /// <summary>
        /// Horizontal rendering size of the X axis of the time/velocity plot on the information wall
        /// </summary>
        public static float TimeVelocityXAxisLength { get; set; }

        /// <summary>
        /// Vertical rendering size of the Y axis of the time/velocity plot on the information wall
        /// </summary>
        public static float TimeVelocityYAxisLength { get; set; }

        #endregion Public members

        //private static NumberFormatInfo _nfi = new NumberFormatInfo() {NumberDecimalSeparator = "."};

        #region Public functions

        /// <summary>
        /// Generate a local curve dataset from a <see cref="Curve"/> node that was generated during initial parsing of#
        /// the init file. 
        /// </summary>
        /// <param name="curve">Curve data node</param>
        /// <returns>Generated curve dataset</returns>
        public static CurveInformationDataset CreatePointDatasetFromCurve(Curve curve)
        {
            var pd = new CurveInformationDataset
            {
                ID = curve.Info.Id,
                Name = curve.Info.Name,
                DisplayString = curve.Info.DisplayText,
                NotebookURL = string.Empty,
                View = curve.CurveSettings.DisplaySettings.View
            };

            // Read curve line color from init file
            var lineColorValues = curve.CurveSettings.DisplaySettings.LineColor;
            var lineColor = new Color(lineColorValues.Rgba[0], lineColorValues.Rgba[1],
                lineColorValues.Rgba[2], lineColorValues.Rgba[3]);
            pd.CurveLineColor = lineColor;

            // Read travel obj color from init file
            var travelObjValArr = curve.CurveSettings.DisplaySettings.TravelObjColor.Rgba;
            var travelObjColor = new Color(travelObjValArr[0], travelObjValArr[1],
                travelObjValArr[2], travelObjValArr[3]);
            pd.TravelObjColor = travelObjColor;

            // Read arc travel obj color from init file
            var arcTravelObjValArr = curve.CurveSettings.DisplaySettings.ArcTravelObjColor.Rgba;
            var arcTravelObjColor = new Color(arcTravelObjValArr[0], arcTravelObjValArr[1],
                arcTravelObjValArr[2], arcTravelObjValArr[3]);
            pd.ArcTravelObjColor = arcTravelObjColor;

            // Attempt to load image resource based on curve name        
            var imgResPath = GlobalDataModel.ImageResourcePath + curve.Info.Name;
            var imgRes = Resources.Load(imgResPath) as Texture2D;
            if (imgRes != null)
            {
                pd.MenuButtonImage = imgRes;
            }

            pd.Is3DCurve = curve.Data.Dimension == 3;
            pd.ArcLength = curve.Data.ArcLength;
            pd.WorldScalingFactor = curve.Data.WorldScalingFactor;
            pd.TableScalingFactor = curve.Data.TableScalingFactor;
            pd.SelectExercisePillarScalingFactor = curve.Data.SelectExercisePillarScalingFactor;
            pd.ParamValues = curve.Data.Data.T;
            pd.ArcLength = curve.Data.ArcLength;

            for (var j = 0; j < curve.Data.Data.T.Count; j++)
            {
                pd.Points.Add(new Vector3(
                    curve.Data.Data.PVec[j][0],
                    curve.Data.Data.PVec[j][1],
                    pd.Is3DCurve ? curve.Data.Data.PVec[j][2] : 0f
                ));

                pd.TimeDistancePoints = CalculateTimeDistancePoints(pd.Points);
                pd.TimeVelocityPoints = CalculateTimeVelocityPoints(pd.Points);

                var fsr = new FresnetSerretApparatus
                {
                    Tangent = new Vector3(
                        curve.Data.Data.VelVec[j][0],
                        curve.Data.Data.VelVec[j][1],
                        pd.Is3DCurve ? curve.Data.Data.VelVec[j][2] : 0f
                    ).normalized,
                    Normal = new Vector3(
                        curve.Data.Data.NormVec[j][0],
                        curve.Data.Data.NormVec[j][1],
                        pd.Is3DCurve ? curve.Data.Data.NormVec[j][2] : 0f
                    ).normalized,
                    Binormal = new Vector3(
                        curve.Data.Data.BinormVec[j][0],
                        curve.Data.Data.BinormVec[j][1],
                        pd.Is3DCurve ? curve.Data.Data.BinormVec[j][2] : 0f
                    ).normalized
                    
                };
                

                // var bin = Vector3.Cross(fsr.Tangent, fsr.Normal);
                // fsr.Binormal = new Vector3(bin.x, bin.y, -Mathf.Abs(bin.z));

                // if (j == 10 && pd.Name == "helix")
                // {
                //     Debug.Log(pd.Name + "_j10: " + 
                //               fsr.Binormal[0].ToString("#0.00000000") + " " +
                //               fsr.Binormal[1].ToString("#0.00000000") + " " +
                //               fsr.Binormal[2].ToString("#0.00000000"));
                // }

                pd.FresnetApparatuses.Add(fsr);
                pd.ArcLengthParamValues = curve.Data.Data.ArcT;
                pd.ArcLenghtPoints.Add(new Vector3(
                    curve.Data.Data.ArcPVec[j][0],
                    curve.Data.Data.ArcPVec[j][1],
                    pd.Is3DCurve ? curve.Data.Data.ArcPVec[j][2] : 0f
                ));

                var arcFsr = new FresnetSerretApparatus
                {
                    Tangent = new Vector3(
                        curve.Data.Data.ArcVelVec[j][0],
                        curve.Data.Data.ArcVelVec[j][1],
                        pd.Is3DCurve ? curve.Data.Data.ArcVelVec[j][2] : 0f
                    ).normalized,
                    Normal = new Vector3(
                        curve.Data.Data.ArcAccVec[j][0],
                        curve.Data.Data.ArcAccVec[j][1],
                        pd.Is3DCurve ? curve.Data.Data.ArcAccVec[j][2] : 0f
                    ).normalized
                };

                var arcBin = Vector3.Cross(fsr.Tangent, fsr.Normal);
                //arcFsr.Binormal = new Vector3(arcBin.x, arcBin.y, -Mathf.Abs(arcBin.z));
                // Vector3.Cross(arcFsr.Tangent, arcFsr.Normal);

                pd.ArcLengthFresnetApparatuses.Add(arcFsr);
            }

            pd.CalculateWorldPoints();

            return pd;
        }

        /// <summary>
        /// Generate a local exercise dataset from a <see cref="SubExercise"/> node that was generated during initial
        /// parsing of the init file.
        /// </summary>
        /// <param name="sub">Sub exercise node</param>
        /// <returns>Generated exercise dataset</returns>
        public static SelectionExerciseDataset CreateExercisePointDatasetFromSubExercise(SelectThree sub)
        {
            return new SelectionExerciseDataset(
                sub.Description,
                CreatePointDatasetFromCurve(sub.LeftCurve),
                CreatePointDatasetFromCurve(sub.MiddleCurve),
                CreatePointDatasetFromCurve(sub.RightCurve)
            );
        }

        /// <summary>
        /// Generate a local exercise dataset from a <see cref="SubExercise"/> node that was generated during initial
        /// parsing of the init file.
        /// </summary>
        /// <param name="sub">Sub exercise node</param>
        /// <returns>Generated exercise dataset</returns>
        public static TangentNormalExerciseDataset CreateTangentNormalDataFromSubExercise(TangentNormal sub)
        {
            return new TangentNormalExerciseDataset(
                sub.Description,
                CreatePointDatasetFromCurve(sub.TangentNormalCurve),
                sub.HighlightPoints
            );
        }

        #endregion Public functions

        #region Private functions

        /// <summary>
        /// Calculate in game time distance plot points based on imported curve polyline
        /// </summary>
        /// <param name="points">Curve polyline</param>
        /// <returns>In game 2d plot points</returns>
        private static List<Vector2> CalculateTimeDistancePoints(List<Vector3> points)
        {
            // Setup variables
            var tdPoints = new List<Vector2>();
            var numSteps = points.Count;

            var maxDistance = CalcUtil.CalculateRawDistance(points);
            var currentDistance = 0f;
            var maxY = 0f;

            // Measure distances between points on polyline
            for (var i = 0; i < numSteps; i++)
            {
                // Move one step to the right on the horizontal x-axis each iteration
                var x = (i / (float) numSteps) *
                        TimeDistanceXAxisLength;
                float y;

                // Start at the vertical origin on first iteration
                if (i == 0)
                {
                    y = 0f;
                }
                // Otherwise calculate vertical distance on y-axis
                else
                {
                    // Calculate new vertical offset based on previous point location
                    currentDistance += Vector3.Distance(points[i], points[i - 1]);
                    y = currentDistance;
                    y /= maxDistance;

                    if (y > maxY)
                    {
                        maxY = y;
                    }
                }

                tdPoints.Add(new Vector2(x, y));
            }

            // Scale the values based on in game render size
            for (var i = 0; i < tdPoints.Count; i++)
            {
                var p = tdPoints[i];
                var factor = p.y / maxY;
                var newY = factor * TimeDistanceYAxisLength;
                tdPoints[i] = new Vector2(p.x, newY);
            }

            return tdPoints;
        }

        /// <summary>
        /// Calculate in game time distance plot points based on imported curve polyline
        /// </summary>
        /// <param name="points">Curve polyline</param>
        /// <returns></returns>
        private static List<Vector2> CalculateTimeVelocityPoints(List<Vector3> points)
        {
            var tvPoints = new List<Vector2>();
            var numSteps = points.Count;
            var maxVelocity = 0f;

            for (var i = 0; i < numSteps; i++)
            {
                var x = (i / (float) numSteps) * TimeVelocityXAxisLength;
                float y;
                if (i == 0)
                {
                    y = 0f;
                }
                else
                {
                    var distance = Vector3.Distance(points[i], points[i - 1]);
                    y = distance;

                    if (y > maxVelocity)
                    {
                        maxVelocity = y;
                    }
                }
                tvPoints.Add(new Vector2(x, y));
            }

            for (var i = 0; i < tvPoints.Count; i++)
            {
                var v = tvPoints[i];
                var factor = v.y / maxVelocity;
                tvPoints[i] = new Vector2(v.x, factor * TimeVelocityYAxisLength);
            }

            return tvPoints;
        }

        #endregion Private functions
    }
}
