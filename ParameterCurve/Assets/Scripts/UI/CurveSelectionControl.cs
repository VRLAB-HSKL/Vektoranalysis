using System;
using System.Linq;
using Controller;
using Model;
using TMPro;
using UI.States;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Control class for the curve selection menu
    /// </summary>
    public class CurveSelectionControl : MonoBehaviour
    {
        #region Public members
        
        /// <summary>
        /// Root parent of the main category menu
        /// </summary>
        public GameObject mainMenuParent;
        
        /// <summary>
        /// Parent for all generated main menu buttons
        /// </summary>
        public GameObject mainMenuButtonsParent;
        
        /// <summary>
        /// Main menu button prefab
        /// </summary>
        public GameObject mainMenuButtonPrefab;

        /// <summary>
        /// Root parent of the curve selection sub menu
        /// </summary>
        public GameObject curveMenuParent;
        
        /// <summary>
        /// Parent for all generated curve selection sub menu buttons
        /// </summary>
        public GameObject curveMenuContent;
        
        /// <summary>
        /// Curve selection button prefab
        /// </summary>
        public GameObject curveMenuButtonPrefab;

        /// <summary>
        /// World instance
        /// </summary>
        public WorldStateController world;

        #endregion Public members
        
        #region Private members
        
        /// <summary>
        /// UI state machine
        /// </summary>
        private CurveSelectionStateContext CurveSelectionFsm { get; set; }
        
        /// <summary>
        /// Initial display curves state
        /// </summary>
        private DisplayCurvesState _displayState;
        
        /// <summary>
        /// Initial exercises state
        /// </summary>
        private ExerciseCurvesState _exerciseState;

        #endregion Private members

        #region Private functions
        
        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update, after
        /// <see>
        ///     <cref>Awake</cref>
        /// </see>
        /// </summary>
        private void Start()
        {
            _displayState = new DisplayCurvesState(curveMenuContent, curveMenuButtonPrefab, world);
            _exerciseState = new ExerciseCurvesState(curveMenuContent, curveMenuButtonPrefab, world);
            CurveSelectionFsm = new CurveSelectionStateContext(_displayState);

            if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.Activated)
            {
                mainMenuParent.SetActive(false);
                curveMenuParent.SetActive(false);
                return;
            };
        
            var displayGroups = Enum.GetNames(typeof(GlobalDataModel.CurveDisplayGroup));
        
            var displayGrpValues = 
                (GlobalDataModel.CurveDisplayGroup[])Enum.GetValues(typeof(GlobalDataModel.CurveDisplayGroup));
            for (var i = 0; i < displayGroups.Length; i++)
            {
                // Get current group name
                var curveDisplayGroupName = displayGroups[i];
            
                // Make sure group is activated
                switch (curveDisplayGroupName)
                {
                    case "Display":
                        if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowDisplayCurves ||
                            !GlobalDataModel.DisplayCurveDatasets.Any())
                        {
                            continue;
                        }
                        break;
                
                    case "Exercises":
                        if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowExercises ||
                            !GlobalDataModel.SelectionExercises.Any())
                        {
                            continue;
                        }

                        break;
                }
            
                // Create instance of button prefab
                var dgrpVal = displayGrpValues[i];
                var tmpButton = Instantiate(mainMenuButtonPrefab, mainMenuButtonsParent.transform);
                tmpButton.name = curveDisplayGroupName + "GrpButton";
                Destroy(tmpButton.GetComponent<RawImage>());

                var label = tmpButton.GetComponentInChildren<TextMeshProUGUI>();
                label.text = curveDisplayGroupName;

                var b = tmpButton.GetComponent<Button>();
                switch(i)
                {
                    default:
                    case 0:
                        b.onClick.AddListener(() => SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Display));
                        break;

                    case 1:
                        b.onClick.AddListener(() => SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Exercises));
                        break;
                }
            }

            SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Display);
        }

        private void SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup cdg)
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.Activated) return;
        
            // Update current display group
            GlobalDataModel.CurrentDisplayGroup = cdg;
        
            switch(cdg)
            {
                default:
                case GlobalDataModel.CurveDisplayGroup.Display:
                    if (GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowDisplayCurves)
                        CurveSelectionFsm.State = _displayState;
                    break;

                case GlobalDataModel.CurveDisplayGroup.Exercises:
                    if(GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowExercises)
                        CurveSelectionFsm.State = _exerciseState;
                    break;
            }
        
            // Reset curve and point indices
            GlobalDataModel.CurrentCurveIndex = 0;

            CurveSelectionFsm.State.OnStateUpdate();

            if (world.browserWall is null)
            {
                Debug.Log("Browser Wall not initialized!");
            }

            var cds = GlobalDataModel.CurrentDataset;
            if(cds is null)
            {
                Debug.Log("Datasets not initialized");
            }

            var ds = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];
            if(ds is null)
            {
                Debug.Log("Current dataset is null");
            }
            else
            {
                // Display html resource
                //if (world.BrowserWall is { }) world.BrowserWall.OpenURL(ds.NotebookURL);
            }
        }
        
        #endregion Private functions
    }
}
