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

    public List<Vector3> points { get; set; } = new List<Vector3>();
    public List<Vector3> worldPoints { get; set; } = new List<Vector3>();
    public List<float> paramValues { get; set; } = new List<float>();

    public float ScalingFactor = 1f;

    public List<Vector2> timeDistancePoints { get; set; } = new List<Vector2>();
    public List<Vector2> timeVelocityPoints { get; set; } = new List<Vector2>();

    public List<FresnetSerretApparatus> fresnetApparatuses { get; set; } = new List<FresnetSerretApparatus>();

    public float arcLength { get; set; } = 0f;
    public List<Vector3> arcLenghtPoints { get; set; } = new List<Vector3>();
    public List<Vector3> arcLengthWorldPoints { get; set; } = new List<Vector3>();
    public List<float> arcLengthParamValues { get; set; } = new List<float>();

    public List<FresnetSerretApparatus> arcLengthFresnetApparatuses { get; set; } = new List<FresnetSerretApparatus>();


}