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

    public AbstractCurveView CurrentView;
    public SimpleCurveView simpleView;
    public SimpleRunCurveView simpleRunView;
    public SimpleRunCurveWithArcLength simpleRunWithArcLengthView;
    

    public BrowserControl BrowserWall;
    public InformationControl InfoWall;
    public static CurveSelectionControl CurveSelectWall;

    private Vector3 InitTravelObjPos;
    private Vector3 InitArcLengthTravelObjPos;

    private float pointStepDuration = 0f;    

    // Start is called before the first frame update
    void Start()
    {
        GlobalData.InitializeData();

        simpleView = new SimpleCurveView(DisplayLR);
        simpleView.UpdateView();

        simpleRunView = new SimpleRunCurveView(DisplayLR, TravelObject);
        simpleRunView.UpdateView();

        simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(DisplayLR, TravelObject, ArcLengthTravelObject);
        simpleRunWithArcLengthView.UpdateView();

        CurrentView = simpleView;

        pointStepDuration = 
            0f //(1f / 30f) //60f) 
            * GlobalData.RunSpeedFactor;


        

        // Display html resource
        BrowserWall.OpenURL(GlobalData.NamedCurveDatasets[GlobalData.CurrentCurveIndex].NotebookURL);

        //CurveSelectWall.UpdateCurveMenuButtons();

        //NamedCurvesState ns = new NamedCurvesState(CurveSelectWall.CurveMenuContent, CurveSelectWall.CurveMenuButtonPrefab);
        //GlobalData.CurveSelectionFSM = new CurveSelectionStateContext(ns);

        //UpdateWorldObjects();        
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
            // SetTravelPointAndDisplay();                
            //}

            

            CurrentView.UpdateView();
        }
            
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
        ++GlobalData.CurrentCurveIndex;
        if (GlobalData.CurrentCurveIndex >= GlobalData.CurrentDataset.Count)
            GlobalData.CurrentCurveIndex = 0;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);

        //UpdateWorldObjects();
    }

    public void SwitchToPreviousDataset()
    {
        // Stop driving
        if (GlobalData.IsDriving)
        {
            if(CurrentView is null)
            {
                Debug.Log("ViewEmpty");
            }

            GlobalData.IsDriving = false;
        }

        // Decrement data set index, reset to last element on negative index
        --GlobalData.CurrentCurveIndex;
        if (GlobalData.CurrentCurveIndex < 0)
            GlobalData.CurrentCurveIndex = GlobalData.CurrentDataset.Count - 1;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);

        //UpdateWorldObjects();
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

        GlobalData.CurrentCurveIndex = index;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);

        //UpdateWorldObjects();
    }



    //private void SetTravelPointAndDisplay()
    //{
    //    // Null checks
    //    if (CurrentView.TravelObject is null) return;
    //    if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
    //    if (GlobalData.CurrentPointIndex < 0) return;

    //    // On arrival at the last point, stop driving
    //    if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
    //    {
    //        GlobalData.IsDriving = false;
    //        return;
    //    }


    //    int pointIndex = GlobalData.CurrentPointIndex;

    //    CurrentView.TravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex];
    //    CurrentView.ArcLengthTravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex];

    //    tangentArr[0] = CurrentView.TravelObject.position;
    //    tangentArr[1] = CurrentView.TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[pointIndex].Tangent;
    //    TangentLR.SetPositions(tangentArr);

    //    normalArr[0] = CurrentView.TravelObject.position;
    //    normalArr[1] = CurrentView.TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[pointIndex].Normal;
    //    NormalLR.SetPositions(normalArr);

    //    binormalArr[0] = CurrentView.TravelObject.position;
    //    binormalArr[1] = CurrentView.TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[pointIndex].Binormal;
    //    BinormalLR.SetPositions(binormalArr);


    //    arcLengthTangentArr[0] = CurrentView.ArcLengthTravelObject.position;
    //    arcLengthTangentArr[1] = CurrentView.ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Tangent;
    //    ArcLengthTangentLR.SetPositions(arcLengthTangentArr);

    //    arcLengthNormalArr[0] = CurrentView.ArcLengthTravelObject.position;
    //    arcLengthNormalArr[1] = CurrentView.ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Normal;
    //    ArcLengthNormalLR.SetPositions(arcLengthNormalArr);

    //    arcLengthBinormalArr[0] = CurrentView.ArcLengthTravelObject.position;
    //    arcLengthBinormalArr[1] = CurrentView.ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Binormal;
    //    ArcLengthBinormalLR.SetPositions(arcLengthBinormalArr);

    //    Vector3 nextPos;
    //    if (pointIndex < GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count - 1)
    //    {
    //        nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex + 1];
    //    }
    //    else
    //    {
    //        nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex];
    //    }

    //    CurrentView.TravelObject.transform.LookAt(nextPos, (binormalArr[0] + binormalArr[1]).normalized);


    //    // ToDo: Add arc length travel object rotation ?


    //    InfoWall.UpdateInfoLabels();
    //    InfoWall.UpdatePlotTravelObjects();


    //    ++GlobalData.CurrentPointIndex;
    //}













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




