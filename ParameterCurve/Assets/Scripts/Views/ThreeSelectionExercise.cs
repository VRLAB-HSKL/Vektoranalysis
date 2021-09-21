using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ThreeSelectionExercise : MonoBehaviour
{
    public Vector3 PillarOffset = Vector3.right;
    public Vector3 CurveOffset = Vector3.zero;
    public float ScalingFactor = 0.25f;

    public TextMeshProUGUI ExerciseTitle;
    public TextMeshProUGUI SubExerciseIdentifier;
    public TextMeshProUGUI HeaderText;

    public List<float[]> ScalingFactorList = new List<float[]>()
    {
        new[] {1f, 0.25f, 0.125f},
        new[] {0.125f, 1f, 0.05f},
        new[] {0.25f, 0.125f, 0.0625f},
        new[] {0.125f, 0.25f, 1f},
        new[] {1f, 0.5f, 0.125f},
        new[] {1f, 0.5f, 0.125f}
    };
    
    public Material CurveLineMat;

    
    
    AbstractCurveView leftView;
    AbstractCurveView middleView;
    AbstractCurveView rightView;

    
    [NonSerialized] 
    public int selectionIndex;
    
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
        
        var leftPds = GlobalData.ParamCurveDatasets[10];
        var middlePds = GlobalData.ExerciseCurveDatasets[0];
        var rightPds = GlobalData.ParamCurveDatasets[7];

        ExercisePointDataset exercPds01 = new ExercisePointDataset(
            "f(t) = t<sup>3</sup> - 2t" + "\n" +"g(t) = t<sup>2</sup> - t",
            leftPds, middlePds, rightPds);
        exercPdsList.Add(exercPds01);
        
        // Exercise 02
        leftPds = GlobalData.ParamCurveDatasets[3];
        middlePds = GlobalData.ParamCurveDatasets[4];
        rightPds = GlobalData.ExerciseCurveDatasets[1];

        ExercisePointDataset exercPds02 = new ExercisePointDataset(
            "f(t) = t<sup>4</sup> - t<sup>2</sup>" + "\n" + "g(t) = t + ln(t)",
            leftPds, middlePds, rightPds);
        exercPdsList.Add(exercPds02);
        
        // Exercise 03
        leftPds = GlobalData.ExerciseCurveDatasets[2];
        middlePds = GlobalData.ParamCurveDatasets[8];
        rightPds = GlobalData.ParamCurveDatasets[6];

        ExercisePointDataset exercPds03 = new ExercisePointDataset(
            "f(t) = sin(3t)" + "\n" + "g(t) = sin(4t)",
            leftPds, middlePds, rightPds);
        exercPdsList.Add(exercPds03);
        
        // Exercise 04
        leftPds = GlobalData.ExerciseCurveDatasets[1];
        middlePds = GlobalData.ParamCurveDatasets[1];
        rightPds = GlobalData.ParamCurveDatasets[4];

        ExercisePointDataset exercPds04 = new ExercisePointDataset(
            "f(t) = t + sin(2t)" + "\n" + "g(t) = t + sin(3t)",
            leftPds, middlePds, rightPds);
        exercPdsList.Add(exercPds04);
        
        // Exercise 05
        leftPds = GlobalData.ParamCurveDatasets[10];
        middlePds = GlobalData.ExerciseCurveDatasets[4];
        rightPds = GlobalData.ExerciseCurveDatasets[1]; // wrong one

        ExercisePointDataset exercPds05 = new ExercisePointDataset(
            "f(t) = sin(t + sin(t))" + "\n" + "g(t) = cos(t + cos(t))",
            leftPds, middlePds, rightPds);
        exercPdsList.Add(exercPds05);
        
        // Exercise 06
        leftPds = GlobalData.ExerciseCurveDatasets[5];
        middlePds = GlobalData.ParamCurveDatasets[8];
        rightPds = GlobalData.ParamCurveDatasets[7];

        ExercisePointDataset exercPds06 = new ExercisePointDataset(
            "f(t) = cos(t)" + "\n" + "g(t) = sin(t + sin(5t))",
            leftPds, middlePds, rightPds);
        exercPdsList.Add(exercPds06);
        
        var selChoiceList = new List<int>
        {
            2, //SelectionChoice.MiddlePillar,
            3, //SelectionChoice.RightPillar,
            1, //SelectionChoice.LeftPillar,
            1, //SelectionChoice.LeftPillar,
            2, //SelectionChoice.MiddlePillar,
            1, //SelectionChoice.LeftPillar
        };


        var selExercise = new SelectionExercise(
            "Parameter5",
                exercPdsList,
                selChoiceList
            );


        //_exercise = selExercise;

        _exercise = GlobalData.SelectionExercises[0];


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
        Debug.Log("datasetsCount: " + _exercise.Datasets.Count);
        
        if(leftView is null)
            Debug.Log("leftView is null");
        
        if(_exercise.Datasets[_exerciseIndex].LeftDataset is null)
            Debug.Log("LeftDataset is null");
        
        leftView.SetCustomDataset(_exercise.Datasets[_exerciseIndex].LeftDataset);
        middleView.SetCustomDataset(_exercise.Datasets[_exerciseIndex].MiddleDataset);
        rightView.SetCustomDataset(_exercise.Datasets[_exerciseIndex].RightDataset);

        leftView.ScalingFactor = ScalingFactorList[_exerciseIndex][0]; //_exercise.Datasets[_exerciseIndex].LeftDataset.ScalingFactor;
        middleView.ScalingFactor = ScalingFactorList[_exerciseIndex][1]; //_exercise.Datasets[_exerciseIndex].MiddleDataset.ScalingFactor;
        rightView.ScalingFactor = ScalingFactorList[_exerciseIndex][2]; //_exercise.Datasets[_exerciseIndex].RightDataset.ScalingFactor;

        ExerciseTitle.text = _exercise.Title;
        char subExerciseLetter = (char) (97 + _exerciseIndex);
        SubExerciseIdentifier.text = subExerciseLetter + ")";
        HeaderText.text = _exercise.Datasets[_exerciseIndex].HeaderText;
        
        
        
        leftView.UpdateView();
        
        Debug.Log("LeftDisplayLRPositions: " + leftView._displayLr.positionCount);
        
        middleView.UpdateView();
        rightView.UpdateView();
    }

    public void NextSubExercise()
    {
        if (_exerciseIndex == _exercise.Datasets.Count - 1)
        {
            int correctCount = 0;
            for (int i = 0; i < _exercise.CorrectAnswers.Count; i++)
            {
                int chosenAnswer = _exercise.ChosenAnswers[i];
                int correctAnswer = _exercise.CorrectAnswers[i];
                
                Debug.Log("Chosen: " + chosenAnswer + ", Correct: " + correctAnswer);

                if (chosenAnswer == correctAnswer) ++correctCount;
            }
            
            Debug.Log("Result: [" + correctCount + "/" + _exercise.CorrectAnswers.Count + "] correct!");
            
            return;
        }
        
        ++_exerciseIndex;
        UpdateView();
    }

    public void PreviousSubExercise()
    {
        if (_exerciseIndex == 0) return;
        
        --_exerciseIndex;
         UpdateView();
    }

    public void SetSelection(int choice)
    {
        //selectionIndex = choice;
        _exercise.ChosenAnswers[_exerciseIndex] = choice;
    }
    
}
