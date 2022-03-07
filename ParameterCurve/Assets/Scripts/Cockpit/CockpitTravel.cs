using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using HTC.UnityPlugin.Vive;
using Model;
using UnityEngine;
using VRKL.MBU;

public class CockpitTravel : MonoBehaviour
{
    public LineRenderer CurveLine;
    public GameObject TravelObjectParent;

    public int CurveIndex;
    
    private float _updateTimer;
    //private int index;

    private WaypointManager wpm;
    private List<Vector3> list;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetPoints();
        
        wpm = new WaypointManager(list.ToArray(), 0.1f, false);

        //Debug.Log(CurveLine.positionCount);
    }

    // Update is called once per frame
    void Update()
    {
        // Start run on trigger press
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {
            ResetTravel();
        }
        
        // Previous curve on dpad left
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.DPadLeft))
        {
            //Log.Debug("Run started with controller button!");
            SwitchToPreviousDataset();
            SetPoints();
            ResetTravel();
        }
        
        // Next curve on dpad right
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.DPadRight))
        {
            SwitchToNextDataset();
            SetPoints();
            ResetTravel();
        }
        
        
        // Update time since last point step
        _updateTimer += Time.deltaTime;
                
        // If the time threshold has been reached, traverse to next point
        if(_updateTimer >= 0.05f)
        {
            _updateTimer = 0f;
            
            NextPoint();
            
            //++index;
        }
        
        
    }

    private void ResetTravel()
    {
        wpm.ResetWaypoints();
    }

    private void NextPoint()
    {
        var pos = wpm.GetWaypoint();
        var target = wpm.GetFollowupWaypoint();
        var dist = Vector3.Distance(pos, target);
        TravelObjectParent.transform.LookAt(target);
        TravelObjectParent.transform.position = wpm.Move(pos, dist);
    }

    private void SetPoints()
    {
        list = GlobalDataModel.CurrentDataset[CurveIndex].worldPoints;
        CurveLine.positionCount = list.Count;
        CurveLine.SetPositions(list.ToArray());
    }
    
    // private List<Vector3> CalculateDemoCurve()
    // {
    //     int n = 500;
    //     int times = 2;
    //     
    //     float xLength = 10f;
    //     float yLength = 10f;
    //
    //     float stepFactor = 1f / n;
    //     
    //     float xStep = xLength * stepFactor;
    //     float yStep = yLength * stepFactor;
    //
    //     var list = new List<Vector3>();
    //     for (int i = 0; i < times; i++)
    //     {
    //         for (int j = 0; j < n; j++)
    //         {
    //             float x = -5f + xStep * j;
    //             float y = yStep * j;
    //             list.Add(new Vector3(x, y, 0f));
    //         }
    //
    //         for (int j = n; j > 0; j--)
    //         {
    //             float x = 5f - xStep * j;
    //             float y = yStep * j;
    //             list.Add(new Vector3(x, y, 0f));
    //         }
    //     }
    //
    //     return list;
    // }
    
    //TODO: Replace duplicate functions
        
    /// <summary>
    /// Switch to the next curve in the current dataset <see cref="GlobalDataModel.CurrentDataset"/>
    /// </summary>
    public void SwitchToNextDataset()
    {
        // Stop driving
        if(GlobalDataModel.IsRunning)
        {
            GlobalDataModel.IsRunning = false;            
        }

        // Increment data set index, reset to 0 on overflow
        ++GlobalDataModel.CurrentCurveIndex;
        if (GlobalDataModel.CurrentCurveIndex >= GlobalDataModel.CurrentDataset.Count)
            GlobalDataModel.CurrentCurveIndex = 0;

        switch (GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].View)
        {
            default:
                GlobalDataModel.WorldCurveViewController.SwitchView(0);
                GlobalDataModel.TableCurveViewController.SwitchView(0);
                break;
                
            case "run":
                GlobalDataModel.WorldCurveViewController.SwitchView(1);
                GlobalDataModel.TableCurveViewController.SwitchView(1);
                break;
                
            case "arc":
                GlobalDataModel.WorldCurveViewController.SwitchView(2);
                GlobalDataModel.TableCurveViewController.SwitchView(2);
                break;
        }
        
        

        GlobalDataModel.WorldCurveViewController.UpdateViewsDelegate();
        GlobalDataModel.WorldCurveViewController.CurrentView.UpdateView();
        GlobalDataModel.TableCurveViewController?.CurrentView.UpdateView();
           
    }

    /// <summary>
    /// Switch to the previous curve in the current dataset <see cref="GlobalDataModel.CurrentDataset"/>
    /// </summary>
    public void SwitchToPreviousDataset()
    {
        // Stop driving
        if (GlobalDataModel.IsRunning)
        {
            if(GlobalDataModel.WorldCurveViewController.CurrentView is null)
            {
                //Log.Warn("ViewEmpty");
            }

            GlobalDataModel.IsRunning = false;
        }

        // Decrement data set index, reset to last element on negative index
        --GlobalDataModel.CurrentCurveIndex;
        if (GlobalDataModel.CurrentCurveIndex < 0)
            GlobalDataModel.CurrentCurveIndex = GlobalDataModel.CurrentDataset.Count - 1;
    
        switch (GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].View)
        {
            default:
                GlobalDataModel.WorldCurveViewController.SwitchView(0);
                GlobalDataModel.TableCurveViewController.SwitchView(0);
                break;
                
            case "run":
                GlobalDataModel.WorldCurveViewController.SwitchView(1);
                GlobalDataModel.TableCurveViewController.SwitchView(1);
                break;
                
            case "arc":
                GlobalDataModel.WorldCurveViewController.SwitchView(2);
                GlobalDataModel.TableCurveViewController.SwitchView(2);
                break;
        }

        GlobalDataModel.WorldCurveViewController.UpdateViewsDelegate();
        GlobalDataModel.WorldCurveViewController.CurrentView?.UpdateView();
        GlobalDataModel.TableCurveViewController?.CurrentView.UpdateView();
        
    }

    public void SwitchToSpecificDataset(string datasetIdentifier)
    {
        // Stop driving
        if (GlobalDataModel.IsRunning)
        {
            GlobalDataModel.IsRunning = false;
        }

        if(GlobalDataModel.CurrentDisplayGroup == GlobalDataModel.CurveDisplayGroup.Exercises)
            GlobalDataModel.ExerciseCurveController.SetViewVisibility(true);
    
        var index = GlobalDataModel.CurrentDataset.FindIndex(
            x => x.Name.Equals(datasetIdentifier));
        if (index == -1) return;

        GlobalDataModel.CurrentCurveIndex = index;

        switch (GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].View)
        {
            default:
                GlobalDataModel.WorldCurveViewController.SwitchView(0);
                GlobalDataModel.TableCurveViewController.SwitchView(0);
                break;
                
            case "run":
                GlobalDataModel.WorldCurveViewController.SwitchView(1);
                GlobalDataModel.TableCurveViewController.SwitchView(1);
                break;
                
            case "arc":
                GlobalDataModel.WorldCurveViewController.SwitchView(2);
                GlobalDataModel.TableCurveViewController.SwitchView(2);
                break;
        }
        
        var worldView = GlobalDataModel.WorldCurveViewController.CurrentView;
        worldView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].WorldScalingFactor;


        var tableView = GlobalDataModel.TableCurveViewController?.CurrentView;
        if (tableView != null)
        {
            tableView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TableScalingFactor;
        }
            
        GlobalDataModel.WorldCurveViewController.UpdateViewsDelegate();
        GlobalDataModel.WorldCurveViewController.CurrentView?.UpdateView();
        GlobalDataModel.TableCurveViewController?.CurrentView.UpdateView();
        
    }
    
    
}
