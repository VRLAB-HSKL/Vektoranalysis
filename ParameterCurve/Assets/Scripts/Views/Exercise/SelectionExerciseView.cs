using Controller.Curve;
using Model;
using UnityEngine;
using Views.Display;

namespace Views.Exercise
{
    public class SelectionExerciseView : AbstractExerciseView
    {
        public enum PillarIdentifier {Left = 0, Middle = 1, Right = 2}

        public PillarIdentifier Pillar;
        
        public new CurveInformationDataset CurrentCurve
        {
            get
            {
                var selExerc = GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex];
                switch (Pillar)
                {
                    case PillarIdentifier.Left:
                        return selExerc.Datasets[GlobalDataModel.CurrentSubExerciseIndex].LeftDataset;
                    
                    default:
                    case PillarIdentifier.Middle:
                        return selExerc.Datasets[GlobalDataModel.CurrentSubExerciseIndex].MiddleDataset;
                    
                    case PillarIdentifier.Right:
                        return selExerc.Datasets[GlobalDataModel.CurrentSubExerciseIndex].RightDataset;
                }

            }
        }
        
        
        public SelectionExerciseView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, 
            PillarIdentifier pid) : 
            base(displayLR, rootPos, scalingFactor)
        {
            Pillar = pid;
        }
        
        public override void UpdateView()
        {
            CurveInformationDataset curve = CurrentCurve; 
        
            var pointArr = curve.worldPoints.ToArray();
            for (var i = 0; i < pointArr.Length; i++)
            {
                pointArr[i] = MapPointPos(pointArr[i]);
            }
        
            DisplayLr.positionCount = curve.worldPoints.Count;
            DisplayLr.SetPositions(pointArr);
        
            DisplayLr.material.color = curve.CurveLineColor;
            DisplayLr.material.SetColor(EmissionColor, curve.CurveLineColor);

        }
        
        
    }
}