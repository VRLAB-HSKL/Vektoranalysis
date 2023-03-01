using System.Collections.Generic;
using Model;
using ParamCurve.Scripts.Views.Display;
using UnityEngine;
using VRKL.MBU;
//using log4net;

namespace ParamCurve.Scripts.Controller.Curve
{
    /// <summary>
    /// Abstract base class for all view controllers related to displaying curve data in global data model
    /// <see cref="GlobalDataModel"/>
    /// </summary>
    public abstract class AbstractCurveViewController
    {
        /// <summary>
        /// Type enum
        /// </summary>
        public enum CurveControllerType { World = 0, Table = 1 };
        
        #region Public members
        
        /// <summary>
        /// Current view displayed by the controller
        /// </summary>
        public AbstractCurveView CurrentView { get; private set; }

        #endregion Public members
        
        #region Protected members
        
        // ToDo: Replace this with MBU observer pattern ?
        /// <summary>
        /// Delegate to update views, intended to replicate observer pattern behaviour using C# language construct
        /// </summary>
        public delegate void DUpdateViewsDelegate();

        /// <summary>
        /// Private delegate instance for custom getter
        /// </summary>
        protected DUpdateViewsDelegate RawUpdateViewsDelegate;

        /// <summary>
        /// Public delegate instance, called to update views
        /// </summary>
        public DUpdateViewsDelegate UpdateViewsDelegate
        {
            get
            {
                // Initialize delegate on first call
                if (RawUpdateViewsDelegate is null)
                {
                    RawUpdateViewsDelegate?.Invoke();
                }

                return RawUpdateViewsDelegate;
            }
        }
        
        /// <summary>
        /// Root element position in the world, used as the origin for all curve coordinates
        /// </summary>
        protected readonly Transform RootElement;

        /// <summary>
        /// Collection of all views associated with this controller
        /// </summary>
        protected List<AbstractCurveView> Views;


        protected WaypointManager Wpm;
        
        #endregion Protected members
        
        #region Private members

        // Static log4net logger instance
        //private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractCurveViewController));
        
        #endregion Private members
        
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="root">Root game object transform</param>
        protected AbstractCurveViewController(Transform root)
        {
            RootElement = root;

            Views = new List<AbstractCurveView>();
            Wpm = new WaypointManager(new Vector3[2], 0.1f);
            //Wpm.SetWaypoints();
            
            InitViews();
        }

        #endregion Constructors

        #region Public functions
                
        /// <summary>
        /// Hides or shows views based on argument value
        /// </summary>
        /// <param name="value">True: Show view, False: Hide view</param>
        public void SetViewVisibility(bool value)
        {
            for (var i = 0; i < RootElement.childCount; i++)
            {
                RootElement.GetChild(i).gameObject.SetActive(value);
            }
        }
        
        #endregion Public functions
        
        #region Protected functions
        
        /// <summary>
        /// Switch to the view associated with the given index
        /// </summary>
        /// <param name="index">View index</param>
        protected void SwitchView(int index)
        {
            if (index < 0 || index >= Views.Count) return;
            
            //Log.Debug("Switching to view with index: " + index);
            
            CurrentView = Views[index];
        }
        
        #endregion Protected functions
        
        #region Private functions
        
        /// <summary>
        /// Initialize views delegate
        /// </summary>
        private void InitViews()
        {
            foreach (var view in Views)
            {
                RawUpdateViewsDelegate += view.UpdateView;
            }

            if(Views.Count > 0)
                UpdateViewsDelegate();
            
            SwitchView(0);
        }
        
        #endregion Private functions
        
    }
}