using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ThreeSelectionView : MonoBehaviour
{
    public Vector3 PillarOffset = Vector3.right;
    public Vector3 CurveOffset = Vector3.zero;
    public float ScalingFactor = 0.25f;

    public List<float[]> ScalingFactorList = new List<float[]>()
    {
        new[] {0.25f, 0.25f, 0.125f},
        new[] {0.05f, 1f, 0.125f},
        new[] {0.0625f, 0.125f, 0.25f},
    };
    
    public Material CurveLineMat;

    AbstractCurveView leftView;
    AbstractCurveView middleView;
    AbstractCurveView rightView;

    
    [NonSerialized] 
    public SelectionChoice selection;
    
    private SelectionExercise _exercise;
    private int _exerciseIndex;
    
    
    private void Start()
    {
        InitExercises();
        InitLineRenders();
        
        leftView.UpdateView();
        middleView.UpdateView();
        rightView.UpdateView();
    }

    private void InitExercises()
    {
        var exercPdsList = new List<ExercisePointDataset>();

        // Exercise 01
        // f(t) = t^3 - 2t , g(t) = t^2 - t
        
        var leftPds = GlobalData.ParamCurveDatasets[0];
        var middlePds = GlobalData.ExerciseCurveDatasets[0];
        var rightPds = GlobalData.ParamCurveDatasets[7];

        ExercisePointDataset exercPds01 = new ExercisePointDataset(leftPds, middlePds, rightPds);
        exercPdsList.Add(exercPds01);
        
        leftPds = GlobalData.ParamCurveDatasets[3];
        middlePds = GlobalData.ParamCurveDatasets[4];
        rightPds = GlobalData.ExerciseCurveDatasets[1];

        ExercisePointDataset exercPds02 = new ExercisePointDataset(rightPds, middlePds, leftPds);
        exercPdsList.Add(exercPds02);
        
        leftPds = GlobalData.ExerciseCurveDatasets[2];
        middlePds = GlobalData.ParamCurveDatasets[8];
        rightPds = GlobalData.ParamCurveDatasets[6];

        ExercisePointDataset exercPds03 = new ExercisePointDataset(rightPds, middlePds, leftPds);
        exercPdsList.Add(exercPds03);
        
        var selChoiceList = new List<SelectionChoice>
        {
            SelectionChoice.MiddlePillar,
            SelectionChoice.RightPillar,
            SelectionChoice.LeftPillar
        };


        var slexerc = new SelectionExercise(
            "TestExercise",
                exercPdsList,
                selChoiceList
            );


        _exercise = slexerc;

    }

    private void InitLineRenders()
    {
        GameObject leftPillar = Instantiate(new GameObject("LeftPillar"), transform);
        LineRenderer leftLR = leftPillar.AddComponent<LineRenderer>();
        leftLR.widthMultiplier = 0.05f;
        leftLR.material = CurveLineMat;

        GameObject middlePillar = Instantiate(new GameObject("MiddlePillar"), transform);
        LineRenderer middleLR = middlePillar.AddComponent<LineRenderer>();
        middleLR.widthMultiplier = 0.05f;
        middleLR.material = CurveLineMat;

        GameObject rightPillar = Instantiate(new GameObject("RightPillar"), transform);
        LineRenderer rightLR = rightPillar.AddComponent<LineRenderer>();
        rightLR.widthMultiplier = 0.05f;
        rightLR.material = CurveLineMat;

        leftPillar.transform.position -= PillarOffset;
        rightPillar.transform.position += PillarOffset;        

        leftView = new SimpleCurveView(leftLR, leftPillar.transform.position + CurveOffset, ScalingFactor);
        middleView = new SimpleCurveView(middleLR, middlePillar.transform.position + CurveOffset, ScalingFactor);
        rightView = new SimpleCurveView(rightLR, rightPillar.transform.position + CurveOffset, ScalingFactor);

        UpdateView();
    }

    public void UpdateView()
    {
        leftView.SetCustomDataset(_exercise.Datasets[_exerciseIndex].LeftDataset);
        middleView.SetCustomDataset(_exercise.Datasets[_exerciseIndex].MiddleDataset);
        rightView.SetCustomDataset(_exercise.Datasets[_exerciseIndex].RightDataset);

        leftView.ScalingFactor = ScalingFactorList[_exerciseIndex][0]; //_exercise.Datasets[_exerciseIndex].LeftDataset.ScalingFactor;
        middleView.ScalingFactor = ScalingFactorList[_exerciseIndex][1]; //_exercise.Datasets[_exerciseIndex].MiddleDataset.ScalingFactor;
        rightView.ScalingFactor = ScalingFactorList[_exerciseIndex][2]; //_exercise.Datasets[_exerciseIndex].RightDataset.ScalingFactor;
        
        leftView.UpdateView();
        middleView.UpdateView();
        rightView.UpdateView();
    }

    public void NextSubExercise()
    {
        if (_exerciseIndex == _exercise.Datasets.Count - 1) return;
        
        ++_exerciseIndex;
        UpdateView();
    }

    public void PreviousSubExercise()
    {
        if (_exerciseIndex == 0) return;
        
        --_exerciseIndex;
         UpdateView();
    }

    public void SetSelection(SelectionChoice choice)
    {
        selection = choice;
    }
    
}
