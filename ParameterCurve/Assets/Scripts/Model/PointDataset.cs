using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDataset
{
    public string Name = string.Empty;
    public string DisplayString = string.Empty;
    public string NotebookURL = string.Empty;

    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> worldPoints = new List<Vector3>();
    public List<float> paramValues = new List<float>();

    public List<FresnetSerretApparatus> fresnetApparatuses = new List<FresnetSerretApparatus>();
}