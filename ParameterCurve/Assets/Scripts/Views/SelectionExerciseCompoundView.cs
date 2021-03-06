using System;
using System.Collections.Generic;
using Controller.Curve;
using Model;
using TMPro;
using UnityEngine;
using Views.Display;
using Views.Exercise;

namespace Views
{
    public class SelectionExerciseCompoundView : AbstractExerciseView
    {
        public AbstractCurveViewController.CurveControllerType Type;
        
        private GameObject PillarPrefab;
        private List<AbstractExerciseView> curveViews { get; set; } = new List<AbstractExerciseView>();
        private Transform origin;
        
        private SelectionExerciseGameObjects selObjects { get; set; }

        private SelectionExercise CurrentSelectionExercise => 
            GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex];

        public string CurrentTitle { get; set; }
        public string CurrentDescription { get; set; }
        
        
        public SelectionExerciseCompoundView(
            SelectionExerciseGameObjects selObjects, GameObject pillarPrefab, Transform origin, 
            ExercisePointDataset initData, AbstractCurveViewController.CurveControllerType type)
            // : base(selObjects, pillarPrefab, origin, initData, type)
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
                            SelectionExerciseView.PillarIdentifier.Left));
            
            curveViews.Add(new SelectionExerciseView(
                            middleLR, 
                            middlePillar.transform.position + selObjects.CurveOffset, 
                            selObjects.ScalingFactor,
                            SelectionExerciseView.PillarIdentifier.Middle));
            
            curveViews.Add(new SelectionExerciseView(
                            rightLR, 
                            rightPillar.transform.position + selObjects.CurveOffset, 
                            selObjects.ScalingFactor, 
                            SelectionExerciseView.PillarIdentifier.Right));

            UpdateView();
        }

        public override void UpdateView()
        {
            var currentSubExercise = CurrentSelectionExercise.Datasets[GlobalDataModel.CurrentSubExerciseIndex]; 
            
            curveViews[0].ScalingFactor = currentSubExercise.LeftDataset.SelectExercisePillarScalingFactor;
            curveViews[1].ScalingFactor = currentSubExercise.MiddleDataset.SelectExercisePillarScalingFactor;
            curveViews[2].ScalingFactor = currentSubExercise.RightDataset.SelectExercisePillarScalingFactor;
            
            if (ShowMainDisplay)
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
                var subExerciseLetter = (char) (97 + GlobalDataModel.CurrentSubExerciseIndex);
                selObjects.SubExerciseIdentifier.text = subExerciseLetter + ")";
                selObjects.HeaderText.text = GlobalDataModel.SelectionExercises[GlobalDataModel.CurrentExerciseIndex]
                    .Datasets[GlobalDataModel.CurrentSubExerciseIndex].HeaderText;
                
                ShowSelectionView();
                
            }

            foreach (AbstractExerciseView v in curveViews)
            {
                v.UpdateView();
            }
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