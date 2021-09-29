using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Controller
{
    public abstract class AbstractViewController
    {
        protected readonly Transform _rootElement;
        // private readonly LineRenderer _displayLr;
        // private readonly Transform _travelObject;
        // private readonly Transform _arcLengthTravelObject;

        private IView currentView;
        public IView CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                currentView.UpdateView();
            }
        }

        protected List<IView> _views;
        
        // ToDo: Replace this with MBU observer pattern ?
        public delegate void d_updateViewsDelegate();
        
        protected d_updateViewsDelegate _updateViewsDelegate;

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

        protected AbstractViewController(Transform root)
        {
            _rootElement = root;
            // _displayLr = displayLR;
            // _travelObject = travel;
            // _arcLengthTravelObject = arcTravel;

            // var simpleView = new SimpleCurveView(_displayLr, _rootElement.position, scalingFactor);
            // var simpleRunView = new SimpleRunCurveView(_displayLr, _rootElement.position, scalingFactor, _travelObject);
            // var simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(_displayLr, _rootElement.position, scalingFactor, _travelObject, _arcLengthTravelObject);

            
            
            // var selView = new SelectionExerciseCompoundView(selEx, pillarPrefab, root);
            
            //var selView = new SelectionExerciseView()
            
            _views = new List<IView>();

            foreach (var view in _views)
            {
                _updateViewsDelegate += view.UpdateView;
            }

            UpdateViewsDelegate();
            
            SwitchView(0);
        }

        public virtual void SwitchView(int index)
        {
            if (index < 0 || index >= _views.Count) return;
            
            currentView = _views[index];
            // _travelObject.gameObject.SetActive(CurrentView.HasTravelPoint);
            // _arcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthPoint);
        }
    }
}