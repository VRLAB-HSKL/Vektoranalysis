using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using Views;


public class CurveViewController : AbstractViewController
{
    // ToDo:
    // Replace single static line renderer with dynamic creation of line renderer segments for each edge
    // between two points. This should remove visual bugs on the curve when the curves are scaled very small.
    private readonly LineRenderer _displayLr;
    private readonly Transform _travelObject;
    private readonly Transform _arcLengthTravelObject;

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
                               Transform arcTravel, float scalingFactor) : base(root)
    {
        _displayLr = displayLR;
        _travelObject = travel;
        _arcLengthTravelObject = arcTravel;

        var simpleView = new SimpleCurveView(_displayLr, _rootElement.position, scalingFactor);
        var simpleRunView = new SimpleRunCurveView(_displayLr, _rootElement.position, scalingFactor, _travelObject);
        var simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(_displayLr, _rootElement.position, scalingFactor, _travelObject, _arcLengthTravelObject);
        
        _views = new List<IView>
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
        
        base.SwitchView(index);
        _travelObject.gameObject.SetActive( (CurrentView as AbstractCurveView).HasTravelPoint);
        _arcLengthTravelObject.gameObject.SetActive((CurrentView as AbstractCurveView).HasArcLengthPoint);
    }
}
