using System;
using System.Collections.Generic;
using Controller.Curve;
using TMPro;
using UnityEngine;
using Views.Display;
using Views.Exercise;

namespace Views
{
    public class SelectionExerciseCompoundView // : AbstractCompoundView
    {
        public CurveControllerTye Type;
        
        private GameObject PillarPrefab;
        private List<AbstractCurveView> curveViews = new List<AbstractCurveView>();
        private Transform origin;
        
        private SelectionExerciseGameObjects selObjects { get; set; }

        private SelectionExercise CurrentSelectionExercise
        {
            get
            {
                return GlobalData.SelectionExercises[GlobalData.CurrentExerciseIndex];
            }
        }
        
        
        //public ExercisePointDataset CurrentExerciseData { get; set; }
        public int CurrentExerciseIndex { get; set; }
        public string CurrentTitle { get; set; }
        public string CurrentDescription { get; set; }
        public bool showMainDisplay { get; set; } = true;
        
        [NonSerialized] 
        
        public List<float[]> ScalingFactorList = new List<float[]>()
        {
            new[] {1f, 1f, 1f},
            new[] {1f, 1f, 1f},
            new[] {1f, 1f, 1f},
            new[] {1f, 1f, 1f},
            new[] {1f, 1f, 1f},
            new[] {1f, 1f, 1f},
        };

        
        
        public SelectionExerciseCompoundView(
            SelectionExerciseGameObjects selObjects, GameObject pillarPrefab, Transform origin, 
            ExercisePointDataset initData, CurveControllerTye type)
        {
            Type = type;
            PillarPrefab = pillarPrefab;
            //CurrentExerciseData = initData;
            this.selObjects = selObjects;
            this.origin = origin;

            InitLineRenders();
            
            foreach (var p in curveViews)
            {
                p.UpdateView();
            }
            
        }
        
        private void InitLineRenders()
        {
            var leftPillar = selObjects.leftPillar;
            var middlePillar = selObjects.middlePillar;
            var rightPillar = selObjects.rightPillar;

            LineRenderer leftLR = leftPillar.GetComponentInChildren<LineRenderer>();
            LineRenderer middleLR = middlePillar.GetComponentInChildren<LineRenderer>();
            LineRenderer rightLR = rightPillar.GetComponentInChildren<LineRenderer>();

            leftLR.widthMultiplier = 0.05f;
            leftLR.material = selObjects.CurveLineMat;
            
            middleLR.widthMultiplier = 0.05f;
            middleLR.material = selObjects.CurveLineMat;
            
            rightLR.widthMultiplier = 0.05f;
            rightLR.material = selObjects.CurveLineMat;
            
            curveViews.Add(new SelectionExerciseView(
                            leftLR, 
                            leftPillar.transform.position + selObjects.CurveOffset, 
                            selObjects.ScalingFactor,
                            SelectionExerciseView.PillarIdentifier.Left, Type));
            
            curveViews.Add(new SelectionExerciseView(
                            middleLR, 
                            middlePillar.transform.position + selObjects.CurveOffset, 
                            selObjects.ScalingFactor,
                            SelectionExerciseView.PillarIdentifier.Middle, Type));
            
            curveViews.Add(new SelectionExerciseView(
                            rightLR, 
                            rightPillar.transform.position + selObjects.CurveOffset, 
                            selObjects.ScalingFactor, 
                            SelectionExerciseView.PillarIdentifier.Right, Type));

            UpdateView();
        }

        public void UpdateView()
        {
            //Debug.Log("SelectionExerciseView.UpdateView()");
            // curveViews[0].SetCustomDataset(CurrentExerciseData.LeftDataset);
            // curveViews[1].SetCustomDataset(CurrentExerciseData.MiddleDataset);
            // curveViews[2].SetCustomDataset(CurrentExerciseData.RightDataset);

            // curveViews[0].ScalingFactor = CurrentExerciseData.LeftDataset.SelectExercisePillarScalingFactor;
            // curveViews[1].ScalingFactor = CurrentExerciseData.MiddleDataset.SelectExercisePillarScalingFactor;
            // curveViews[2].ScalingFactor = CurrentExerciseData.RightDataset.SelectExercisePillarScalingFactor;
            
            
            
            if (showMainDisplay)
            {
                selObjects.MiddleDisplayText.text = CurrentDescription;
                selObjects.ExerciseTitle.text = string.Empty;
                selObjects.SubExerciseIdentifier.text = string.Empty;
                selObjects.HeaderText.text = string.Empty;
                
                ShowMainDisplayView();
            }
            else
            {
                selObjects.MiddleDisplayText.text = string.Empty;
                
                selObjects.ExerciseTitle.text = CurrentTitle;
                
                // Start incrementing on small 'a' character
                var subExerciseLetter = (char) (97 + CurrentExerciseIndex);
                selObjects.SubExerciseIdentifier.text = subExerciseLetter + ")";
                selObjects.HeaderText.text = GlobalData.SelectionExercises[GlobalData.CurrentExerciseIndex]
                    .Datasets[GlobalData.CurrentSubExerciseIndex].HeaderText;
                
                ShowSelectionView();
            }

            foreach (AbstractCurveView v in curveViews)
            {
                v.UpdateView();
            }
        }

        public void StartRun()
        {
            // ToDo: Refactor this in class hierarchy, is this even needed here ?
        }


        private void ShowMainDisplayView()
        {
            //Debug.Log("ShowMainDisplayView()");
            selObjects.SelectionParent.SetActive(false);
            selObjects.MainDisplayParent.SetActive(true);
        }

        private void ShowSelectionView()
        {
            //Debug.Log("ShowSelectionView()");
            selObjects.MainDisplayParent.SetActive(false);
            selObjects.SelectionParent.SetActive(true);
        }
        
    }
}