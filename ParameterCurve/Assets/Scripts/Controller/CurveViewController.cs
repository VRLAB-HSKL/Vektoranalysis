using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveViewController
{
    [Header("DisplayElements")]
    private readonly Transform _rootElement;
    private readonly LineRenderer _displayLr;
    private readonly Transform _travelObject;
    private readonly Transform _arcLengthTravelObject;

    private AbstractCurveView currentView;
    public AbstractCurveView CurrentView
    {
        get => currentView;
        set
        {
            currentView = value;
            currentView.UpdateView();
        }
    }

    private readonly List<AbstractCurveView> _views;
    
    public delegate void d_updateViewsDelegate();
    
    private readonly d_updateViewsDelegate _updateViewsDelegate;

    public d_updateViewsDelegate UpdateViewsDelegate
    {
        get
        {
            if (_updateViewsDelegate is null)
            {
                _updateViewsDelegate();
            }

            return _updateViewsDelegate;
        }
    }

    public CurveViewController(Transform root, LineRenderer displayLR, Transform travel, Transform arcTravel, float scalingFactor)
    {
        _rootElement = root;
        _displayLr = displayLR;
        _travelObject = travel;
        _arcLengthTravelObject = arcTravel;

        var simpleView = new SimpleCurveView(_displayLr, _rootElement.position, scalingFactor);
        var simpleRunView = new SimpleRunCurveView(_displayLr, _rootElement.position, scalingFactor, _travelObject);
        var simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(_displayLr, _rootElement.position, scalingFactor, _travelObject, _arcLengthTravelObject);
        
        _views = new List<AbstractCurveView>
        {
            simpleView,
            simpleRunView,
            simpleRunWithArcLengthView
        };

        foreach (var view in _views)
        {
            _updateViewsDelegate += view.UpdateView;
        }

        UpdateViewsDelegate();
        
        SwitchView(0);
    }

    public void SwitchView(int index)
    {
        if (index < 0 || index >= _views.Count) return;
        
        currentView = _views[index];
        _travelObject.gameObject.SetActive(CurrentView.HasTravelPoint);
        _arcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthPoint);
    }
}
