using System.Collections.Generic;
using log4net;
using UnityEngine;
using Views.Display;

namespace Controller
{
    public abstract class AbstractExerciseViewController
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(AbstractExerciseViewController));
        
        protected readonly Transform _rootElement;

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

        protected List<AbstractCurveView> _views;
        
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

        protected AbstractExerciseViewController(Transform root)
        {
            _rootElement = root;

            _views = new List<AbstractCurveView>()
            {
                //selView
            };
            
            InitViews();
        }

        public void InitViews()
        {
            foreach (var view in _views)
            {
                _updateViewsDelegate += view.UpdateView;
            }

            if(_views.Count > 0)
                UpdateViewsDelegate();
            
            SwitchView(0);
        }

        public virtual void SwitchView(int index)
        {
            if (index < 0 || index >= _views.Count) return;
            
            // Log.Debug("Setting view: " + index);
            
            currentView = _views[index];
            
            // _travelObject.gameObject.SetActive(CurrentView.HasTravelPoint);
            // _arcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthPoint);
        }

        public virtual void SetViewVisibility(bool value)
        {
            for (int i = 0; i < _rootElement.childCount; i++)
            {
                _rootElement.GetChild(i).gameObject.SetActive(value);
            }
        }
    }
}