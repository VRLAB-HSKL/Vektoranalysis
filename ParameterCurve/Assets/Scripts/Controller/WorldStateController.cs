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


    private AbstractCurveView currentView;
    public AbstractCurveView CurrentView
    {
        get { return currentView; }
        set
        {
            currentView = value;
            currentView.UpdateView();

            TravelObject.gameObject.SetActive(CurrentView.HasTravelPoint);
            ArcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthPoint);
        }
    }

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

        CurrentView = simpleRunWithArcLengthView;
        
        TravelObject.gameObject.SetActive(CurrentView.HasTravelPoint);
        ArcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthPoint);



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

    void Update()
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


        CurrentView.UpdateView();
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
        CurrentView.UpdateView();
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
        CurrentView.UpdateView();
    }

}