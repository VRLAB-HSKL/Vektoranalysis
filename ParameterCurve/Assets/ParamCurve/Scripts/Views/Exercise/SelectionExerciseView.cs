using Model;
using ParamCurve.Scripts.Model;
using UnityEngine;

namespace ParamCurve.Scripts.Views.Exercise
{
    public class SelectionExerciseView : AbstractExerciseView
    {
        #region Public members
        
        /// <summary>
        /// Positional identifier for pillars
        /// </summary>
        public enum PillarIdentifier {Left = 0, Middle = 1, Right = 2}

        #endregion Public members
        
        #region Private members
        
        /// <summary>
        /// Local positional identifier of pillar
        /// </summary>
        private readonly PillarIdentifier _pillar;

        private new CurveInformationDataset CurrentCurve
        {
            get
            {
                var selExercise = GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex];
                var cd = selExercise.Datasets[GlobalDataModel.CurrentSubExerciseIndex].GetCurveData();
                switch (_pillar)
                {
                    case PillarIdentifier.Left:
                        return cd[0];
                    
                    default:
                    case PillarIdentifier.Middle:
                        return cd[1];
                    
                    case PillarIdentifier.Right:
                        return cd[2];
                }

            }
        }

        #endregion Private members
        
        #region Constructors
        
        public SelectionExerciseView(LineRenderer displayLr, Vector3 rootPos, float scalingFactor, 
            PillarIdentifier pid) : 
            base(displayLr, rootPos, scalingFactor)
        {
            _pillar = pid;
        }
        
        #endregion Constructors
        
        #region Public functions
        
        public override void UpdateView()
        {
            var curve = CurrentCurve; 
        
            var pointArr = curve.WorldPoints.ToArray();
            for (var i = 0; i < pointArr.Length; i++)
            {
                pointArr[i] = MapPointPos(pointArr[i]);
            }
        
            DisplayLr.positionCount = curve.WorldPoints.Count;
            DisplayLr.SetPositions(pointArr);
        
            DisplayLr.material.color = curve.CurveLineColor;
            DisplayLr.material.SetColor(EmissionColor, curve.CurveLineColor);
        }
        
        #endregion Public functions
        
    }
}