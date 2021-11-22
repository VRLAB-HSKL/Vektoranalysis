using Controller.Curve;
using Controller.Exercise;
using HTC.UnityPlugin.Vive;
using log4net;
using Model;
using UI;
using UnityEngine;
using Views.Display;

namespace Controller
{
    /// <summary>
    /// Controls global game state of the scene. This class initializes important utilities like the global data
    /// model <see cref="GlobalData"/>, and contains the main game loop. Additionally, game objects related to
    /// the different curve displays in the scene are mapped from the scene into the code. 
    /// </summary>
    public class WorldStateController : MonoBehaviour
    {
        #region Public members
        
        /// <summary>
        /// Controls pose logging for every frame. If true, the player pose is logged using the log4net logger object
        /// </summary>
        [Header("Options")]
        public bool activatePoseLogging;
        
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
        
        /// <summary>
        /// Game object transform used to visualize runs along the world curve display
        /// </summary>
        public Transform worldTravelObject;
        
        /// <summary>
        /// Game object transform used to visualize arc length parametrization based runs along the world curve display
        /// </summary>
        public Transform worldArcLengthTravelObject;
        
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
        private static readonly ILog Log = LogManager.GetLogger(typeof(WorldStateController));

        /// <summary>
        /// Time passed since last traversal to the current point. Once this value is greater than
        /// <see cref="GlobalData.RunSpeedFactor"/>, the travel object moves to the next point on the curve
        /// </summary>
        private float _updateTimer;
        
        #endregion Private members
        
        #region Public functions

        /// <summary>
        /// Start run on views on curves that have a travel object associated with them
        /// </summary>
        public static void StartRun()
        {
            Log.Info("Starting curve run");
            GlobalData.IsRunning = true;        
            GlobalData.WorldCurveViewController.StartRun();
            GlobalData.TableCurveViewController?.StartRun();
        }

