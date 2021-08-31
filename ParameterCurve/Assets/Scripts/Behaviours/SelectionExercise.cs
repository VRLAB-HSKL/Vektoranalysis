using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionChoice { None = 0, LeftPillar = 1, MiddlePillar = 2, RightPillar = 3 }

public class SelectionExercise
{
    public string Title = string.Empty;
    public int NumberOfSubExercises;

    public List<SelectionChoice> CorrectAnswers;
    public List<SelectionChoice> ChosenAnswers;

    public SelectionExercise(string title, List<SelectionChoice> correctAnswers)
    {
        Title = title;
        NumberOfSubExercises = correctAnswers.Count;
        CorrectAnswers = correctAnswers;

        ChosenAnswers = new List<SelectionChoice>();
        for(int i = 0; i < NumberOfSubExercises; i++)
        {
            ChosenAnswers.Add(SelectionChoice.None);
        }        
    }

}
