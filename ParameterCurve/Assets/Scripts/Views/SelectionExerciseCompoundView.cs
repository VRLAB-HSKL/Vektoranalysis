﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Views
{
    public class SelectionExerciseCompoundView : AbstractCompoundView, IView
    {
        private GameObject PillarPrefab;
        private List<AbstractCurveView> curveViews = new List<AbstractCurveView>();
        private Transform origin;
        
        private SelectionExerciseGameObjects selObjects;

        public ExercisePointDataset CurrentExerciseData { get; set; }
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

        
        
        public SelectionExerciseCompoundView(SelectionExerciseGameObjects selObjects, GameObject pillarPrefab, Transform origin, ExercisePointDataset initData)
        {
            PillarPrefab = pillarPrefab;
            CurrentExerciseData = initData;
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
            
            curveViews.Add(new SimpleCurveView(leftLR, leftPillar.transform.position + selObjects.CurveOffset, selObjects.ScalingFactor));
            curveViews.Add(new SimpleCurveView(middleLR, middlePillar.transform.position + selObjects.CurveOffset, selObjects.ScalingFactor));
            curveViews.Add(new SimpleCurveView(rightLR, rightPillar.transform.position + selObjects.CurveOffset, selObjects.ScalingFactor));

            UpdateView();
        }

        public void UpdateView()
        {
            //Debug.Log("SelectionExerciseView.UpdateView()");
            curveViews[0].SetCustomDataset(CurrentExerciseData.LeftDataset);
            curveViews[1].SetCustomDataset(CurrentExerciseData.MiddleDataset);
            curveViews[2].SetCustomDataset(CurrentExerciseData.RightDataset);
            
            // ToDo: Import scaling factor from json and set them here
            
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
                selObjects.HeaderText.text = CurrentExerciseData.HeaderText;
                
                ShowSelectionView();
            }

            foreach (AbstractCurveView v in curveViews)
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