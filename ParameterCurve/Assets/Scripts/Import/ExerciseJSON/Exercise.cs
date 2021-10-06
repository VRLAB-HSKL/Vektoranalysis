
using System;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ExerciseRoot
{
    public Exercise exercise { get; set; }
}

[Serializable]
public class Exercise
{
    public string id { get; set; } = string.Empty;
    public string title { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    
    public List<Subexercise> subExercises = new List<Subexercise>();

    public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("id: " + id);
        sb.AppendLine("title: " + title);
        sb.AppendLine("type: " + type);
        sb.AppendLine("description: " + description);
        sb.AppendLine("subExercises:");

        for (int i = 0; i < subExercises.Count; i++)
        {
            sb.AppendLine(subExercises[i].ToString());
        }

        return sb.ToString();
    }
}

[Serializable]
public class Subexercise
{
    public string title { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public int correctAnswer { get; set; } = -1;
    
    public CurveData leftCurveData = new CurveData();
    public CurveData middleCurveData = new CurveData();
    public CurveData rightCurveData = new CurveData();

    public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("title: " + title);
        sb.AppendLine("description: " + description);
        sb.AppendLine("correctAnswer: " + correctAnswer);

        return sb.ToString();
    }
}

[Serializable]
public class CurveData
{
    public int dim { get; set; } = 0;

    public float arcLength { get; set; } = 0f;
    
    public List<PointDataJSON> data { get; set; } = new List<PointDataJSON>();
}

[Serializable]
public class PointDataJSON
{
    public float t { get; set; } = 0f;
    public List<float> pVec { get; set; } = new List<float>();
    public List<float> velVec { get; set; } = new List<float>();
    public List<float> accVec { get; set; } = new List<float>();
    
    public List<float> arcPVec { get; set; } = new List<float>();
    public List<float> arcVelVec { get; set; } = new List<float>();
    public List<float> arcAccVec { get; set; } = new List<float>();
}