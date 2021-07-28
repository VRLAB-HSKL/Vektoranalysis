using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDataset
{
    public string Name = string.Empty;
    public string DisplayString = string.Empty;
    public string NotebookURL = string.Empty;
    public Texture2D MenuButtonImage;


    public float Distance = 0f;

    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> worldPoints = new List<Vector3>();
    public List<float> paramValues = new List<float>();

    public List<Vector2> timeDistancePoints { get; set; } = new List<Vector2>();
    public List<Vector2> timeVelocityPoints = new List<Vector2>();

    public List<FresnetSerretApparatus> fresnetApparatuses = new List<FresnetSerretApparatus>();


    public List<Vector3> arcLenghtPoints = new List<Vector3>();
    public List<Vector3> arcLengthWorldPoints = new List<Vector3>();
    public List<float> arcLengthParamValues = new List<float>();

    public List<FresnetSerretApparatus> arcLengthFresnetApparatuses = new List<FresnetSerretApparatus>();


}