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
using UI;
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
    public GameObject TableParent;
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

    
    public CurveViewController TableViewController { get; set; }
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
        GlobalData.ExerciseController = new ExerciseViewController(SelObjects.gameObject.transform, SelObjects, PillarPrefab);
        
        GlobalData.ExerciseController.SetViewVisibility(false);
        //GlobalData.exerciseController.SetViewVisibility(GlobalData.initFile.curveSelection.activated);

        
        
        // var curveColorValues = GlobalData.initFile
        // if (curveColorValues.Length >= 3)
        // {
        //     float r = curveColorValues[0] / 255f;
        //     float g = curveColorValues[1] / 255f;
        //     float b = curveColorValues[2] / 255f;
        //     float a = curveColorValues.Length == 4 ? curveColorValues[3] : 1f;
        //     
        //     Color newColor = new Color(r, g, b, a);
        //     WorldDisplayLR.material.color = newColor;
        //     WorldDisplayLR.material.SetColor("_EmissionColor", newColor);
        //     
        // }

        var displayCurve = GlobalData.DisplayCurveDatasets[0];

        GlobalData.WorldViewController = new CurveViewController(WorldRootElement, WorldDisplayLR, 
            WorldTravelObject, WorldArcLengthTravelObject, displayCurve.WorldScalingFactor);
        GlobalData.WorldViewController.SetViewVisibility(true);

        




        if (GlobalData.initFile.ApplicationSettings.TableSettings.Activated)
        {
            TableViewController = new CurveViewController(TableRootElement, TableDisplayLR, 
                TableTravelObject, TableArcLengthTravelObject, displayCurve.TableScalingFactor);
        }
        else
        {
            TableParent.SetActive(false);
        }


        InfoWall.ObserevdViewController = GlobalData.WorldViewController;
        

        //if(GlobalData.initFile.table.activated)
        
        
        
        
        pointStepDuration = 
            //0.25f 
            (1f / 30f) 
            //60f) 
            * GlobalData.RunSpeedFactor;

        
    }


    private void Start()
    {
        // Display html resource
        BrowserWall.OpenURL(GlobalData.DisplayCurveDatasets[GlobalData.CurrentCurveIndex].NotebookURL);

        // Set plotline renderers
        InfoWall.Update();
    }


    private float updateTimer = 0f;

    private void Update()
    {
        if (GlobalData.IsRunning)
        {
            updateTimer += Time.deltaTime;
            if(updateTimer >= pointStepDuration)
            {
                updateTimer = 0f;
                
                //GlobalData.WorldViewController.UpdateViewsDelegate();
                GlobalData.WorldViewController.CurrentView.UpdateView();
                TableViewController?.CurrentView.UpdateView();

                InfoWall.Update();
            }
            //WorldViewController.CurrentView.UpdateView();
            
            
            
            

            // if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
            // {
            //     GlobalData.IsDriving = false;
            // }
        }
        
        if(ActivatePoseTracking) StaticLogging();
            
        //Debug.Log("GlobalData_IsDriving: " + GlobalData.IsDriving);
    }

    

    public void StartRun()
    {
        Log.Info("Starting curve run...");
        //GlobalData.CurrentPointIndex = 0;
        GlobalData.IsRunning = true;        
        GlobalData.WorldViewController.StartRun();
        TableViewController.StartRun();
    }

    public void SwitchToNextDataset()
    {
        // Stop driving
        if(GlobalData.IsRunning)
        {
            GlobalData.IsRunning = false;            
        }

        // Increment data set index, reset to 0 on overflow
        ++GlobalData.CurrentCurveIndex;
        if (GlobalData.CurrentCurveIndex >= GlobalData.CurrentDataset.Count)
            GlobalData.CurrentCurveIndex = 0;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;

        var worldView = GlobalData.WorldViewController.CurrentView as AbstractCurveView;
        worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
        if(worldView.GetType() == typeof(SimpleRunCurveView) || worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
        {
            Debug.Log("adjust travel gameobject");
            var runview = worldView as SimpleRunCurveView;
            runview.CurrentPointIndex = 0;
            runview.SetTravelPoint();
            runview.SetMovingFrame();
        }
        
        var tableView = TableViewController.CurrentView as AbstractCurveView;
        tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
        if(tableView.GetType() == typeof(SimpleRunCurveView) || tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
        {
            Debug.Log("adjust travel gameobject");
            var runview = tableView as SimpleRunCurveView;
            runview.CurrentPointIndex = 0;
            runview.SetTravelPoint();
            runview.SetMovingFrame();
        }
        
        
        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
        
        // Udpate info wall
        InfoWall.Update();

        //WorldViewController.CurrentView.UpdateView();
        GlobalData.WorldViewController.UpdateViewsDelegate();
        TableViewController?.CurrentView.UpdateView();
        
        //UpdateWorldObjects();
    }

    public void SwitchToPreviousDataset()
    {
        // Stop driving
        if (GlobalData.IsRunning)
        {
            if(GlobalData.WorldViewController.CurrentView is null)
            {
                Log.Warn("ViewEmpty");
            }

            GlobalData.IsRunning = false;
        }

        // Decrement data set index, reset to last element on negative index
        --GlobalData.CurrentCurveIndex;
        if (GlobalData.CurrentCurveIndex < 0)
            GlobalData.CurrentCurveIndex = GlobalData.CurrentDataset.Count - 1;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;
        
        var worldView = GlobalData.WorldViewController.CurrentView as AbstractCurveView;
        worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
        if(worldView.GetType() == typeof(SimpleRunCurveView) || worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
        {
            Debug.Log("adjust travel gameobject");
            var runview = worldView as SimpleRunCurveView;
            runview.CurrentPointIndex = 0;
            runview.SetTravelPoint();
            runview.SetMovingFrame();
        }
        
        var tableView = TableViewController.CurrentView as AbstractCurveView;
        tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
        if(tableView.GetType() == typeof(SimpleRunCurveView) || tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
        {
            Debug.Log("adjust travel gameobject");
            var runview = tableView as SimpleRunCurveView;
            runview.CurrentPointIndex = 0;
            runview.SetTravelPoint();
            runview.SetMovingFrame();
        }
        
        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
        InfoWall.Update();

        //WorldViewController.CurrentView.UpdateView();
        GlobalData.WorldViewController.UpdateViewsDelegate();
        TableViewController?.CurrentView.UpdateView();
    }


   

    public void SwitchToSpecificDataset(string name)
    {
        // Stop driving
        if (GlobalData.IsRunning)
        {
            GlobalData.IsRunning = false;
        }

        if(GlobalData.CurrentDisplayGroup == GlobalData.CurveDisplayGroup.Exercises)
            GlobalData.ExerciseController.SetViewVisibility(true);
        
        var index = GlobalData.CurrentDataset.FindIndex(x => x.Name.Equals(name));
        if (index == -1) return;

        GlobalData.CurrentCurveIndex = index;

        // Reset point index
        GlobalData.CurrentPointIndex = 0;
        
        var worldView = GlobalData.WorldViewController.CurrentView as AbstractCurveView;
        worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
        
        var tableView = TableViewController.CurrentView as AbstractCurveView;
        tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;

        // Display html resource
        BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
        InfoWall.Update();

        GlobalData.WorldViewController.UpdateViewsDelegate();
        TableViewController?.CurrentView.UpdateView();
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