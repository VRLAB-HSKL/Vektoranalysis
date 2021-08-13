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
using UnityEngine.Events;

public class WorldStateController : MonoBehaviour
{
    public GameObject RootElement;
    public LineRenderer DisplayLR;
    public Transform TravelObject;
    public Transform ArcLengthTravelObject;

    public BrowserControl BrowserWall;
    public InformationControl InfoWall;
    public CurveSelectionControl CurveSelectWall;

    private Vector3 InitTravelObjPos;
    private Vector3 InitArcLengthTravelObjPos;

    private float pointStepDuration = 0f;

    private LineRenderer TangentLR;
    private Vector3[] tangentArr = new Vector3[2];

    private LineRenderer NormalLR;
    private Vector3[] normalArr = new Vector3[2];

    private LineRenderer BinormalLR;
    private Vector3[] binormalArr = new Vector3[2];

    private LineRenderer ArcLengthTangentLR;
    private Vector3[] arcLengthTangentArr = new Vector3[2];

    private LineRenderer ArcLengthNormalLR;
    private Vector3[] arcLengthNormalArr = new Vector3[2];

    private LineRenderer ArcLengthBinormalLR;
    private Vector3[] arcLengthBinormalArr = new Vector3[2];


    // Start is called before the first frame update
    void Start()
    {
        pointStepDuration = 
            0f //(1f / 30f) //60f) 
            * GlobalData.RunSpeedFactor;

        // Setup travel object line renderers
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

        // Setup arc length travel object
        InitArcLengthTravelObjPos = ArcLengthTravelObject.position;
        if (ArcLengthTravelObject.childCount > 0)
        {
            GameObject firstChild = ArcLengthTravelObject.GetChild(0).gameObject;
            ArcLengthTangentLR = firstChild.GetComponent<LineRenderer>();
            ArcLengthTangentLR.positionCount = 2;
        }

        if (ArcLengthTravelObject.childCount > 1)
        {
            GameObject secondChild = ArcLengthTravelObject.GetChild(1).gameObject;
            ArcLengthNormalLR = secondChild.GetComponent<LineRenderer>();
            ArcLengthNormalLR.positionCount = 2;
        }

        if (ArcLengthTravelObject.childCount > 2)
        {
            GameObject thirdChild = ArcLengthTravelObject.GetChild(2).gameObject;
            ArcLengthBinormalLR = thirdChild.GetComponent<LineRenderer>();
            ArcLengthBinormalLR.positionCount = 2;
        }


        GlobalData.InitializeData();

        // Display html resource
        BrowserWall.OpenURL(GlobalData.NamedCurveDatasets[GlobalData.currentCurveIndex].NotebookURL);

        CurveSelectWall.UpdateCurveMenuButtons();
        UpdateWorldObjects();        
    }

    //private float updateTimer = 0f;

    private void Update()
    {
        if (GlobalData.IsDriving)
        {
            //updateTimer += Time.deltaTime;
            //if(updateTimer >= pointStepDuration)
            //{
            //    updateTimer = 0f;
            SetTravelPointAndDisplay();                
            //}
        }
            
    }

    public void UpdateLineRenderers()
    {        
        if (GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints is null) return;

        if (DisplayLR is null)
            Debug.Log("Failed to get line renderer component");

        DisplayLR.positionCount = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints.Count;
        DisplayLR.SetPositions(GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints.ToArray());

        InfoWall.UpdatePlotLineRenderers();

    }

    public void StartRun()
    {
        GlobalData.CurrentPointIndex = 0;
        GlobalData.IsDriving = true;        
    }

    public void SwitchToNextDataset()
    {
        // Stop driving
        if(GlobalData.IsDriving)
        {
            GlobalData.IsDriving = false;            
        }

        // Increment data set index, reset to 0 on overflow
        ++GlobalData.currentCurveIndex;
        if (GlobalData.currentCurveIndex >= GlobalData.CurrentDataset.Count)
            GlobalData.currentCurveIndex = 0;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateWorldObjects();
    }

    public void SwitchToPreviousDataset()
    {
        // Stop driving
        if (GlobalData.IsDriving)
        {
            GlobalData.IsDriving = false;
        }

        // Decrement data set index, reset to last element on negative index
        --GlobalData.currentCurveIndex;
        if (GlobalData.currentCurveIndex < 0)
            GlobalData.currentCurveIndex = GlobalData.CurrentDataset.Count - 1;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateWorldObjects();
    }


    public void SwitchToSpecificDataset(string name)
    {
        // Stop driving
        if (GlobalData.IsDriving)
        {
            GlobalData.IsDriving = false;
        }

        //// Increment data set index, reset to 0 on overflow
        //++currentDataSetIndex;
        //if (currentDataSetIndex >= CurrentDataset.Count)
        //    currentDataSetIndex = 0;

        int index = GlobalData.CurrentDataset.FindIndex(x => x.Name.Equals(name));
        if (index == -1) return;

        GlobalData.currentCurveIndex = index;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateWorldObjects();
    }

    
    

    public void UpdateWorldObjects()
    {
        UpdateLineRenderers();
        SetTravelPointAndDisplay();
    }

    private void SetTravelPointAndDisplay()
    {
        // Null checks
        if (TravelObject is null) return;
        if (GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints is null) return;
        if (GlobalData.CurrentPointIndex < 0 ) return;
        
        // On arrival at the last point, stop driving
        if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints.Count)
        {
            GlobalData.IsDriving = false;
            return;
        }

        
        int pointIndex = GlobalData.CurrentPointIndex;

        TravelObject.position = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints[pointIndex];
        ArcLengthTravelObject.position = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLengthWorldPoints[pointIndex];

        tangentArr[0] = TravelObject.position;
        tangentArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].fresnetApparatuses[pointIndex].Tangent;
        TangentLR.SetPositions(tangentArr);

        normalArr[0] = TravelObject.position;
        normalArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].fresnetApparatuses[pointIndex].Normal;
        NormalLR.SetPositions(normalArr);

        binormalArr[0] = TravelObject.position;
        binormalArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].fresnetApparatuses[pointIndex].Binormal;
        BinormalLR.SetPositions(binormalArr);


        arcLengthTangentArr[0] = ArcLengthTravelObject.position;
        arcLengthTangentArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Tangent;
        ArcLengthTangentLR.SetPositions(arcLengthTangentArr);

        arcLengthNormalArr[0] = ArcLengthTravelObject.position;
        arcLengthNormalArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Normal;
        ArcLengthNormalLR.SetPositions(arcLengthNormalArr);

        arcLengthBinormalArr[0] = ArcLengthTravelObject.position;
        arcLengthBinormalArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Binormal;
        ArcLengthBinormalLR.SetPositions(arcLengthBinormalArr);



        Vector3 nextPos = Vector3.zero;
        if (pointIndex < GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints.Count - 1)
        {
            nextPos = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints[pointIndex + 1];
        }
        else
        {
            nextPos = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints[pointIndex];
        }

        TravelObject.transform.LookAt(nextPos, (binormalArr[0] + binormalArr[1]).normalized);


        // ToDo: Add arc length travel object rotation ?


        InfoWall.UpdateInfoLabels();
        InfoWall.UpdatePlotTravelObjects();
        

        ++GlobalData.CurrentPointIndex;
    }


    

    

    

    

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



