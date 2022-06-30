using Controller;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRKL.MBU;

namespace UI.States
{
    /// <summary>
    /// Abstract class representing a state related to the in-game curve selection menu
    /// </summary>
    public abstract class AbstractCurveSelectionState : State 
    {
        #region Private members

        /// <summary>
        /// World instance
        /// </summary>
        private static WorldStateController _world;
        
        /// <summary>
        /// Container the buttons will be placed in
        /// </summary>
        private static GameObject _curveMenuContent;
        
        /// <summary>
        /// Prefab used to generate buttons
        /// </summary>
        private static GameObject _curveMenuButtonPrefab;

        #endregion Private members
        
        #region Constructors

        protected AbstractCurveSelectionState(
            GameObject menuContent, GameObject prefab, WorldStateController world)
        {
            _curveMenuContent = menuContent;
            _curveMenuButtonPrefab = prefab;
            _world = world;
        }
        
        #endregion Constructors

        #region Public functions
        
        /// <summary>
        /// Called on entering the state
        /// </summary>
        public override void OnStateEntered()
        {
            switch (GlobalDataModel.CurrentDisplayGroup)
            {
                case GlobalDataModel.CurveDisplayGroup.Display:
                    GlobalDataModel.WorldCurveViewController?.SetViewVisibility(true);
                    break;
                case GlobalDataModel.CurveDisplayGroup.Exercises:
                    GlobalDataModel.ExerciseCurveController.SetViewVisibility(true);
                    break;
            }

            // Create buttons        
            for (var i = 0; i < GlobalDataModel.CurrentDataset.Count; i++)
            {
                var pds = GlobalDataModel.CurrentDataset[i];
                var tmpButton = Object.Instantiate(_curveMenuButtonPrefab, _curveMenuContent.transform);

                tmpButton.name = pds.Name + "Button";

                var img = tmpButton.GetComponentInChildren<RawImage>();
                if (pds.MenuButtonImage != null)
                {
                    img.texture = pds.MenuButtonImage;
                }

                var label = tmpButton.GetComponentInChildren<TextMeshProUGUI>();
                label.text = pds.DisplayString;

                var b = tmpButton.GetComponent<Button>();
                b.onClick.AddListener(() => _world.SwitchToSpecificDataset(pds.Name));
            }

            
        }

        /// <summary>
        /// Called on leaving the state
        /// </summary>
        public override void OnStateQuit()
        {
            // Hide all views
            GlobalDataModel.WorldCurveViewController.SetViewVisibility(false);
            GlobalDataModel.ExerciseCurveController.SetViewVisibility(false);
        
            // Clear old buttons
            var children = new GameObject[_curveMenuContent.transform.childCount];
            for (var i = 0; i < _curveMenuContent.transform.childCount; i++)
            {
                var child = _curveMenuContent.transform.GetChild(i).gameObject;
                children[i] = child;
            }

            foreach (var child in children)
            {
                Object.DestroyImmediate(child);
            }
        }
        
        #endregion Public functions
    }
}
