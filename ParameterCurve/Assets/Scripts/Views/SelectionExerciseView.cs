
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectionExerciseView : AbstractCurveView
{
    private readonly AbstractCurveView _leftPillar;
    private readonly AbstractCurveView _rightPillar;

    private GameObject PillarPrefab;
    
    private readonly SelectionExercise _exercise;
    private int _currentExerciseIndex;
    
    public SelectionExerciseView(
        LineRenderer leftLR, Vector3 leftRootPos, float leftScalingFactor,
        LineRenderer middleLR, Vector3 middleRootPos, float middleScalingFactor,
        LineRenderer rightLR, Vector3 rightRootPos, float rightScalingFactor,
        GameObject pillarPrefab, SelectionExercise exercise) : 
        base(middleLR, middleRootPos, middleScalingFactor)
    {
        _leftPillar = new SimpleCurveView(leftLR, leftRootPos, leftScalingFactor);
        _rightPillar = new SimpleCurveView(rightLR, rightRootPos, rightScalingFactor);
        PillarPrefab = pillarPrefab;
        _exercise = exercise;
        
        SetCurveData();
    }

    public override void UpdateView()
    {
        _leftPillar.UpdateView();
        base.UpdateView();
        _rightPillar.UpdateView();
    }

    public void NextSubExercise()
    {
        if (_currentExerciseIndex == _exercise.Datasets.Count - 1) return;
        
        ++_currentExerciseIndex;
        SetCurveData();
        UpdateView();
    }

    public void PreviousSubExercise()
    {
        if (_currentExerciseIndex == 0) return;
        
        --_currentExerciseIndex;
        SetCurveData();
        UpdateView();
    }

    private void SetCurveData()
    {
        var datasets = _exercise.Datasets[_currentExerciseIndex];
        _leftPillar.SetCustomDataset(datasets.LeftDataset);
        SetCustomDataset(datasets.MiddleDataset);
        _rightPillar.SetCustomDataset(datasets.RightDataset);
    }
    
    
    
    
}