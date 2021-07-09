using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ParamCurve : MonoBehaviour
{
    public GameObject RootElement;
    public LineRenderer DisplayLR;
    //public TextAsset CSV_File;
    public float PointScaleFactor = 1f;
    public float RunSpeedFactor = 1f;
    
    //public bool SwapYZCoordinates = false;

    public Transform TravelObject;

    public TextMeshProUGUI SourceLabel;
    public TextMeshProUGUI IndexLabel;
    public TextMeshProUGUI TLabel;
    public TextMeshProUGUI XLabel;
    public TextMeshProUGUI YLabel;
    public TextMeshProUGUI ZLabel;



    public bool IsDriving = true;

    private List<PointDataset> datasets = new List<PointDataset>();
    private int currentDataSetIndex = 2;
    private int currentPointIndex = 0;

    private NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

    private Vector3 InitTravelObjPos;

    private float pointStepDuration = 0f;

    //private bool csvIs3D = false;

    
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        pointStepDuration = (1f / 60f) * PointScaleFactor;

        InitTravelObjPos = TravelObject.position;

        ImportAllCSVResources();

        UpdateWorldObjects();
    }

    private float updateTimer = 0f;

    private void Update()
    {
        if (IsDriving)
        {
            updateTimer += Time.deltaTime;
            if(updateTimer >= pointStepDuration)
            {
                updateTimer = 0f;
                SetTravelPointAndDisplay();                
            }
            
            
        }
            
    }

    public void UpdateLineRenderer()
    {        
        if (datasets[currentDataSetIndex].worldPoints is null) return;

        if (DisplayLR is null)
            Debug.Log("Failed to get line renderer component");

        DisplayLR.positionCount = datasets[currentDataSetIndex].worldPoints.Count;
        DisplayLR.SetPositions(datasets[currentDataSetIndex].worldPoints.ToArray());        
    }

    public void StartRun()
    {
        currentPointIndex = 0;
        IsDriving = true;        
    }

    public void SwitchToNextDataset()
    {
        // Stop driving
        if(IsDriving)
        {
            IsDriving = false;            
        }

        // Increment data set index, reset to 0 on overflow
        ++currentDataSetIndex;
        if (currentDataSetIndex >= datasets.Count)
            currentDataSetIndex = 0;

        // Reset point index
        currentPointIndex = 0;

        UpdateWorldObjects();
    }

    private void UpdateWorldObjects()
    {
        UpdateLineRenderer();
        SetTravelPointAndDisplay();
    }

    private void SetTravelPointAndDisplay()
    {
        // Null checks
        if (TravelObject is null) return;
        if (datasets[currentDataSetIndex].worldPoints is null) return;
        if (currentPointIndex < 0 ) return;
        
        // On arrival at the last point, stop driving
        if (currentPointIndex >= datasets[currentDataSetIndex].worldPoints.Count)
        {
            IsDriving = false;
            return;
        }

        TravelObject.position = datasets[currentDataSetIndex].worldPoints[currentPointIndex];

        IndexLabel.text = (currentPointIndex + 1) + 
            " / " + 
            datasets[currentDataSetIndex].points.Count;

        SourceLabel.text = datasets[currentDataSetIndex].name;
        TLabel.text = datasets[currentDataSetIndex].paramValues[currentPointIndex].ToString("0.#####");
        XLabel.text = datasets[currentDataSetIndex].points[currentPointIndex].x.ToString("0.#####");
        YLabel.text = datasets[currentDataSetIndex].points[currentPointIndex].y.ToString("0.#####");
        ZLabel.text = datasets[currentDataSetIndex].points[currentPointIndex].z.ToString("0.#####");

        ++currentPointIndex;
    }


    private PointDataset ImportPointsFromCSVResource(TextAsset txt)
    {
        bool swapYZCoordinates = false;

        PointDataset pd = new PointDataset();
        pd.name = txt.name;
        pd.points = new List<Vector3>();
        pd.worldPoints = new List<Vector3>();
        pd.paramValues = new List<float>();

        string[] lineArr = txt.text.Split('\n'); //Regex.Split(textfile.text, "\n|\r|\r\n");

        //Debug.Log("SplitLength: " + lineArr[0].Split(',').Length);
        //if (lineArr[0].Split(',').Length == 4) csvIs3D = true;
        //Debug.Log("csvIs3D: " + ((csvIs3D == true) ? "true" : "false"));

        //Skip header
        for (int i = 1; i < lineArr.Length; i++)
        {
            string line = lineArr[i];
            if (string.IsNullOrEmpty(line)) continue;
            
            string[] values = line.Split(',');
            //Debug.Log("valuesArrSize: " + values.Length);
            float t = float.Parse(values[0], nfi);
            float x = float.Parse(values[1], nfi);
            float y = float.Parse(values[2], nfi);
            float z = 0f;
            if(values.Length == 4)
            {
                swapYZCoordinates = true; //Debug.Log("csvIs3dValuesSize: " + values.Length);
                z = float.Parse(values[3], nfi);
            }
            pd.points.Add(new Vector3(x, y, z));
            pd.worldPoints.Add(swapYZCoordinates ?
                new Vector3(x, z, y) * PointScaleFactor :  
                new Vector3(x, y, z) * PointScaleFactor);
            pd.paramValues.Add(t);

        }       

        return pd;
    }

    private void ImportAllCSVResources()
    {
        UnityEngine.Object[] ressources = Resources.LoadAll("csv/exercises/", typeof(TextAsset));

        for(int i = 0; i < ressources.Length; i++)
        {
            datasets.Add(ImportPointsFromCSVResource(ressources[i] as TextAsset));
        }

    }    


}

public class PointDataset
{
    public string name;    

    public List<Vector3> points;
    public List<Vector3> worldPoints;
    public List<float> paramValues;


}
