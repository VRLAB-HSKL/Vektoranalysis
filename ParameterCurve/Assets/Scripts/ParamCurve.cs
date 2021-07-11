using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
//using System.Text.Json;

using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;


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

    private LineRenderer TangentLR;
    private Vector3[] tangentArr = new Vector3[2];

    private LineRenderer NormalLR;
    private Vector3[] normalArr = new Vector3[2];

    private LineRenderer BinormalLR;
    private Vector3[] binormalArr = new Vector3[2];

    //private bool csvIs3D = false;






    // Start is called before the first frame update
    void Start()
    {
        pointStepDuration = (1f / 60f) * PointScaleFactor;

        InitTravelObjPos = TravelObject.position;
        if(TravelObject.childCount > 0)
        {
            GameObject firstChild = TravelObject.GetChild(0).gameObject;
            TangentLR = firstChild.GetComponent<LineRenderer>();
            TangentLR.positionCount = 2;
        }

        if (TravelObject.childCount > 1)
        {
            GameObject secondChild = TravelObject.GetChild(1).gameObject;
            NormalLR = secondChild.GetComponent<LineRenderer>();
            NormalLR.positionCount = 2;
        }

        if (TravelObject.childCount > 2)
        {
            GameObject thirdChild = TravelObject.GetChild(2).gameObject;
            BinormalLR = thirdChild.GetComponent<LineRenderer>();
            BinormalLR.positionCount = 2;
        }


        ImportAllResources();

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

        int pointIndex = currentPointIndex;

        TravelObject.position = datasets[currentDataSetIndex].worldPoints[pointIndex];
        
        tangentArr[0] = TravelObject.position;
        tangentArr[1] = TravelObject.position + datasets[currentDataSetIndex].fresnetApparatuses[pointIndex].Tangent;
        TangentLR.SetPositions(tangentArr);

        normalArr[0] = TravelObject.position;
        normalArr[1] = TravelObject.position + datasets[currentDataSetIndex].fresnetApparatuses[pointIndex].Normal;
        NormalLR.SetPositions(normalArr);

        binormalArr[0] = TravelObject.position;
        binormalArr[1] = TravelObject.position + datasets[currentDataSetIndex].fresnetApparatuses[pointIndex].Binormal;
        BinormalLR.SetPositions(binormalArr);
        

        IndexLabel.text = (pointIndex + 1) + 
            " / " + 
            datasets[currentDataSetIndex].points.Count;

        SourceLabel.text = datasets[currentDataSetIndex].Name;
        TLabel.text = datasets[currentDataSetIndex].paramValues[pointIndex].ToString("0.#####");
        XLabel.text = datasets[currentDataSetIndex].points[pointIndex].x.ToString("0.#####");
        YLabel.text = datasets[currentDataSetIndex].points[pointIndex].y.ToString("0.#####");
        ZLabel.text = datasets[currentDataSetIndex].points[pointIndex].z.ToString("0.#####");

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

            
            FresnetSerretApparatus fsa = new FresnetSerretApparatus();
            fsa.Tangent = Vector3.zero;
            pd.fresnetApparatuses.Add(fsa);
        }       

        return pd;
    }

    private PointDataset ImportFromJSONResource(TextAsset json)
    {
        Debug.Log("TxtAsset_Text: " + json.text);

        JsonRoot jsr = JsonConvert.DeserializeObject<JsonRoot>(json.text); 
            //JsonUtility.FromJson<JsonRoot>(json.text); //JsonSerializer.Deserialize<JsonRoot>(txt.text);

        //Debug.Log("JsonName: " + jsr.name);

        PointDataset pds = new PointDataset();
        pds.Name = jsr.name + "_JSON";



        bool swapYZCoordinates = false;

        for (int i = 0; i < jsr.pointData.Count; i++)
        {
            PointData pd = jsr.pointData[i];
            float t = float.Parse(pd.t, nfi);
            float x = float.Parse(pd.x, nfi);
            float y = float.Parse(pd.y, nfi);
            float z = 0f;

            float tx = float.Parse(pd.tan[0], nfi);
            float ty = float.Parse(pd.tan[1], nfi);
            float tz = float.Parse(pd.tan[2], nfi);

            float nx = float.Parse(pd.norm[0], nfi);
            float ny = float.Parse(pd.norm[1], nfi);
            float nz = float.Parse(pd.norm[2], nfi);

            float bnx = float.Parse(pd.binorm[0], nfi);
            float bny = float.Parse(pd.binorm[1], nfi);
            float bnz = float.Parse(pd.binorm[2], nfi);

            //float t = float.Parse(pd.t, nfi);

            pds.points.Add(new Vector3(x, y, z));
            pds.worldPoints.Add(swapYZCoordinates ?
                new Vector3(x, z, y) * PointScaleFactor :
                new Vector3(x, y, z) * PointScaleFactor);
            pds.paramValues.Add(t);

            FresnetSerretApparatus fsp = new FresnetSerretApparatus();
            fsp.Tangent = swapYZCoordinates ? 
                new Vector3(tx, tz, ty) * PointScaleFactor :
                new Vector3(tx, ty, tz) * PointScaleFactor;

            fsp.Normal = swapYZCoordinates ?
                new Vector3(nx, nz, ny) * PointScaleFactor :
                new Vector3(nx, ny, nz) * PointScaleFactor;

            fsp.Binormal = swapYZCoordinates ?
                new Vector3(bnx, bnz, bny) * PointScaleFactor :
                new Vector3(bnx, bny, bnz) * PointScaleFactor;

            pds.fresnetApparatuses.Add(fsp);
        }

        return pds;
    }

    private void ImportAllResources()
    {
        // CSV Resources
        UnityEngine.Object[] csv_resources = Resources.LoadAll("csv/exercises/", typeof(TextAsset));

        for(int i = 0; i < csv_resources.Length; i++)
        {
            datasets.Add(ImportPointsFromCSVResource(csv_resources[i] as TextAsset));
        }

        // JSON Resources
        UnityEngine.Object[] json_resources = Resources.LoadAll("json/exercises/", typeof(TextAsset));

        for (int i = 0; i < json_resources.Length; i++)
        {
            datasets.Add(ImportFromJSONResource(json_resources[i] as TextAsset));
        }
    }    


}

public class PointDataset
{
    public string Name = string.Empty;    

    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> worldPoints = new List<Vector3>();
    public List<float> paramValues = new List<float>();
    // public List<Vector3> velocityVectors = new List<Vector3>();
    public List<FresnetSerretApparatus> fresnetApparatuses = new List<FresnetSerretApparatus>();
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

[Serializable]
public class JsonRoot
{
    public string name { get; set; }
    public List<PointData> pointData { get; set; } = new List<PointData>();
}


[Serializable]
public class PointData
{
    public string t { get; set; }
    public string x { get; set; }
    public string y { get; set; }
    public List<string> tan { get; set; } = new List<string>();
    public List<string> norm { get; set; } = new List<string>();
    public List<string> binorm { get; set; } = new List<string>();
}



