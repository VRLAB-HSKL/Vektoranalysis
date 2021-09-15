
using System;
using System.Collections.Generic;


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
}

[Serializable]
public class Subexercise
{
    public List<PointCurveDataJSON> leftCurveData = new List<PointCurveDataJSON>();
    public List<PointCurveDataJSON> middleCurveData = new List<PointCurveDataJSON>();
    public List<PointCurveDataJSON> rightCurveData = new List<PointCurveDataJSON>();
    public int correctAnswer { get; set; } = -1;
    public string description { get; set; } = string.Empty;
}

[Serializable]
public class PointCurveDataJSON
{
    public float t { get; set; } = 0f;
    public List<float> pVec { get; set; } = new List<float>();
    public List<float> velVec { get; set; } = new List<float>();
    public List<float> accVec { get; set; } = new List<float>();
}

// [Serializable]
// public class LeftCurveData : AbstractCurveDataJSON {}
//
// [Serializable]
// public class MiddleCurveData : AbstractCurveDataJSON {}
//
// [Serializable]
// public class RightCurveData : AbstractCurveDataJSON {} 