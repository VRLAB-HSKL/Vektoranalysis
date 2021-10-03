﻿using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Controller
{
    public abstract class AbstractViewController
    {
        protected readonly Transform _rootElement;

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

            _views = new List<IView>()
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