using Controller.Curve;
using Controller.Exercise;
using HTC.UnityPlugin.Vive;
//using log4net;
using Model;
using UI;
using UnityEngine;
using Views.Display;
using System.IO;
using System.Text.RegularExpressions;

namespace Controller
{
    /// <summary>
    /// Controls global game state of the scene. This class initializes important utilities like the global data
    /// model <see cref="GlobalDataModel"/>, and contains the main game loop. Additionally, game objects related to
    /// the different curve displays in the scene are mapped from the scene into the code. 
    /// </summary>
    public class WorldStateController : MonoBehaviour
    {
        #region Public members
        
        /// <summary>
        /// Controls pose logging for every frame. If true, the player pose is logged using the log4net logger object
        /// </summary>
        //[Header("Options")]
        //public bool activatePoseLogging;
        
        /// <summary>
        /// Root element position of the world curve display. This should be an empty parent game object containing
        /// all related game objects as children
        /// </summary>
        [Header("WorldCurveElements")]
        public Transform worldRootElement;
        
        /// <summary>
        /// Line renderer displaying the world curve
        /// </summary>
        public LineRenderer worldDisplayLr;

        public TubeMesh worldDisplayMesh;
        
        
        /// <summary>
        /// Game object transform used to visualize runs along the world curve display
        /// </summary>
        public Transform worldTravelObject;
        
        /// <summary>
        /// Game object transform used to visualize arc length parametrization based runs along the world curve display
        /// </summary>
        public Transform worldArcLengthTravelObject;

        /// <summary>
        /// Display for drawing tangent and normal lines
        /// </summary>
        public DrawTangentNormal drawDisplay;

        /// <summary>
        /// Root element containing the table prefab and all other related game objects. This differs from
        /// <see cref="tableRootElement"/> by containing all elements, not just the ones related to simple curve
        /// display
        /// </summary>
        [Header("TableCurveElements")]
        public GameObject tableParent;
        
        /// <summary>
        /// Root element position of the table curve display. This should be an empty parent game object containing
        /// all related game objects as children
        /// </summary>
        public Transform tableRootElement;
        
        /// <summary>
        /// Line renderer displaying the table curve
        /// </summary>
        public LineRenderer tableDisplayLr;

        public TubeMesh tableDisplayMesh;
        
        /// <summary>
        /// Game object transform used to visualize runs along the table curve display
        /// </summary>
        public Transform tableTravelObject;
        
        /// <summary>
        /// Game object transform used to visualize arc length parametrization based runs along the table curve display
        /// </summary>
        public Transform tableArcLengthTravelObject;

        /// <summary>
        /// Exercise script instance <see cref="SelectionExerciseGameObjects"/> bundling all relevant game object
        /// related to selection exercise interaction
        /// </summary>
        [Header("ExerciseElements")] 
        public SelectionExerciseGameObjects selObjects;
        
        /// <summary>
        /// Prefab used to instance pillars containing curves to be selected by the user
        /// </summary>
        public GameObject pillarPrefab;
    
        /// <summary>
        /// Control for room wall that uses an in-game browser asset to display websites or local html resources
        /// </summary>
        [Header("Walls")]
        public BrowserControl browserWall;
        
        /// <summary>
        /// Control for room wall that displays general curve information and current point information during runs
        /// </summary>
        public InformationControl infoWall;
        
        /// <summary>
        /// Control for room wall used to select curves to display from the imported datasets
        /// </summary>
        public static CurveSelectionControl CurveSelectWall;

        #endregion Public members

        #region Private members
        
        /// <summary>
        /// Static log4net Logger 
        /// </summary>
        //private static readonly ILog Log = LogManager.GetLogger(typeof(WorldStateController));

        /// <summary>
        /// Time passed since last traversal to the current point. Once this value is greater than
        /// <see cref="GlobalDataModel.RunSpeedFactor"/>, the travel object moves to the next point on the curve
        /// </summary>
        private float _updateTimer;

        /// <summary>
        /// Path to file containing point data for curve, sent to cockpit.
        /// </summary>
        private string pathToData = "Assets/Resources/linecoords.txt";

