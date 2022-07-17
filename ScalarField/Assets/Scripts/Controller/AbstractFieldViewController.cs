using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Controller
{
    public abstract class AbstractFieldViewController
    {
        /// <summary>
        /// Current view displayed by the controller
        /// </summary>
        public AbstractFieldView CurrentView { get; private set; }
        
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
        /// Collection of all views associated with this controller
        /// </summary>
        protected List<AbstractFieldView> Views;


        protected AbstractFieldViewController()
        {
            Views = new List<AbstractFieldView>();
        }
        
        
        /// <summary>
        /// Switch to the view associated with the given index
        /// </summary>
        /// <param name="index">View index</param>
        protected void SwitchView(int index)
        {
            if (index < 0 || index >= Views.Count) return;
            
            Debug.Log("Switching to view with index: " + index);
            
            CurrentView = Views[index];
        }
        
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

    }
}