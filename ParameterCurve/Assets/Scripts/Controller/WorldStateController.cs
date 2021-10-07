using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Controller;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using log4net;
//using System.Text.Json;

using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;


using TMPro;
using UnityEngine.Events;

public class WorldStateController : MonoBehaviour
{
    public static readonly ILog Log = LogManager.GetLogger(typeof(WorldStateController));
    
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

    [Header("ExerciseElements")] 
    public SelectionExerciseGameObjects SelObjects;

    public GameObject PillarPrefab;
    
    [Header("Walls")]
    public BrowserControl BrowserWall;
    public InformationControl InfoWall;
    public static CurveSelectionControl CurveSelectWall;

    public CurveViewController WorldViewController;
    public CurveViewController TableViewController;
    //public ExerciseViewController ExerciseController;


    public bool ActivatePoseTracking = false;
    
    
    private Vector3 InitTravelObjPos;
    private Vector3 InitArcLengthTravelObjPos;

    private float pointStepDuration = 0f;    

    // Start is called before the first frame update
    private void Awake()
    {
        GlobalData.InitializeData();
        Debug.Log("ExerciseControllerRoot: " + SelObjects.gameObject.name);
        GlobalData.exerciseController = new ExerciseViewController(SelObjects.gameObject.transform, SelObjects, PillarPrefab);
        
        GlobalData.exerciseController.SetViewVisibility(false);
        //GlobalData.exerciseController.SetViewVisibility(GlobalData.initFile.curveSelection.activated);
        
        WorldViewController = new CurveViewController(WorldRootElement, WorldDisplayLR, WorldTravelObject, WorldArcLengthTravelObject, 1f);
        WorldViewController.SetViewVisibility(true);
        
        
        
        
        
        
        TableViewController = new CurveViewController(TableRootElement, TableDisplayLR, TableTravelObject, TableArcLengthTravelObject, 0.125f);

        pointStepDuration = 
            0f //(1f / 30f) //60f) 
            * GlobalData.RunSpeedFactor;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.NamedCurveDatasets[GlobalData.CurrentCurveIndex].NotebookURL);

        // Set plotline renderers
        InfoWall.UpdatePlotLineRenderers();
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
        
        if(ActivatePoseTracking) StaticLogging();
            
    }

    

    public void StartRun()
    {
        Log.Info("Starting curve run...");
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
                Log.Warn("ViewEmpty");
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

        WorldViewController.UpdateViewsDelegate();
        TableViewController.CurrentView.UpdateView();
    }

    
    /// <summary>
    /// Static logging operations that are performed every frame, regardless of user interaction
    /// </summary>
    private void StaticLogging()
    {
        // Head pose
        RigidPose headPose = VivePose.GetPoseEx(BodyRole.Head);
        Log.Info("Head position: " + headPose.pos);
        Log.Info("Head rotation: " + headPose.rot);
        Log.Info("Head up: " + headPose.up);
        Log.Info("Head forward: " + headPose.forward);
        Log.Info("Head right: " + headPose.right);

        // Left hand pose
        var leftPose = VivePose.GetPoseEx(HandRole.LeftHand);
        Log.Info("Left hand position: " + leftPose.pos);
        Log.Info("Left hand rotation: " + leftPose.rot);
        Log.Info("Left hand up: " + leftPose.up);
        Log.Info("Left hand forward: " + leftPose.forward);
        Log.Info("Left hand right: " + leftPose.right);
        
        // Right hand pose
        var rightPose = VivePose.GetPoseEx(HandRole.RightHand);
        Log.Info("Right hand position: " + rightPose.pos);
        Log.Info("Right hand rotation: " + rightPose.rot);
        Log.Info("Right hand up: " + rightPose.up);
        Log.Info("Right hand forward: " + rightPose.forward);
        Log.Info("Right hand right: " + rightPose.right);
    }

}