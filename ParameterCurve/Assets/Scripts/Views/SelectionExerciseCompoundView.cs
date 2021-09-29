using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Views
{
    public class SelectionExerciseCompoundView : AbstractCompoundView
    {
        private GameObject PillarPrefab;
        private List<AbstractCurveView> curveViews;
        private Transform origin;
        
        private SelectionExerciseGameObjects selObjects;

        public ExercisePointDataset CurrentExerciseData;
        public int CurrentExerciseIndex;
        public string CurrentTitle;
        public string CurrentDescription;
        public bool showMainDisplay;
        
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

        
        
        public SelectionExerciseCompoundView(SelectionExerciseGameObjects selObjects, GameObject pillarPrefab, Transform origin)
        {
            // _leftPillar = new SimpleCurveView(leftLR, leftRootPos, leftScalingFactor);
            // _rightPillar = new SimpleCurveView(rightLR, rightRootPos, rightScalingFactor);
            
            //views.Add(new SimpleCurveView());
            
            //PillarPrefab = pillarPrefab;
            //_exercise = exercise;

            PillarPrefab = pillarPrefab;

            this.origin = origin;
            
            //InitExercises();
            InitLineRenders();
            //SetCurveData();
            
            
        
            // leftView.UpdateView();
            // middleView.UpdateView();
            // rightView.UpdateView();
            foreach (AbstractCurveView p in curveViews)
            {
                p.UpdateView();
            }
        }


        // public void NextSubExercise()
        // {
        //     if (_currentExerciseIndex == _exercise.Datasets.Count - 1) return;
        //     
        //     ++_currentExerciseIndex;
        //     SetCurveData();
        //     UpdateView();
        // }
        //
        // public void PreviousSubExercise()
        // {
        //     if (_currentExerciseIndex == 0) return;
        //     
        //     --_currentExerciseIndex;
        //     SetCurveData();
        //     UpdateView();
        // }
        
        

        

        private void InitLineRenders()
        {
            GameObject leftPillar = MonoBehaviour.Instantiate(new GameObject("LeftPillar"), origin);
            LineRenderer leftLR = leftPillar.AddComponent<LineRenderer>();
            leftLR.widthMultiplier = 0.05f;
            leftLR.material = selObjects.CurveLineMat;

            GameObject middlePillar = MonoBehaviour.Instantiate(new GameObject("MiddlePillar"), origin);
            LineRenderer middleLR = middlePillar.AddComponent<LineRenderer>();
            middleLR.widthMultiplier = 0.05f;
            middleLR.material = selObjects.CurveLineMat;

            GameObject rightPillar = MonoBehaviour.Instantiate(new GameObject("RightPillar"), origin);
            LineRenderer rightLR = rightPillar.AddComponent<LineRenderer>();
            rightLR.widthMultiplier = 0.05f;
            rightLR.material = selObjects.CurveLineMat;

            leftPillar.transform.position -= selObjects.PillarOffset;
            rightPillar.transform.position += selObjects.PillarOffset;        

            curveViews.Add(new SimpleCurveView(leftLR, leftPillar.transform.position + selObjects.CurveOffset, selObjects.ScalingFactor));
            curveViews.Add(new SimpleCurveView(middleLR, middlePillar.transform.position + selObjects.CurveOffset, selObjects.ScalingFactor));
            curveViews.Add(new SimpleCurveView(rightLR, rightPillar.transform.position + selObjects.CurveOffset, selObjects.ScalingFactor));

            UpdateView();
        }

        // private void SetCurveData(ExercisePointDataset execPds)
        // {
        //     //var datasets = execPds[_exerciseController.currentExerciseIndex];
        //
        //     CurrentExerciseData = execPds;
        //
        // }

        private void UpdateData()
        {
            
        }
        
        
        public void UpdateView()
        {
            curveViews[0].SetCustomDataset(CurrentExerciseData.LeftDataset);
            curveViews[1].SetCustomDataset(CurrentExerciseData.MiddleDataset);
            curveViews[2].SetCustomDataset(CurrentExerciseData.RightDataset);
            
            // ToDo: Import scaling factor from json and set them here

            
            // curveViews[0].SetCustomDataset(_exerciseController.exercise.Datasets[_exerciseController.currentExerciseIndex].LeftDataset);
            // curveViews[1].SetCustomDataset(_exerciseController.exercise.Datasets[_exerciseController.currentExerciseIndex].MiddleDataset);
            // curveViews[2].SetCustomDataset(_exerciseController.exercise.Datasets[_exerciseController.currentExerciseIndex].RightDataset);

            // curveViews[0].ScalingFactor = ScalingFactorList[_exerciseController.currentExerciseIndex][0]; //_exercise.Datasets[_exerciseIndex].LeftDataset.ScalingFactor;
            // curveViews[1].ScalingFactor = ScalingFactorList[_exerciseController.currentExerciseIndex][1]; //_exercise.Datasets[_exerciseIndex].MiddleDataset.ScalingFactor;
            // curveViews[2].ScalingFactor = ScalingFactorList[_exerciseController.currentExerciseIndex][2]; //_exercise.Datasets[_exerciseIndex].RightDataset.ScalingFactor;

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
                selObjects.ExerciseTitle.text = CurrentTitle; //exercise.Title;
                
                // Start incrementing on small 'a' character
                var subExerciseLetter = (char) (97 + CurrentExerciseIndex);
                selObjects.SubExerciseIdentifier.text = subExerciseLetter + ")";
                selObjects.HeaderText.text = CurrentExerciseData.HeaderText;
                
                ShowSelectionView();
            }
            
            // leftView.UpdateView();
            // middleView.UpdateView();
            // rightView.UpdateView();

            foreach (AbstractCurveView v in curveViews)
            {
                v.UpdateView();
            }
        }

        

        private void ShowMainDisplayView()
        {
            selObjects.SelectionParent.SetActive(false);
            selObjects.MainDisplayParent.SetActive(true);
        }

        private void ShowSelectionView()
        {
            selObjects.MainDisplayParent.SetActive(false);
            selObjects.SelectionParent.SetActive(true);
        }
        
    }
}