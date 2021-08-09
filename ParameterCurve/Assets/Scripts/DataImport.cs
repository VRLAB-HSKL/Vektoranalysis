using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class DataImport
{
    public static float TimeDistanceXAxisLength { get; set; }
    public static float TimeDistanceYAxisLength;

    public static float TimeVelocityXAxisLength;
    public static float TimeVelocityYAxisLength;


    private static NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

    public static PointDataset ImportPointsFromCSVResource(TextAsset txt)
    {
        bool swapYZCoordinates = false;

        PointDataset pd = new PointDataset();
        pd.Name = txt.name;
        pd.DisplayString = txt.name;
        pd.NotebookURL = GlobalData.LocalHTMLResourcePath + txt.name + ".html";

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
            if (values.Length == 4)
            {
                swapYZCoordinates = true; //Debug.Log("csvIs3dValuesSize: " + values.Length);
                z = float.Parse(values[3], nfi);
            }
            pd.points.Add(new Vector3(x, y, z));
            pd.worldPoints.Add(swapYZCoordinates ?
                new Vector3(x, z, y) * GlobalData.PointScaleFactor :
                new Vector3(x, y, z) * GlobalData.PointScaleFactor);
            pd.paramValues.Add(t);


            FresnetSerretApparatus fsa = new FresnetSerretApparatus();
            fsa.Tangent = Vector3.zero;
            pd.fresnetApparatuses.Add(fsa);
        }

        return pd;
    }

    public static PointDataset ImportFromJSONResource(TextAsset json)
    {
        JsonRoot jsr = JsonConvert.DeserializeObject<JsonRoot>(json.text);

        PointDataset pds = new PointDataset();
        pds.Name = jsr.name + "_JSON";
        pds.DisplayString = jsr.name;
        pds.NotebookURL = GlobalData.LocalHTMLResourcePath + jsr.name + ".html";

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
                new Vector3(x, z, y) * GlobalData.PointScaleFactor :
                new Vector3(x, y, z) * GlobalData.PointScaleFactor);
            pds.paramValues.Add(t);

            FresnetSerretApparatus fsp = new FresnetSerretApparatus();
            fsp.Tangent = swapYZCoordinates ?
                new Vector3(tx, tz, ty) * GlobalData.PointScaleFactor :
                new Vector3(tx, ty, tz) * GlobalData.PointScaleFactor;

            fsp.Normal = swapYZCoordinates ?
                new Vector3(nx, nz, ny) * GlobalData.PointScaleFactor :
                new Vector3(nx, ny, nz) * GlobalData.PointScaleFactor;

            fsp.Binormal = swapYZCoordinates ?
                new Vector3(bnx, bnz, bny) * GlobalData.PointScaleFactor :
                new Vector3(bnx, bny, bnz) * GlobalData.PointScaleFactor;

            pds.fresnetApparatuses.Add(fsp);
        }

        return pds;
    }

    public static PointDataset CreateDatasetFormLocalCalculation(AbstractCurveCalc curveCalc)
    {
        // Local Calculation
        PointDataset pdsa = new PointDataset
        {
            Name = curveCalc.Name,
            DisplayString = curveCalc.DisplayString,
            NotebookURL = GlobalData.LocalHTMLResourcePath + curveCalc.Name + ".html"
        };

        string imgResPath = GlobalData.ImageResourcePath + curveCalc.Name;
        Texture2D imgRes = Resources.Load(imgResPath) as Texture2D;

        if (imgRes != null)
        {
            pdsa.MenuButtonImage = imgRes;
        }

        pdsa.paramValues = new List<float>(curveCalc.ParameterIntervall);
        pdsa.points = curveCalc.CalculatePoints();

        for (int i = 0; i < pdsa.points.Count; i++)
        {
            Vector3 pv = pdsa.points[i];
            pdsa.worldPoints.Add(curveCalc.Is3DCurve ?
                new Vector3(pv.x, pv.z, pv.y) * GlobalData.PointScaleFactor :
                pv * GlobalData.PointScaleFactor);
        }

        pdsa.fresnetApparatuses = curveCalc.CalculateFresnetApparatuses();
        pdsa.timeDistancePoints = curveCalc.CalculateTimeDistancePoints();        

        for(int i = 0; i < pdsa.timeDistancePoints.Count; i++)
        {
            Vector2 p = pdsa.timeDistancePoints[i];
            p.x *= TimeDistanceXAxisLength;
            p.y *= TimeDistanceYAxisLength;            
            pdsa.timeDistancePoints[i] = new Vector2(p.x, p.y);
        }

        pdsa.timeVelocityPoints = curveCalc.CalculateTimeVelocityPoints();

        for(int i = 0; i < pdsa.timeVelocityPoints.Count; i++)
        {
            Vector2 p = pdsa.timeVelocityPoints[i];
            p.x *= TimeVelocityXAxisLength;
            p.y *= TimeVelocityYAxisLength;
            pdsa.timeVelocityPoints[i] = new Vector2(p.x, p.y);            
        }

        pdsa.Distance = AbstractCurveCalc.CalculateRawDistance(pdsa.points);

        // Calculate arc length param valus
        //var initParamIntervall = curveCalc.ParameterIntervall;
        
        pdsa.arcLengthParamValues = curveCalc.CalculateArcLengthParamRange();
        //curveCalc.ParameterIntervall = pdsa.arcLengthParamValues;
        
        pdsa.arcLength = pdsa.arcLengthParamValues[pdsa.arcLengthParamValues.Count - 1];      
        pdsa.arcLenghtPoints = curveCalc.CalculateArcLengthParameterizedPoints(); //curveCalc.CalculatePoints();

        for (int i = 0; i < pdsa.arcLenghtPoints.Count; i++)
        {
            Vector3 pv = pdsa.arcLenghtPoints[i];
            pdsa.arcLengthWorldPoints.Add(curveCalc.Is3DCurve ?
                new Vector3(pv.x, pv.z, pv.y) * GlobalData.PointScaleFactor :
                pv * GlobalData.PointScaleFactor);
        }

        pdsa.arcLengthFresnetApparatuses = curveCalc.CalculateFresnetApparatuses();

        //curveCalc.ParameterIntervall = initParamIntervall;

        return pdsa;
    }
}
