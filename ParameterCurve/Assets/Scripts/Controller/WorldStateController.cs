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
    [Header("WorldCurveElements")]
    public Transform WorldRootElement;
    public LineRenderer WorldDisplayLR;
    public Transform WorldTravelObject;
    public Transform WorldArcLengthTravelObject;

    [Header("TableCurveElements")]
    public Transform TableRootElement;
    public LineRenderer TableDisplayLR;
    public Transform TableTravelObject;
    public Transform TableArcLengthTravelObject;

    [Header("Walls")]
    public BrowserControl BrowserWall;
    public InformationControl InfoWall;
    public static CurveSelectionControl CurveSelectWall;

    public CurveViewController WorldViewController;
    public CurveViewController TableViewController;
    
    private Vector3 InitTravelObjPos;
    private Vector3 InitArcLengthTravelObjPos;

    private float pointStepDuration = 0f;    

    // Start is called before the first frame update
    private void Awake()
    {
        GlobalData.InitializeData();

        WorldViewController = new CurveViewController(WorldRootElement, WorldDisplayLR, WorldTravelObject, WorldArcLengthTravelObject, 1f);
        TableViewController = new CurveViewController(TableRootElement, TableDisplayLR, TableTravelObject, TableArcLengthTravelObject, 0.125f);
        

        pointStepDuration = 
            0f //(1f / 30f) //60f) 
            * GlobalData.RunSpeedFactor;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.NamedCurveDatasets[GlobalData.CurrentCurveIndex].NotebookURL);
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
            //WorldViewController.CurrentView.UpdateView();
            WorldViewController.UpdateViewsDelegate();
            TableViewController.CurrentView.UpdateView();

            InfoWall.UpdatePlotLineRenderers();

            //if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
            //{
            //    GlobalData.IsDriving = false;
            //}
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
        InfoWall.UpdatePlotLineRenderers();

        //WorldViewController.CurrentView.UpdateView();
        WorldViewController.UpdateViewsDelegate();
        TableViewController.CurrentView.UpdateView();
        
        //UpdateWorldObjects();
    }

    public void SwitchToPreviousDataset()
    {
        // Stop driving
        if (GlobalData.IsDriving)
        {
            if(WorldViewController.CurrentView is null)
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
        InfoWall.UpdatePlotLineRenderers();

        //UpdateWorldObjects();
        //WorldViewController.CurrentView.UpdateView();
        WorldViewController.UpdateViewsDelegate();
        TableViewController.CurrentView.UpdateView();
    }


   

    public void SwitchToSpecificDataset(string name)
    {
        // Stop driving
        if (GlobalData.IsDriving)
        {
            GlobalData.IsDriving = false;
        }

        var index = GlobalData.CurrentDataset.FindIndex(x => x.Name.Equals(name));
        if (index == -1) return;

        GlobalData.CurrentCurveIndex = index;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
        InfoWall.UpdatePlotLineRenderers();

        //UpdateWorldObjects();
        //WorldViewController.CurrentView.UpdateView();
        WorldViewController.UpdateViewsDelegate();
        TableViewController.CurrentView.UpdateView();
    }

}