        #endregion Private members

        #region Public functions

        /// <summary>
        /// Start run on views on curves that have a travel object associated with them
        /// </summary>
        public static void StartRun()
        {
            //Log.Info("Starting curve run");
            GlobalDataModel.IsRunning = true;        
            GlobalDataModel.WorldCurveViewController.StartRun();
            GlobalDataModel.TableCurveViewController?.StartRun();
        }

        /// <summary>
        /// Switch to the next curve in the current dataset <see cref="GlobalDataModel.CurrentDataset"/>
        /// </summary>
        public void SwitchToNextDataset()
        {
            // Stop driving
            if(GlobalDataModel.IsRunning)
            {
                GlobalDataModel.IsRunning = false;            
            }

            // Increment data set index, reset to 0 on overflow
            ++GlobalDataModel.CurrentCurveIndex;
            if (GlobalDataModel.CurrentCurveIndex >= GlobalDataModel.CurrentDataset.Count)
                GlobalDataModel.CurrentCurveIndex = 0;

            switch (GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].View)
            {
                default:
                    GlobalDataModel.WorldCurveViewController.SwitchView(0);
                    GlobalDataModel.TableCurveViewController.SwitchView(0);
                    break;
                    
                case "run":
                    GlobalDataModel.WorldCurveViewController.SwitchView(1);
                    GlobalDataModel.TableCurveViewController.SwitchView(1);
                    break;
                    
                case "arc":
                    GlobalDataModel.WorldCurveViewController.SwitchView(2);
                    GlobalDataModel.TableCurveViewController.SwitchView(2);
                    break;
            }
            
            var worldView = GlobalDataModel.WorldCurveViewController.CurrentView;
            worldView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].WorldScalingFactor;
            if(worldView.GetType() == typeof(SimpleRunCurveView) ||
               worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
            {
                if (worldView is SimpleRunCurveView runView)
                {
                    runView._wpm.ResetWaypoints();
                    
                    runView.CurrentPointIndex = 0;
                    runView.SetTravelObjectPoint();
                    runView.SetMovingFrame();
                    runView.CurrentPointIndex = 0;
                }

                if (worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    if (worldView is SimpleRunCurveWithArcLength arcView)
                    {
                        arcView._wpm.ResetWaypoints();
                        arcView._arcWpm.ResetWaypoints();
                        
                        arcView.CurrentPointIndex = 0;
                        arcView.SetArcTravelPoint();
                        arcView.SetArcMovingFrame();
                        arcView.CurrentPointIndex = 0;
                    }
                }
            
            }
        
            var tableView = GlobalDataModel.TableCurveViewController?.CurrentView;
            if (tableView != null)
            {
                tableView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TableScalingFactor;
                if (tableView.GetType() == typeof(SimpleRunCurveView) ||
                    tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    // Debug.Log("adjust travel game object");
                    if (tableView is SimpleRunCurveView runView)
                    {
                        runView._wpm.ResetWaypoints();
                        runView.CurrentPointIndex = 0;
                        runView.SetTravelObjectPoint();
                        runView.SetMovingFrame();
                        runView.CurrentPointIndex = 0;
                    }

                    if (tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                    {
                        if (tableView is SimpleRunCurveWithArcLength arcView)
                        {
                            arcView._wpm.ResetWaypoints();
                            arcView._arcWpm.ResetWaypoints();
                            arcView.CurrentPointIndex = 0;
                            arcView.SetArcTravelPoint();
                            arcView.SetArcMovingFrame();
                            arcView.CurrentPointIndex = 0;
                        }
                    }
                }
            }

            // Display html resource
            browserWall.OpenURL(GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].NotebookURL);
        
            // Update info wall
            infoWall.Update();

            //GlobalDataModel.WorldCurveViewController.UpdateViewsDelegate();
            GlobalDataModel.WorldCurveViewController.CurrentView.UpdateView();
            GlobalDataModel.TableCurveViewController?.CurrentView.UpdateView();
            
            //tableDisplayMesh.GenerateFieldMesh();
        }

