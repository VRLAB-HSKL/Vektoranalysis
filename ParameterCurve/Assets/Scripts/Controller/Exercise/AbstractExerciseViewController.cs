using System.Collections.Generic;
using log4net;
using Model;
using UnityEngine;
using Views.Display;
using Views.Exercise;

namespace Controller.Exercise
{
    /// <summary>
    /// Abstract base class for all view controllers related to displaying curve data in global data model
    /// <see cref="GlobalDataModel"/>
    /// </summary>
    public abstract class AbstractExerciseViewController
    {
        
        #region Public members

        /// <summary>
        /// Current view on the exercise
        /// </summary>
        protected AbstractExerciseView CurrentView { get; set; }

        #endregion Public members

        #region Protected members

        // ToDo: Replace this with MBU observer pattern ?
        /// <summary>
        /// Delegate to update views, intended to replicate observer pattern behaviour using C# language construct
        /// </summary>
        protected delegate void DUpdateViewsDelegate();

        /// <summary>
        /// Private delegate instance for custom getter
        /// </summary>
        protected DUpdateViewsDelegate RawUpdateViewsDelegate;

        /// <summary>
        /// Public delegate instance, called to update views
        /// </summary>
        protected DUpdateViewsDelegate UpdateViewsDelegate
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
        /// Collection of all views associated with this controller
        /// </summary>
        protected List<AbstractExerciseView> Views;
        
        #endregion Protected members

        #region Private members

        /// <summary>
        /// Static log4net logger instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractExerciseViewController));
        
        /// <summary>
        /// Root element position in the world, used as the origin for all curve coordinates
        /// </summary>
        private readonly Transform _rootElement;
        
        #endregion Private members
        
        
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="root">Root transform</param>
        protected AbstractExerciseViewController(Transform root)
        {
            _rootElement = root;

            Views = new List<AbstractExerciseView>();
            
            InitViews();
        }
        
        #endregion Constructors

        #region Public functions

        /// <summary>
        /// Changes view visibility
        /// </summary>
        /// <param name="value">View is visible</param>
        public virtual void SetViewVisibility(bool value)
        {
            for (var i = 0; i < _rootElement.childCount; i++)
            {
                _rootElement.GetChild(i).gameObject.SetActive(value);
            }
        }
        
        #endregion Public functions

        #region Protected functions
        
        /// <summary>
        /// Initialize views
        /// </summary>
        protected void InitViews()
        {
            foreach (var view in Views)
            {
                RawUpdateViewsDelegate += view.UpdateView;
            }

            if(Views.Count > 0)
                UpdateViewsDelegate();
            
            SwitchView(0);
        }
        
        #endregion Protected functions
        
        #region Private functions

        /// <summary>
        /// Switch to specific view based on index argument 
        /// </summary>
        /// <param name="index"></param>
        private void SwitchView(int index)
        {
            if (index < 0 || index >= Views.Count) return;
            CurrentView = Views[index];
        }
        
        #endregion Private functions
    }
}