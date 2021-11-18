using System.Collections.Generic;
using UnityEngine;
using Views.Display;

namespace Controller.Curve
{
    public enum CurveControllerTye { World = 0, Table = 1 };

    public class CurveViewController : AbstractCurveViewController
    {
        // ToDo:
        // Replace single static line renderer with dynamic creation of line renderer segments for each edge
        // between two points. This should remove visual bugs on the curve when the curves are scaled very small.
        private readonly Transform _travelObject;
        private readonly Transform _arcLengthTravelObject;
    
        public CurveViewController(Transform root, LineRenderer displayLineRenderer, Transform travel, 
            Transform arcTravel, float scalingFactor, CurveControllerTye type) : base(root)
        {
            _travelObject = travel;
            _arcLengthTravelObject = arcTravel;

            var rootPosition = _rootElement.position;
            var simpleView = new SimpleCurveView(displayLineRenderer, rootPosition, scalingFactor, type);
            var simpleRunView = new SimpleRunCurveView(displayLineRenderer, rootPosition, scalingFactor, _travelObject, type);
            var simpleRunWithArcLengthView = new SimpleRunCurveWithArcLength(displayLineRenderer, rootPosition, 
                scalingFactor, _travelObject, _arcLengthTravelObject, type);
        
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

            var initViewIndex = 0;
        
            if (GlobalData.initFile.DisplayCurves.Count > 0)
            {
                initViewIndex = GlobalData.initFile.DisplayCurves[0].CurveSettings.DisplaySettings.View switch
                {
                    "simple" => 0,
                    "run" => 1,
                    "arc" => 2,
                    _ => initViewIndex
                };
            }
        
            SwitchView(initViewIndex);
        }

        public new void SwitchView(int index)
        {
            if (index < 0 || index >= _views.Count) return;
        
            base.SwitchView(index);
            _travelObject.gameObject.SetActive( CurrentView.HasTravelPoint);
            _arcLengthTravelObject.gameObject.SetActive(CurrentView.HasArcLengthTravelPoint);
        }
    
        public void StartRun()
        {
            foreach (var view in _views)
            {
                view.StartRun();
            }
        
            GlobalData.IsRunning = true;        
        }
    }
}