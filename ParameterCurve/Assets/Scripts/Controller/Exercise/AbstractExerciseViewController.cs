using System.Collections.Generic;
using log4net;
using UnityEngine;
using Views.Display;

namespace Controller.Exercise
{
    /// <summary>
    /// Abstract base class for all view controllers related to displaying curve data in global data model
    /// <see cref="GlobalData"/>
    /// </summary>
    public abstract class AbstractExerciseViewController
    {
        
        #region Public members

        protected AbstractCurveView CurrentView { get; private set; }

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
        /// Root element position in the world, used as the origin for all curve coordinates
        /// </summary>
        private readonly Transform _rootElement;
        
        /// <summary>
        /// Collection of all views associated with this controller
        /// </summary>
        private readonly List<AbstractCurveView> Views;
        
        #endregion Protected members

        
        #region Private members

        /// <summary>
        /// Static log4net logger instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractExerciseViewController));
        
        #endregion Private members
        
        
        #region Constructors
        
        protected AbstractExerciseViewController(Transform root)
        {
            _rootElement = root;

            Views = new List<AbstractCurveView>();
            
            InitViews();
        }
        
        #endregion Constructors

        #region Public functions

        public virtual void SetViewVisibility(bool value)
        {
            for (var i = 0; i < _rootElement.childCount; i++)
            {
                _rootElement.GetChild(i).gameObject.SetActive(value);
            }
        }
        
        #endregion Public functions
        
        
        #region Protected functions
        
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

        private void SwitchView(int index)
        {
            if (index < 0 || index >= Views.Count) return;
            CurrentView = Views[index];
        }
        
        #endregion Private functions
    }
}