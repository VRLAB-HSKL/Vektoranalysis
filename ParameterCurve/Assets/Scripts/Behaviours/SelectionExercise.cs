using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionChoice { None = 0, LeftPillar = 1, MiddlePillar = 2, RightPillar = 3 }

public class SelectionExercise
{
    public string Title = string.Empty;
    public int NumberOfSubExercises;

    public List<ExercisePointDataset> Datasets;
    
    public List<SelectionChoice> CorrectAnswers;
    public List<SelectionChoice> ChosenAnswers;

    public SelectionExercise(string title, List<ExercisePointDataset> exercisePointDatasets , List<SelectionChoice> correctAnswers)
    {
        Title = title;
        NumberOfSubExercises = correctAnswers.Count;

        Datasets = exercisePointDatasets;
        CorrectAnswers = correctAnswers;
        ChosenAnswers = new List<SelectionChoice>();
        for(int i = 0; i < NumberOfSubExercises; i++)
        {
            ChosenAnswers.Add(SelectionChoice.None);
        }        
    }

}

public class ExercisePointDataset
{
    public PointDataset LeftDataset { get; set; }
    public PointDataset MiddleDataset { get; set; }
    public PointDataset RightDataset { get; set; }

    public ExercisePointDataset(PointDataset leftDataset, PointDataset middleDataset, PointDataset rightDataset)
    {
        LeftDataset = leftDataset;
        MiddleDataset = middleDataset;
        RightDataset = rightDataset;
    }
}

