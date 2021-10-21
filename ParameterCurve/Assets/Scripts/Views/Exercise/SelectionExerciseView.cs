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
        
        
        public SelectionExerciseView(LineRenderer displayLR, Vector3 rootPos, float scalingFactor, PillarIdentifier pid) : 
            base(displayLR, rootPos, scalingFactor)
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
                pointArr[i] = MapPointPos(pointArr[i]);
            }
        
            _displayLr.positionCount = curve.worldPoints.Count;
            _displayLr.SetPositions(pointArr);
        
            _displayLr.material.color = curve.CurveLineColor;
            _displayLr.material.SetColor(EmissionColor, curve.CurveLineColor);

        }
        
        
    }
}