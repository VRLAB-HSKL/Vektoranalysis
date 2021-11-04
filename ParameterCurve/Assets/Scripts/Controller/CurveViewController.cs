using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Controller;
using HTC.UnityPlugin.Vive.WaveVRExtension;
using UnityEngine;
using Views;

public enum CurveControllerTye { World = 0, Table = 1 };

public class CurveViewController : AbstractViewController
{
    public CurveControllerTye Type;
    
    // ToDo:
    // Replace single static line renderer with dynamic creation of line renderer segments for each edge
    // between two points. This should remove visual bugs on the curve when the curves are scaled very small.
    private readonly LineRenderer _displayLr;
    private readonly Transform _travelObject;
    private readonly Transform _arcLengthTravelObject;
    
    
    // private new AbstractCurveView currentView;
    // public new AbstractCurveView CurrentView
    // {
    //     get => currentView;
    //     set
    //     {
    //         currentView = value;
    //         currentView.UpdateView();
    //     }
    // }
    
    
    // public delegate void d_updateViewsDelegate();
    //
    // private readonly d_updateViewsDelegate _updateViewsDelegate;
    //
    // public d_updateViewsDelegate UpdateViewsDelegate
    // {
    //     get
    //     {
    //         if (_updateViewsDelegate is null)
    //         {
    //             _updateViewsDelegate();
    //         }
    //
    //         return _updateViewsDelegate;
    //     }
    // }

    
    

    public CurveViewController(Transform root, LineRenderer displayLR, Transform travel, 
                               Transform arcTravel, float scalingFactor, CurveControllerTye type) : base(root)
    {
        Type = type;
        
        _displayLr = displayLR;
        _travelObject = travel;
        _arcLengthTravelObject = arcTravel;

        var simpleView = new SimpleCurveView(_displayLr, _rootElement.position, scalingFactor, Type);
        var simpleRunView = new SimpleRunCurveView(_displayLr, _rootElement.position, scalingFactor, _travelObject, Type);
        var simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(_displayLr, _rootElement.position, 
            scalingFactor, _travelObject, _arcLengthTravelObject, Type);
        
        // Temporarily deactivate run object on arc view
        //simpleRunWithArcLengthView.HasTravelPoint = false;
        
        _views = new List<AbstractView>
        {
            simpleView,
            simpleRunView,
            simpleRunWithArcLengthView
        };
 
        // Debug.Log("ViewCount: " + _views.Count);
        
        foreach (var view in _views)
        {
            _updateViewsDelegate += view.UpdateView;
        }

        UpdateViewsDelegate();

        int initViewIndex = 0;
        
        if (GlobalData.initFile.DisplayCurves.Count > 0)
        {
            if (GlobalData.initFile.DisplayCurves[0].CurveSettings.DisplaySettings.View.Equals("simple")) initViewIndex = 0;
            if (GlobalData.initFile.DisplayCurves[0].CurveSettings.DisplaySettings.View.Equals("run")) initViewIndex = 1;
            if (GlobalData.initFile.DisplayCurves[0].CurveSettings.DisplaySettings.View.Equals("arc")) initViewIndex = 2;    
        }
        
        
        
        SwitchView(initViewIndex);
    }

    public void SwitchView(int index)
    {
        if (index < 0 || index >= _views.Count) return;
        
        
        base.SwitchView(index);
        _travelObject.gameObject.SetActive( (CurrentView as AbstractCurveView).HasTravelPoint);
        _arcLengthTravelObject.gameObject.SetActive((CurrentView as AbstractCurveView).HasArcLengthTravelPoint);
    }
    
    public void StartRun()
    {
        Log.Info("Starting curve run...");
        
        foreach (var view in _views)
        {
            view.StartRun();
        }

        
        GlobalData.IsRunning = true;        
    }
}
