using Controller.Curve;
using UnityEngine;

namespace Views.Exercise
{
    public class SelectionExerciseView : SimpleCurveView
    {
        public enum PillarIdentifier {Left = 0, Middle = 1, Right = 2}

        public PillarIdentifier Pillar;
        
        public new CurveInformationDataset CurrentCurve
        {
            get
            {
                //

                var selExerc = GlobalData.SelectionExercises[GlobalData.CurrentExerciseIndex];
                switch (Pillar)
                {
                    case PillarIdentifier.Left:
                        return selExerc.Datasets[GlobalData.CurrentSubExerciseIndex].LeftDataset;
                    
                    default:
                    case PillarIdentifier.Middle:
                        return selExerc.Datasets[GlobalData.CurrentSubExerciseIndex].MiddleDataset;
                    
                    case PillarIdentifier.Right:
                        return selExerc.Datasets[GlobalData.CurrentSubExerciseIndex].RightDataset;
                }

            }
        }
        
        
        public SelectionExerciseView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, PillarIdentifier pid, CurveControllerTye controllerType) : 
            base(displayLR, rootPos, scalingFactor, controllerType)
        {
            Pillar = pid;
        }
        
        public override void UpdateView()
        {
            CurveInformationDataset curve = CurrentCurve; // HasCustomDataset ? CustomDataset : GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

            //ScalingFactor =
        
            var pointArr = curve.worldPoints.ToArray();
            for (var i = 0; i < pointArr.Length; i++)
            {
                pointArr[i] = MapPointPos(pointArr[i]); //, curve.Is3DCurve);
            }
        
            DisplayLr.positionCount = curve.worldPoints.Count;
            DisplayLr.SetPositions(pointArr);
        
            DisplayLr.material.color = curve.CurveLineColor;
            DisplayLr.material.SetColor(EmissionColor, curve.CurveLineColor);

        }
        
        
    }
}