        /// <summary>
        /// Switch to the previous curve in the current dataset <see cref="GlobalDataModel.CurrentDataset"/>
        /// </summary>
        public void SwitchToPreviousDataset()
        {
            // Stop driving
            if (GlobalDataModel.IsRunning)
            {
                if(GlobalDataModel.WorldCurveViewController.CurrentView is null)
                {
                    //Log.Warn("ViewEmpty");
                }

                GlobalDataModel.IsRunning = false;
            }

            // Decrement data set index, reset to last element on negative index
            --GlobalDataModel.CurrentCurveIndex;
            if (GlobalDataModel.CurrentCurveIndex < 0)
                GlobalDataModel.CurrentCurveIndex = GlobalDataModel.CurrentDataset.Count - 1;
        
            switch (GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].View)
            {
                default:
                    GlobalDataModel.WorldCurveViewController.SwitchView(0);
                    GlobalDataModel.TableCurveViewController.SwitchView(0);
                    break;
                    
                case "run":
                    GlobalDataModel.WorldCurveViewController.SwitchView(1);
                    GlobalDataModel.TableCurveViewController.SwitchView(1);
                    break;
                    
                case "arc":
                    GlobalDataModel.WorldCurveViewController.SwitchView(2);
                    GlobalDataModel.TableCurveViewController.SwitchView(2);
                    break;
            }
            
            var worldView = GlobalDataModel.WorldCurveViewController.CurrentView;
            if (worldView != null)
            {
                worldView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].WorldScalingFactor;
                if (worldView.GetType() == typeof(SimpleRunCurveView) ||
                    worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    if (worldView is SimpleRunCurveView runView)
                    {
                        runView._wpm.ResetWaypoints();
                        runView.CurrentPointIndex = 0;
                        runView.SetTravelObjectPoint();
                        runView.SetMovingFrame();
                        runView.CurrentPointIndex = 0;
                    }

                    if (worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                    {
                        if (worldView is SimpleRunCurveWithArcLength arcView)
                        {
                            arcView._wpm.ResetWaypoints();
                            arcView._arcWpm.ResetWaypoints();
                            arcView.CurrentPointIndex = 0;
                            arcView.SetArcTravelPoint();
                            arcView.SetArcMovingFrame();
                            arcView.CurrentPointIndex = 0;
                        }
                    }
                }
            }

            var tableView = GlobalDataModel.TableCurveViewController?.CurrentView;
            if (tableView != null)
            {
                var rootPos = tableView._rootPos;
                //
                // GlobalDataModel.TableCurveViewController.
                
                tableView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TableScalingFactor;
                if (tableView.GetType() == typeof(SimpleRunCurveView) ||
                    tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    if (tableView is SimpleRunCurveView runView)
                    {
                        runView._wpm.ResetWaypoints();
                        runView.CurrentPointIndex = 0;
                        runView.SetTravelObjectPoint();
                        runView.SetMovingFrame();
                        runView.CurrentPointIndex = 0;
                    }

                    if (tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                    {
                        if (tableView is SimpleRunCurveWithArcLength arcView)
                        {
                            arcView._wpm.ResetWaypoints();
                            arcView._arcWpm.ResetWaypoints();
                            arcView.CurrentPointIndex = 0;
                            arcView.SetArcTravelPoint();
                            arcView.SetArcMovingFrame();
                            arcView.CurrentPointIndex = 0;
                        }
                    }
                }
            }

            // tableView.UpdateView();
        
            // Display html resource
            browserWall.OpenURL(GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].NotebookURL);
            infoWall.Update();

            
            GlobalDataModel.WorldCurveViewController.UpdateViewsDelegate();
            GlobalDataModel.WorldCurveViewController.CurrentView.UpdateView();
            GlobalDataModel.TableCurveViewController?.CurrentView.UpdateView();
            
