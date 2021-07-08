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
    public TextAsset CSV_File;
    public float PointScaleFactor = 1f;
    public bool SwapYZCoordinates = false;

    public Transform TravelObject;

    public TextMeshProUGUI IndexLabel;
    public TextMeshProUGUI TLabel;
    public TextMeshProUGUI XLabel;
    public TextMeshProUGUI YLabel;
    public TextMeshProUGUI ZLabel;

    //public float rangeStart = -2f;
    //public float rangeEnd = 4f;
    //public float numPoints = 100f;

    private List<Vector3> points;
    private List<Vector3> worldPoints;
    private List<float> paramValues;
    //private Func<float, float> current_f_Function = FCalc01;
    //private Func<float, float> current_g_Function = FCalc02;
    
    private NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

    private bool csvIs3D = false;

    public bool IsDriving = true;
    private int currentIndex = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        //current_f_Function = FCalc01;
        //current_g_Function = GCalc01;
        //displayLR = GetComponent<LineRenderer>();
        //WriteSampleCurveCoordinates(@"C:\Users\user\Desktop\curveTest\");

        if(CSV_File != null)
        {
            ImportPointsFromCSVResource();
        }

        UpdateDisplay();

        //if (IsDriving)
        //{
        //    InvokeRepeating("SetTravelPoint", 2f, 0.05f);
        //}
    }

    private void Update()
    {
        if (IsDriving)
            SetTravelPoint();
    }

    

    public void UpdateDisplay()
    {        
        if (worldPoints is null) return;

        if (DisplayLR is null)
            Debug.Log("Failed to get line renderer component");

        DisplayLR.positionCount = worldPoints.Count;
        DisplayLR.SetPositions(worldPoints.ToArray());        
    }

    public void StartRun()
    {
        currentIndex = 0;
        IsDriving = true;
        

        //InvokeRepeating("SetTravelPoint", 2f, 0.125f);
    }

    public void SetTravelPoint()
    {
        if (TravelObject is null) return;
        if (worldPoints is null) return;
        if (currentIndex < 0 ) return;
        if (currentIndex >= worldPoints.Count)
        {
            IsDriving = false;
            return;
        }

        TravelObject.position = worldPoints[currentIndex];

        IndexLabel.text = (currentIndex + 1) + " / " + points.Count;
        TLabel.text = paramValues[currentIndex].ToString("0.#####");
        XLabel.text = points[currentIndex].x.ToString("0.#####");
        YLabel.text = points[currentIndex].y.ToString("0.#####");
        ZLabel.text = points[currentIndex].z.ToString("0.#####");

        ++currentIndex;

    }


    //private void CalculatePoints()
    //{
    //    points = new List<Vector2>();
    //    worldPoints = new List<Vector3>();
    //    float increment = (rangeEnd - rangeStart) / (float)numPoints;

    //    //float yOffset = 1;

    //    //Debug.Log("Increment: " + increment);

    //    for (int i = 0; i < numPoints; i++)
    //    {
    //        float tVal = rangeStart + i * increment;
    //        //Debug.Log("i: " + i + ", tVal: " + tVal);
    //        float ft = current_f_Function(tVal);
    //        float gt = current_g_Function(tVal);
    //        points.Add(new Vector2((float)ft, (float)gt));

    //        Vector3 worldVec = new Vector3((float)ft, (float)(gt), 0f);
    //        //Debug.Log("WorldVec: " + worldVec);
    //        //Debug.Log("RootElemPos: " + RootElement.transform.TransformPoint(RootElement.transform.position));
    //        //worldVec += RootElement.transform.position;
    //        //new Vector3(-4.25f, 2f, 1.75f); //RootElement.transform.TransformPoint(RootElement.transform.position);
    //        //worldVec.x += RootElement.transform.position.x;
    //        //worldVec.x *= 0.05f;

    //        //worldVec.y += 2f;//RootElement.position.y;            
    //        //worldVec.y *= 0.05f;

    //        //worldVec.z = 2f;

    //        //Debug.Log("FinalWorldVec: " + worldVec);
    //        worldPoints.Add(worldVec);
    //    }

    //}


    //public void UpdateFFunction(int index)
    //{        
    //    switch(index)
    //    {
    //        case 0:
    //            current_f_Function = FCalc01;
    //            break;

    //        case 1:
    //            current_f_Function = FCalc02;
    //            Debug.Log("FCalc02");
    //            break;

    //        case 2:
    //            current_f_Function = FCalc03;
    //            break;
    //    }
    //}


    //private static float FCalc01(float t)
    //{
    //    return (t * t) - 2f * t;
    //}

    //private static float FCalc02(float t)
    //{
    //    return Mathf.Sin(t);
    //}

    //private static float FCalc03(float t)
    //{
    //    return Mathf.Sin(t);
    //}



    //public void UpdateGunction(int index)
    //{
    //    switch (index)
    //    {
    //        case 0:
    //            current_g_Function = GCalc01;
    //            break;

    //        case 1:
    //            current_g_Function = GCalc02;
    //            Debug.Log("GCalc02");
    //            break;

    //        case 2:
    //            current_g_Function = GCalc03;
    //            break;
    //    }
    //}

    //private static float GCalc01(float t)
    //{
    //    return t + 1;
    //}

    //private static float GCalc02(float t)
    //{
    //    return Mathf.Cos(t);
    //}

    //private static float GCalc03(float t)
    //{
    //    return 0.5f * (1f - Mathf.Cos(2 * t));
    //}

    //private void WriteSampleCurveCoordinates(string folderPath)
    //{
    //    int fileCount = 3;
    //    string[] fileNames = new string[] {
    //        "01_BaseCurve.csv",
    //        "02_CosineSinusCircle.csv",
    //        "03_SinSquaredSin.csv"
    //    };

    //    Func<float, float>[] fFuncArr = new Func<float, float>[]
    //    {
    //        FCalc01,
    //        FCalc02,
    //        FCalc03
    //    };

    //    Func<float, float>[] gFuncArr = new Func<float, float>[]
    //    {
    //        GCalc01,
    //        GCalc02,
    //        GCalc03
    //    };

    //    Vector2[] rangeArr = new Vector2[]
    //    {
    //        new Vector2(-2f, 4f),
    //        new Vector2(0f, 2 * Mathf.PI),
    //        new Vector2(-Mathf.PI, Mathf.PI)
    //    };

    //    StringBuilder sb = new StringBuilder();
        

    //    for(int i = 0; i < fileCount; i++)
    //    {
    //        current_f_Function = fFuncArr[i];
    //        current_g_Function = gFuncArr[i];
    //        //rangeStart = rangeArr[i].x;
    //        //rangeEnd = rangeArr[i].y;
    //        //CalculatePoints();

    //        sb.AppendLine("x,y");
    //        for (int j = 0; j < points.Count; j++)
    //        {
    //            Vector2 point = points[j];
    //            sb.AppendLine(point.x.ToString(nfi) + "," + point.y.ToString(nfi));
    //        }

    //        //File basef = new File(folderPath + "01_basecurve.csv", FileMode.Create, FileAccess.Write, FileShare.Read);
    //        using (var w = new StreamWriter(folderPath + fileNames[i], false))
    //        {
    //            w.Write(sb.ToString());
    //            w.Flush();
    //        }

    //        sb.Clear();
    //    }

        


    //    //var baseCsv = Resources.Load("csv/01_basecurve.csv");
    //    //baseCsv.

    //}

    public void ImportPointsFromCSVResource()
    {
        points = new List<Vector3>();
        worldPoints = new List<Vector3>();
        paramValues = new List<float>();

        var lineArr = CSV_File.text.Split('\n'); //Regex.Split(textfile.text, "\n|\r|\r\n");

        //for(int i = 0; i < lineArr.Length; i++)
        //{
        //    Debug.Log("LineArr[" + i + "]: " + lineArr[i]);
        //}



        //using(var sr = new StreamReader(Application.dataPath + "/Resources/csv/" + resourceName))
        //{
        //    string line = sr.ReadLine();
        //   // Debug.Log("Line[" + i + "]: " + line);
        //    var values = line.Split(',');
        //    float x = float.Parse(values[0], nfi);
        //    float y = float.Parse(values[1], nfi);
        //    points.Add(new Vector2(x, y));
        //    worldPoints.Add(new Vector3(x, y, 0f));
        //}

        if (lineArr[0].Split(',').Length == 4) csvIs3D = true;

        //Skip header
        for (int i = 1; i < lineArr.Length; i++)
        {
            string line = lineArr[i];
            if (string.IsNullOrEmpty(line)) continue;
            //Debug.Log("Line[" + i + "]: " + line);
            string[] values = line.Split(',');
            //Debug.Log("val0: " + values[0]);
            //Debug.Log("val1: " + values[1]);
            float t = float.Parse(values[0], nfi);
            float x = float.Parse(values[1], nfi);
            float y = float.Parse(values[2], nfi);
            float z = csvIs3D ? float.Parse(values[3], nfi) : 0f;
            points.Add(new Vector3(x, y, z));
            worldPoints.Add(SwapYZCoordinates ?
                new Vector3(x, z, y) * PointScaleFactor :  
                new Vector3(x, y, z) * PointScaleFactor);
            paramValues.Add(t);

        }

        UpdateDisplay();


    }


}
