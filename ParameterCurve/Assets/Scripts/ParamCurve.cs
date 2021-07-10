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
    private int currentDataSetIndex = 0;
    private int currentPointIndex = 0;

    private NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

    private Vector3 InitTravelObjPos;

    private float pointStepDuration = 0f;

    private LineRenderer VelocityLR;
    private Vector3[] velocityArr = new Vector3[2];

    //private bool csvIs3D = false;






    // Start is called before the first frame update
    void Start()
    {
        pointStepDuration = (1f / 60f) * PointScaleFactor;

        InitTravelObjPos = TravelObject.position;
        if(TravelObject.childCount > 0)
        {
            GameObject firstChild = TravelObject.GetChild(0).gameObject;
            VelocityLR = firstChild.GetComponent<LineRenderer>();
            VelocityLR.positionCount = 2;
        }


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

    public void SwitchToPreviousDataset()
    {
        // Stop driving
        if (IsDriving)
        {
            IsDriving = false;
        }

        // Decrement data set index, reset to last element on negative index
        --currentDataSetIndex;
        if (currentDataSetIndex < 0)
            currentDataSetIndex = datasets.Count - 1;

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
        velocityArr[0] = TravelObject.position;
        velocityArr[1] = TravelObject.position + datasets[currentDataSetIndex].velocityVectors[currentPointIndex];
        VelocityLR.SetPositions(velocityArr);

        IndexLabel.text = (currentPointIndex + 1) + 
            " / " + 
            datasets[currentDataSetIndex].points.Count;

        SourceLabel.text = datasets[currentDataSetIndex].Name;
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
        pd.Name = txt.name;

        string[] lineArr = txt.text.Split('\n'); //Regex.Split(textfile.text, "\n|\r|\r\n");

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

            //int rndIdx = i % 4;

            //switch(rndIdx)
            //{
            //    case 0:
            //        velVec = Vector3.up;
            //        break;

            //    case 1:
            //        velVec = Vector3.right;
            //        break;

            //    case 2:
            //        velVec = Vector3.down;
            //        break;

            //    case 3:
            //        velVec = Vector3.left;
            //        break;
            //}

            Vector3 velVec = Vector3.zero;

            
            // On first point, velocity is zero, so only calculate points after that
            if(i > 0)
            {
                
            }


            pd.velocityVectors.Add(velVec);
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
    public string Name = string.Empty;    

    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> worldPoints = new List<Vector3>();
    public List<float> paramValues = new List<float>();
    public List<Vector3> velocityVectors = new List<Vector3>();

}

public class FresnetSerretApparatus
{
    /// <summary>
    /// Unit vector tangent to the curve, pointing in the direction of motion
    /// Source: Fresnet-Serret formulas (Wikipedia)
    /// 
    ///           r'(t)
    /// T(t) = -----------
    ///         ||r'(t)||
    /// 
    /// </summary> 
    public Vector3 Tangent = Vector3.zero;

    /// <summary>
    /// Normal unit vector, derivative of Tangent with respect to the arclength
    /// parameter of the curve, divided by its length (normalized)
    /// Source: Fresnet-Serret formulas (Wikipedia)
    /// 
    ///           T'(t)
    /// N(t) = -----------
    ///         ||T'(t)||
    ///
    /// </summary> 
    public Vector3 Normal = Vector3.zero;

    /// <summary>
    /// Binormal unit vector, crossproduct of Tangent and Normal
    /// Source: Fresnet-Serret formulas (Wikipedia)
    /// 
    /// B(t) = T x N
    /// 
    /// </summary> 
    public Vector3 Binormal = Vector3.zero;

    public float Curvature;

    public float Torsion;

}