        /// <summary>
        /// Switch to the next curve in the current dataset <see cref="GlobalData.CurrentDataset"/>
        /// </summary>
        public void SwitchToNextDataset()
        {
            // Stop driving
            if(GlobalData.IsRunning)
            {
                GlobalData.IsRunning = false;            
            }

            // Increment data set index, reset to 0 on overflow
            ++GlobalData.CurrentCurveIndex;
            if (GlobalData.CurrentCurveIndex >= GlobalData.CurrentDataset.Count)
                GlobalData.CurrentCurveIndex = 0;

            
            var worldView = GlobalData.WorldCurveViewController.CurrentView;
            worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
            if(worldView.GetType() == typeof(SimpleRunCurveView) ||
               worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
            {
                if (worldView is SimpleRunCurveView runView)
                {
                    runView.CurrentPointIndex = 0;
                    runView.SetTravelObjectPoint();
                    runView.SetMovingFrame();
                    runView.CurrentPointIndex = 0;
                }

                if (worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    if (worldView is SimpleRunCurveWithArcLength arcView)
                    {
                        arcView.CurrentPointIndex = 0;
                        arcView.SetArcTravelPoint();
                        arcView.SetArcMovingFrame();
                        arcView.CurrentPointIndex = 0;
                    }
                }
            
            }
        
            var tableView = GlobalData.TableCurveViewController?.CurrentView;
            if (tableView != null)
            {
                tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
                if (tableView.GetType() == typeof(SimpleRunCurveView) ||
                    tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    // Debug.Log("adjust travel game object");
                    if (tableView is SimpleRunCurveView runView)
                    {
                        runView.CurrentPointIndex = 0;
                        runView.SetTravelObjectPoint();
                        runView.SetMovingFrame();
                        runView.CurrentPointIndex = 0;
                    }

                    if (tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                    {
                        if (tableView is SimpleRunCurveWithArcLength arcView)
                        {
                            arcView.CurrentPointIndex = 0;
                            arcView.SetArcTravelPoint();
                            arcView.SetArcMovingFrame();
                            arcView.CurrentPointIndex = 0;
                        }
                    }
                }
            }

            // Display html resource
            browserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
        
            // Update info wall
            infoWall.Update();

            GlobalData.WorldCurveViewController.UpdateViewsDelegate();
            GlobalData.TableCurveViewController?.CurrentView.UpdateView();
        
        }

        /// <summary>
        /// Switch to the previous curve in the current dataset <see cref="GlobalData.CurrentDataset"/>
        /// </summary>
        public void SwitchToPreviousDataset()
        {
            // Stop driving
            if (GlobalData.IsRunning)
            {
                if(GlobalData.WorldCurveViewController.CurrentView is null)
                {
                    Log.Warn("ViewEmpty");
                }

                GlobalData.IsRunning = false;
            }

            // Decrement data set index, reset to last element on negative index
            --GlobalData.CurrentCurveIndex;
            if (GlobalData.CurrentCurveIndex < 0)
                GlobalData.CurrentCurveIndex = GlobalData.CurrentDataset.Count - 1;
        
            var worldView = GlobalData.WorldCurveViewController.CurrentView;
            if (worldView != null)
            {
                worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
                if (worldView.GetType() == typeof(SimpleRunCurveView) ||
                    worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    if (worldView is SimpleRunCurveView runView)
                    {
                        runView.CurrentPointIndex = 0;
                        runView.SetTravelObjectPoint();
                        runView.SetMovingFrame();
                        runView.CurrentPointIndex = 0;
                    }

                    if (worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                    {
                        if (worldView is SimpleRunCurveWithArcLength arcView)
                        {
                            arcView.CurrentPointIndex = 0;
                            arcView.SetArcTravelPoint();
                            arcView.SetArcMovingFrame();
                            arcView.CurrentPointIndex = 0;
                        }
                    }
                }
            }

            var tableView = GlobalData.TableCurveViewController?.CurrentView;
            if (tableView != null)
            {
                tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
                if (tableView.GetType() == typeof(SimpleRunCurveView) ||
                    tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    if (tableView is SimpleRunCurveView runView)
                    {
                        runView.CurrentPointIndex = 0;
                        runView.SetTravelObjectPoint();
                        runView.SetMovingFrame();
                        runView.CurrentPointIndex = 0;
                    }

                    if (tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                    {
                        if (tableView is SimpleRunCurveWithArcLength arcView)
                        {
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
            browserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
            infoWall.Update();

            //WorldViewController.CurrentView.UpdateView();
            GlobalData.WorldCurveViewController.UpdateViewsDelegate();
            GlobalData.TableCurveViewController?.CurrentView.UpdateView();
        }

        public void SwitchToSpecificDataset(string datasetIdentifier)
        {
            // Stop driving
            if (GlobalData.IsRunning)
            {
                GlobalData.IsRunning = false;
            }

            if(GlobalData.CurrentDisplayGroup == GlobalData.CurveDisplayGroup.Exercises)
                GlobalData.ExerciseCurveController.SetViewVisibility(true);
        
            var index = GlobalData.CurrentDataset.FindIndex(
                x => x.Name.Equals(datasetIdentifier));
            if (index == -1) return;

            GlobalData.CurrentCurveIndex = index;

            var worldView = GlobalData.WorldCurveViewController.CurrentView;
            worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;


            var tableView = GlobalData.TableCurveViewController?.CurrentView;
            if (tableView != null)
            {
                tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
            }
                
            // Display html resource
            browserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
            infoWall.Update();

            GlobalData.WorldCurveViewController.UpdateViewsDelegate();
            GlobalData.TableCurveViewController?.CurrentView.UpdateView();
        }
        
        #endregion Public functions
        
        #region Private functions

        /// <summary>
        /// Unity Awake function
        /// ====================
        /// 
        /// This function is called when the script instance is loaded. This is used to prepare the global data model
        /// <see cref="GlobalData"/> before any gameplay happens. All future instantiation procedures should therefore
        /// be done in this function.
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
            browserWall.OpenURL(GlobalData.DisplayCurveDatasets[GlobalData.CurrentCurveIndex].NotebookURL);

            // Set plot line renderers
            infoWall.Update();
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
            if (GlobalData.IsRunning)
            {
                // Update time since last point step
                _updateTimer += Time.deltaTime;
                Log.Debug("deltaTime: " + Time.deltaTime + 
                          ", updateTimer: " + _updateTimer + 
                          ", pointStepDuration: " + GlobalData.RunSpeedFactor +
                          " - " + (_updateTimer >= GlobalData.RunSpeedFactor));
                
                // If the time threshold has been reached, traverse to next point
                if(_updateTimer >= GlobalData.RunSpeedFactor)
                {
                    _updateTimer = 0f;
                    GlobalData.WorldCurveViewController.CurrentView.UpdateView();
                    GlobalData.TableCurveViewController?.CurrentView.UpdateView();

                    infoWall.Update();
                }
            }

            // Start run on trigger press
            if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
            {
                Log.Debug("Run started with controller button!");
                StartRun();
            }
        
            // Log pose if activated
            if (activatePoseLogging)
            {
                StaticLogging();
            }

        }

        /// <summary>
        /// Initialize global data model <see cref="GlobalData"/>
        /// </summary>
        private void InitializeModel()
        {
            // Initialize info wall plot lengths first because the rendered size of some game objects
            // is needed in the model calculation
            infoWall.InitPlotLengths();
            
            // Initialize global model
            GlobalData.InitializeData();
        }

        /// <summary>
        /// Initialize all view controllers in the scene
        /// </summary>
        private void InitializeViewControllers()
        {
            // World display curve
            var displayCurve = GlobalData.DisplayCurveDatasets[0];
            GlobalData.WorldCurveViewController = new CurveViewController(
                worldRootElement, worldDisplayLr, 
                worldTravelObject, worldArcLengthTravelObject, 
                displayCurve.WorldScalingFactor, AbstractCurveViewController.CurveControllerType.World);
            GlobalData.WorldCurveViewController.SetViewVisibility(true);

            // Table display curve (if activated)
            if (GlobalData.InitFile.ApplicationSettings.TableSettings.Activated)
            {
                GlobalData.TableCurveViewController = new CurveViewController(
                    tableRootElement, tableDisplayLr, 
                    tableTravelObject, tableArcLengthTravelObject, displayCurve.TableScalingFactor, 
                    AbstractCurveViewController.CurveControllerType.Table);
            }
            else
            {
                tableParent.SetActive(false);
            }

            // Exercise controller
            GlobalData.ExerciseCurveController = new ExerciseCurveViewController(
                selObjects.gameObject.transform, selObjects, pillarPrefab, AbstractCurveViewController.CurveControllerType.World);
            GlobalData.ExerciseCurveController.SetViewVisibility(false);    
        }
        
    
        /// <summary>
        /// Static logging operations that are performed every frame, regardless of user interaction
        /// </summary>
        private static void StaticLogging()
        {
            // Head pose
            var headPose = VivePose.GetPoseEx(BodyRole.Head);
            Log.Info("Head position: " + headPose.pos);
            Log.Info("Head rotation: " + headPose.rot);
            Log.Info("Head up: " + headPose.up);
            Log.Info("Head forward: " + headPose.forward);
            Log.Info("Head right: " + headPose.right);

            // Left hand pose
            var leftPose = VivePose.GetPoseEx(HandRole.LeftHand);
            Log.Info("Left hand position: " + leftPose.pos);
            Log.Info("Left hand rotation: " + leftPose.rot);
            Log.Info("Left hand up: " + leftPose.up);
            Log.Info("Left hand forward: " + leftPose.forward);
            Log.Info("Left hand right: " + leftPose.right);
        
            // Right hand pose
            var rightPose = VivePose.GetPoseEx(HandRole.RightHand);
            Log.Info("Right hand position: " + rightPose.pos);
            Log.Info("Right hand rotation: " + rightPose.rot);
            Log.Info("Right hand up: " + rightPose.up);
            Log.Info("Right hand forward: " + rightPose.forward);
            Log.Info("Right hand right: " + rightPose.right);
        }

        #endregion Private functions
    }
}