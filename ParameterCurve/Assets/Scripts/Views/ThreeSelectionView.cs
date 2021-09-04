using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSelectionView : MonoBehaviour
{
    public Vector3 PillarOffset = Vector3.right;
    public Vector3 CurveOffset = Vector3.zero;
    public float ScalingFactor = 0.25f;

    public Material CurveLineMat;

    AbstractCurveView leftView;
    AbstractCurveView middleView;
    AbstractCurveView rightView;

    public List<SelectionExercise> Exercises;

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
        Exercises = new List<SelectionExercise>();

        

        var leftPds = GlobalData.ParamCurveDatasets[0];
        var middlePds = GlobalData.ParamCurveDatasets[1];
        var rightPds = GlobalData.ParamCurveDatasets[2];

        ExercisePointDataset exercPds = new ExercisePointDataset(leftPds, middlePds, rightPds);

        var exercPdsList = new List<ExercisePointDataset>();
        exercPdsList.Add((exercPds));

        SelectionChoice sel01 = SelectionChoice.MiddlePillar;
        var selChoiceList = new List<SelectionChoice>();
        
        SelectionExercise slexerc = new SelectionExercise(
            "TestExercise",
                exercPdsList,
                selChoiceList
            );

        List<SelectionExercise> lst = new List<SelectionExercise>();
        lst.Add(slexerc);

        Exercises = lst;

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
        leftView.SetCustomDataset(Exercises[0].Datasets[0].LeftDataset);

        middleView = new SimpleCurveView(middleLR, middlePillar.transform.position + CurveOffset, ScalingFactor);
        middleView.SetCustomDataset(Exercises[0].Datasets[0].MiddleDataset);
        
        rightView = new SimpleCurveView(rightLR, rightPillar.transform.position + CurveOffset, ScalingFactor);
        rightView.SetCustomDataset(Exercises[0].Datasets[0].RightDataset);
    }


    
}
