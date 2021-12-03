using System.Collections.Generic;
using Model;
using UnityEngine;
using Views.Display;

namespace Controller.Curve
{
    /// <summary>
    /// View controller for all views related to displaying curve data
    /// </summary>
    public class CurveViewController : AbstractCurveViewController
    {
        #region Private members
        
        // ToDo:
        // Replace single static line renderer with dynamic creation of line renderer segments for each edge
        // between two points. This should remove visual bugs on the curve when the curves are scaled very small.
        
        /// <summary>
        /// Travel object transform used to move the travel object along the curve line
        /// </summary>
        private readonly Transform _travelObject;
        
        /// <summary>
        /// Travel object transform used to move the arc travel object along the curve line
        /// </summary>
        private readonly Transform _arcLengthTravelObject;
    
        #endregion Private members
        
        #region Constructors
        
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="root">Root game position</param>
        /// <param name="displayLineRenderer">Line renderer used for curve display</param>
        /// <param name="travel">Travel game object for run based views</param>
        /// <param name="arcTravel">Arc travel game object for run based views</param>
        /// <param name="scalingFactor">Scaling factor</param>
        /// <param name="type">Type of controller</param>
        public CurveViewController(Transform root, LineRenderer displayLineRenderer, Transform travel, 
            Transform arcTravel, float scalingFactor, CurveControllerType type) : base(root)
        {
            _travelObject = travel;
            _arcLengthTravelObject = arcTravel;

            var rootPosition = RootElement.position;
            var simpleView = new SimpleCurveView(displayLineRenderer, rootPosition, scalingFactor, type);
            var simpleRunView = new SimpleRunCurveView(displayLineRenderer, rootPosition, scalingFactor, _travelObject, type);
            var simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(displayLineRenderer, rootPosition, 
                scalingFactor, _travelObject, _arcLengthTravelObject, type);
        
            Views = new List<AbstractCurveView>
            {
                simpleView,
                simpleRunView,
                simpleRunWithArcLengthView
            };
        
            foreach (var view in Views)
            {
                RawUpdateViewsDelegate += view.UpdateView;
            }

            UpdateViewsDelegate();

            var initViewIndex = 0;
        
            if (GlobalDataModel.InitFile.DisplayCurves.Count > 0)
            {
                initViewIndex = GlobalDataModel.InitFile.DisplayCurves[0].CurveSettings.DisplaySettings.View switch
                {
                    "simple" => 0,
                    "run" => 1,
                    "arc" => 2,
                    _ => initViewIndex
                };
            }
        
            SwitchView(initViewIndex);
        }
        
        #endregion Constructors

        #region Public functions
        
        /// <summary>
        /// Switch to view associated with the given index
        /// </summary>
        /// <param name="index">View index</param>
        public new void SwitchView(int index)
        {
            // Only allow valid index values
            if (index < 0 || index >= Views.Count) return;
        
            base.SwitchView(index);
            
            // Show travel objects if current view allows them
            _travelObject.gameObject.SetActive( CurrentView.HasTravelPoint);
            _arcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthTravelPoint);
        }
    
        /// <summary>
        /// Starts run on all views associated with this controller
        /// </summary>
        public void StartRun()
        {
            // Start view runs
            foreach (var view in Views)
            {
                view.StartRun();
            }
        
            // Set global marker
            GlobalDataModel.IsRunning = true;        
        }
        
        #endregion Public functions
    }
}