            //tableDisplayMesh.GenerateFieldMesh();
        }

        /// <summary>
        /// Switch to a specific curve in the current dataset <see cref="GlobalDataModel.CurrentDataset"/>
        /// based on given identifier
        /// </summary>
        public void SwitchToSpecificDataset(string datasetIdentifier)
        {
            // Stop driving
            if (GlobalDataModel.IsRunning)
            {
                GlobalDataModel.IsRunning = false;
            }

            if(GlobalDataModel.CurrentDisplayGroup == GlobalDataModel.CurveDisplayGroup.Exercises)
                GlobalDataModel.ExerciseCurveController.SetViewVisibility(true);
        
            var index = GlobalDataModel.CurrentDataset.FindIndex(
                x => x.Name.Equals(datasetIdentifier));
            if (index == -1) return;

            GlobalDataModel.CurrentCurveIndex = index;

            switch (GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].View)
            {
                default:
                    GlobalDataModel.WorldCurveViewController.SwitchView(0);
                    GlobalDataModel.TableCurveViewController.SwitchView(0);
                    break;
                    
                case "run":
                    GlobalDataModel.WorldCurveViewController.SwitchView(1);
                    GlobalDataModel.TableCurveViewController.SwitchView(1);
                    break;
                    
                case "arc":
                    GlobalDataModel.WorldCurveViewController.SwitchView(2);
                    GlobalDataModel.TableCurveViewController.SwitchView(2);
                    break;
            }
            
            var worldView = GlobalDataModel.WorldCurveViewController.CurrentView;
            worldView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].WorldScalingFactor;

            var tableView = GlobalDataModel.TableCurveViewController?.CurrentView;
            if (tableView != null)
            {
                tableView.ScalingFactor = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TableScalingFactor;
            }
                
            // Display html resource
            browserWall.OpenURL(GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].NotebookURL);
            infoWall.Update();

            GlobalDataModel.WorldCurveViewController.UpdateViewsDelegate();
            GlobalDataModel.WorldCurveViewController.CurrentView?.UpdateView();
            GlobalDataModel.TableCurveViewController?.CurrentView.UpdateView();
            
            //tableDisplayMesh.GenerateFieldMesh();
        }

        /// <summary>
        /// Switch to a specific exercise in the current exercise dataset based on given identifier
        /// </summary>
        public void SwitchToSpecificExercise(string exerciseIdentifier)
        {
            // Stop driving
            if (GlobalDataModel.IsRunning)
            {
                GlobalDataModel.IsRunning = false;
            }

            if (GlobalDataModel.CurrentDisplayGroup == GlobalDataModel.CurveDisplayGroup.Exercises)
                GlobalDataModel.ExerciseCurveController.SetViewVisibility(true);

            var index = GlobalDataModel.SelectionExercises.FindIndex(
                x => x.Title.Equals(exerciseIdentifier));

            if (index == -1) return;
            else if(GlobalDataModel.CurrentExerciseIndex == index) GlobalDataModel.ExerciseCurveController.ResetCurrentExercise();
            else
            {
                GlobalDataModel.CurrentExerciseIndex = index;
                GlobalDataModel.ExerciseCurveController.NewExercise();
            }
            
            GlobalDataModel.ExerciseCurveController.SetViewVisibility(true);
        }

        #endregion Public functions

        #region Private functions

        /// <summary>
        /// Unity Awake function
        /// ====================
        /// 
        /// This function is called when the script instance is loaded. This is used to prepare the global data model
        /// <see cref="GlobalDataModel"/> before any gameplay happens. All future instantiation procedures should there-
        /// fore be done in this function.
        ///
        /// </summary>
        private void Awake()
        {
            InitializeModel();
            InitializeViewControllers();
        }
        
        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update, after <see cref="Awake"/>
        /// </summary>
        private void Start()
        {
            // Display html resource
            browserWall.OpenURL(GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex].NotebookURL);

            // Set plot line renderers
            infoWall.Update();

            CheckForCurve();
        }

        /// <summary>
        /// Unity Update function
        /// =====================
        ///
        /// Core game loop, is called once per frame
        /// 
        /// </summary>
        private void Update()
        {
            // During run
            if (GlobalDataModel.IsRunning)
            {
                // Update time since last point step
                _updateTimer += Time.deltaTime;
                //Log.Debug("deltaTime: " + Time.deltaTime + 
                //          ", updateTimer: " + _updateTimer + 
                //          ", pointStepDuration: " + GlobalDataModel.RunSpeedFactor +
                //          " - " + (_updateTimer >= GlobalDataModel.RunSpeedFactor));
                
                // If the time threshold has been reached, traverse to next point
                if(_updateTimer >= GlobalDataModel.RunSpeedFactor)
                {
                    _updateTimer = 0f;
                    GlobalDataModel.WorldCurveViewController.CurrentView.UpdateView();
                    GlobalDataModel.TableCurveViewController?.CurrentView.UpdateView();

                    infoWall.Update();
                }
            }

            // Start run on trigger press
            if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Grip))
            {
                //Log.Debug("Run started with controller button!");
                StartRun();
            }
        
            // // Log pose if activated
            // if (activatePoseLogging)
            // {
            //     StaticLogging();
            // }

        }

        /// <summary>
        /// Initialize global data model <see cref="GlobalDataModel"/>
        /// </summary>
        private void InitializeModel()
        {
            // Initialize info wall plot lengths first because the rendered size of some game objects
            // is needed in the model calculation
            infoWall.InitPlotLengths();
            
            // Initialize global model
            GlobalDataModel.InitializeData();
        }

        /// <summary>
        /// Initialize all view controllers in the scene
        /// </summary>
        private void InitializeViewControllers()
        {
            // World display curve
            var displayCurve = GlobalDataModel.DisplayCurveDatasets[0];
            GlobalDataModel.WorldCurveViewController = new CurveViewController(
                worldRootElement, worldDisplayLr, worldDisplayMesh, 
                worldTravelObject, worldArcLengthTravelObject, 
                displayCurve.WorldScalingFactor, AbstractCurveViewController.CurveControllerType.World);
            GlobalDataModel.WorldCurveViewController.SetViewVisibility(true);
            
            GlobalDataModel.WorldCurveViewController.CurrentView.UpdateView();
            
            // Table display curve (if activated)
            if (GlobalDataModel.InitFile.ApplicationSettings.TableSettings.Activated)
            {
                GlobalDataModel.TableCurveViewController = new CurveViewController(
                    tableRootElement, tableDisplayLr, tableDisplayMesh,
                    tableTravelObject, tableArcLengthTravelObject, displayCurve.TableScalingFactor, 
                    AbstractCurveViewController.CurveControllerType.Table);

                GlobalDataModel.TableCurveViewController.CurrentView.UpdateView();
                
                // // Add global position of parent object into points
                // var lrPositions = new Vector3[tableDisplayLr.positionCount]; 
                // tableDisplayLr.GetPositions(lrPositions);
                //
                // var newPositions = new Vector3[tableDisplayLr.positionCount];
                // for(var i = 0; i < lrPositions.Length; i++)
                // {
                //     newPositions[i] = lrPositions[i] + tableDisplayLr.transform.position;
                // }
                //
                // tableDisplayLr.SetPositions(newPositions);
            }
            else
            {
                tableParent.SetActive(false);
            }

            // Exercise controller
            GlobalDataModel.ExerciseCurveController = new ExerciseCurveViewController(
                selObjects.gameObject.transform, selObjects, pillarPrefab, AbstractCurveViewController.CurveControllerType.World);
            GlobalDataModel.ExerciseCurveController.SetViewVisibility(false);    
        }

        private void CheckForCurve()
        {
            //pathToData = Application.Path Resources.Load() "Assets/Resources/linecoords.txt"; 
            var file = Resources.Load<TextAsset>("linecoords.txt");
            if (file)
            {
                var lines = Regex.Split(file.text, "\n|\r|\r\n");
                if (lines.Length > 0)
                {
                    SwitchToSpecificDataset(lines[0]);
                }
            }
            
            // using (StreamReader reader = new StreamReader(pathToData))
            // {
            //     string name = reader.ReadLine();
            //
            //     //will be null if this is the first time the scene is being loaded
            //     //will contain name of curve that was just run in the cockpit scene if not null
            //     if (name != null)
            //     {
            //         //if curve does not exist, function will just return and load regularly
            //         SwitchToSpecificDataset(name);  
            //     }
            // }
        }
    
        

        #endregion Private functions
    }
}