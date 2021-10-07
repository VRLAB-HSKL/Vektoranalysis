using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDataset
{
    public string Name = string.Empty;
    public string DisplayString = string.Empty;
    public string NotebookURL = string.Empty;
    public Texture2D MenuButtonImage { get; set; }

    
    
    

    public float Distance = 0f;

    public bool Is3DCurve { get; set; }

    
    public float WorldScalingFactor { get; set; }
    public float TableScalingFactor { get; set; }
    public float SelectExercisePillarScalingFactor { get; set; }
    
    
    
    private List<Vector3> ps = new List<Vector3>();

    public List<Vector3> points
    {
        get => ps;
        set
        {
            ps = value;
            
            CalculateWorldPoints();
        }
    }
    
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


    public void CalculateWorldPoints()
    {
        // Calculate world points
        worldPoints.Clear();
        for (int i = 0; i < ps.Count; i++)
        {
            var point = ps[i];

            bool swapYZCoordinates = Is3DCurve;
                
            worldPoints.Add(swapYZCoordinates ?
                new Vector3(point.x, point.z, point.y) * GlobalData.PointScaleFactor :
                new Vector3(point.x, point.y, point.z) * GlobalData.PointScaleFactor);
        }
    }
}