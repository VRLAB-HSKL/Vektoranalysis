using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum SelectionChoice { None = 0, LeftPillar = 1, MiddlePillar = 2, RightPillar = 3 }

public class SelectionExercise
{
    public string Title = string.Empty;
    public int NumberOfSubExercises;

    public List<ExercisePointDataset> Datasets;
    
    public List<int> CorrectAnswers;
    public List<int> ChosenAnswers;

    public SelectionExercise(string title, List<ExercisePointDataset> exercisePointDatasets , List<int> correctAnswers)
    {
        Title = title;
        NumberOfSubExercises = correctAnswers.Count;

        Datasets = exercisePointDatasets;
        CorrectAnswers = correctAnswers;
        ChosenAnswers = new List<int>();
        for(int i = 0; i < NumberOfSubExercises; i++)
        {
            ChosenAnswers.Add(-1);
        }        
    }

}

public class ExercisePointDataset
{
    public string HeaderText { get; set; }
    public PointDataset LeftDataset { get; set; }
    public PointDataset MiddleDataset { get; set; }
    public PointDataset RightDataset { get; set; }

    public ExercisePointDataset(string headerText, PointDataset leftDataset, PointDataset middleDataset, PointDataset rightDataset)
    {
        HeaderText = headerText;
        LeftDataset = leftDataset;
        MiddleDataset = middleDataset;
        RightDataset = rightDataset;
    }
}

