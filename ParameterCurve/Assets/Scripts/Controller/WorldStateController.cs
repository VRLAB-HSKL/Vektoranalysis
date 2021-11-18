using Controller.Curve;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using log4net;
using UI;
using UnityEngine;
using Views.Display;

namespace Controller
{
    public class WorldStateController : MonoBehaviour
    {
    
        /// <summary>
        /// Controls pose logging for every frame
        /// </summary>
        [Header("Options")]
        public bool activatePoseLogging;
    
         
        
        [Header("WorldCurveElements")]
        public Transform worldRootElement;
        public LineRenderer worldDisplayLr;
        public Transform worldTravelObject;
        public Transform worldArcLengthTravelObject;

        [Header("TableCurveElements")] 
        public GameObject tableParent;
        public Transform tableRootElement;
        public LineRenderer tableDisplayLr;
        public Transform tableTravelObject;
        public Transform tableArcLengthTravelObject;

        [Header("ExerciseElements")] 
        public SelectionExerciseGameObjects selObjects;
        public GameObject pillarPrefab;
    
        [Header("Walls")]
        public BrowserControl browserWall;
        public InformationControl infoWall;
        public static CurveSelectionControl CurveSelectWall;

    
        public CurveViewController TableCurveViewController { get; private set; }
        

        
        
        /// <summary>
        /// Static log4net Logger 
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(WorldStateController));

        private float _pointStepDuration;
        private float _updateTimer;
        
    
        private void Awake()
        {
            // Initialize global model
            infoWall.InitPlotLengths();
            GlobalData.InitializeData();
            
            // Set up view controllers
            GlobalData.ExerciseCurveController = new ExerciseCurveViewController(
                selObjects.gameObject.transform, selObjects, pillarPrefab, CurveControllerTye.World);
            GlobalData.ExerciseCurveController.SetViewVisibility(false);

            var displayCurve = GlobalData.DisplayCurveDatasets[0];

            GlobalData.WorldCurveViewController = new CurveViewController(
                worldRootElement, worldDisplayLr, 
                worldTravelObject, worldArcLengthTravelObject, 
                displayCurve.WorldScalingFactor, CurveControllerTye.World);
            GlobalData.WorldCurveViewController.SetViewVisibility(true);

            if (GlobalData.initFile.ApplicationSettings.TableSettings.Activated)
            {
                TableCurveViewController = new CurveViewController(
                    tableRootElement, tableDisplayLr, 
                    tableTravelObject, tableArcLengthTravelObject, displayCurve.TableScalingFactor, 
                    CurveControllerTye.Table);
            }
            else
            {
                tableParent.SetActive(false);
            }

            _pointStepDuration = GlobalData.RunSpeedFactor;

        }

        // Start is called before the first frame update
        private void Start()
        {
            // Display html resource
            browserWall.OpenURL(GlobalData.DisplayCurveDatasets[GlobalData.CurrentCurveIndex].NotebookURL);

            // Set plot line renderers
            infoWall.Update();
        }

        

        public WorldStateController(Vector3 initTravelObjPos, Vector3 initArcLengthTravelObjPos)
        {
            // _initTravelObjPos = initTravelObjPos;
            // _initArcLengthTravelObjPos = initArcLengthTravelObjPos;
        }

        private void Update()
        {
            if (GlobalData.IsRunning)
            {
                _updateTimer += Time.deltaTime;
                Log.Debug("deltaTime: " + Time.deltaTime + 
                          ", updateTimer: " + _updateTimer + 
                          ", pointStepDuration: " + _pointStepDuration +
                          " - " + (_updateTimer >= _pointStepDuration));
                
                if(_updateTimer >= _pointStepDuration)
                {
                    _updateTimer = 0f;
                
                    //GlobalData.WorldViewController.UpdateViewsDelegate();
                    GlobalData.WorldCurveViewController.CurrentView.UpdateView();
                    TableCurveViewController?.CurrentView.UpdateView();

                    infoWall.Update();
                }

                // Debug.Log("crP: " + GlobalData.CurrentPointIndex + 
                //           ", max: " + GlobalData.DisplayCurveDatasets[GlobalData.CurrentCurveIndex].points.Count);
                // if (GlobalData.CurrentPointIndex >= GlobalData.DisplayCurveDatasets[GlobalData.CurrentCurveIndex].points.Count)
                // {
                //     Debug.Log("Stopping run");
                //     GlobalData.IsRunning = false;
                // }
            }

            // Start run on trigger press
            if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
            {
                Debug.Log("Run started with controller button!");
                StartRun();
            }
        
            if(activatePoseLogging) StaticLogging();
            

        }

        public void StartRun()
        {
            //Log.Info("Starting curve run...");
            GlobalData.IsRunning = true;        
            GlobalData.WorldCurveViewController.StartRun();
            TableCurveViewController.StartRun();
        }

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

            // Reset point index
            //GlobalData.CurrentPointIndex = 0;

            var worldView = GlobalData.WorldCurveViewController.CurrentView as AbstractCurveView;
            worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
            if(worldView.GetType() == typeof(SimpleRunCurveView) ||
               worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
            {
                // Debug.Log("adjust travel gameobject");
                var runview = worldView as SimpleRunCurveView;
                runview.CurrentPointIndex = 0;
                runview.SetTravelPoint();
                runview.SetMovingFrame();
                runview.CurrentPointIndex = 0;

                if (worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    var arcview = worldView as SimpleRunCurveWithArcLength;
                    arcview.CurrentPointIndex = 0;
                    arcview.SetArcTravelPoint();
                    arcview.SetArcMovingFrame();
                    arcview.CurrentPointIndex = 0;
                }
            
            }
        
            // worldView.UpdateView();
        
            var tableView = TableCurveViewController.CurrentView as AbstractCurveView;
            tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
            if(tableView.GetType() == typeof(SimpleRunCurveView) ||
               tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
            {
                // Debug.Log("adjust travel gameobject");
                var runview = tableView as SimpleRunCurveView;
                runview.CurrentPointIndex = 0;
                runview.SetTravelPoint();
                runview.SetMovingFrame();
                runview.CurrentPointIndex = 0;
            
                if (tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    var arcview = tableView as SimpleRunCurveWithArcLength;
                    arcview.CurrentPointIndex = 0;
                    arcview.SetArcTravelPoint();
                    arcview.SetArcMovingFrame();
                    arcview.CurrentPointIndex = 0;
                }
            
            }
        
            // Display html resource
            browserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
        
            // Udpate info wall
            infoWall.Update();

            GlobalData.WorldCurveViewController.UpdateViewsDelegate();
            TableCurveViewController?.CurrentView.UpdateView();
        
        }

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

            // Reset point index
            //GlobalData.CurrentPointIndex = 0;
        
            var worldView = GlobalData.WorldCurveViewController.CurrentView as AbstractCurveView;
            worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
            if(worldView.GetType() == typeof(SimpleRunCurveView) ||
               worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
            {
                Debug.Log("adjust travel gameobject");
                var runview = worldView as SimpleRunCurveView;
                runview.CurrentPointIndex = 0;
                runview.SetTravelPoint();
                runview.SetMovingFrame();
                runview.CurrentPointIndex = 0;
            
                if (worldView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    var arcview = worldView as SimpleRunCurveWithArcLength;
                    arcview.CurrentPointIndex = 0;
                    arcview.SetArcTravelPoint();
                    arcview.SetArcMovingFrame();
                    arcview.CurrentPointIndex = 0;
                }
            }
        
            // worldView.UpdateView();
        
            var tableView = TableCurveViewController.CurrentView as AbstractCurveView;
            tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
            if(tableView.GetType() == typeof(SimpleRunCurveView) || 
               tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
            {
                Debug.Log("adjust travel gameobject");
                var runview = tableView as SimpleRunCurveView;
                runview.CurrentPointIndex = 0;
                runview.SetTravelPoint();
                runview.SetMovingFrame();
                runview.CurrentPointIndex = 0;
            
                if (tableView.GetType() == typeof(SimpleRunCurveWithArcLength))
                {
                    var arcview = tableView as SimpleRunCurveWithArcLength;
                    arcview.CurrentPointIndex = 0;
                    arcview.SetArcTravelPoint();
                    arcview.SetArcMovingFrame();
                    arcview.CurrentPointIndex = 0;
                }
            }
        
            // tableView.UpdateView();
        
            // Display html resource
            browserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
            infoWall.Update();

            //WorldViewController.CurrentView.UpdateView();
            GlobalData.WorldCurveViewController.UpdateViewsDelegate();
            TableCurveViewController?.CurrentView.UpdateView();
        }

        public void SwitchToSpecificDataset(string name)
        {
            // Stop driving
            if (GlobalData.IsRunning)
            {
                GlobalData.IsRunning = false;
            }

            if(GlobalData.CurrentDisplayGroup == GlobalData.CurveDisplayGroup.Exercises)
                GlobalData.ExerciseCurveController.SetViewVisibility(true);
        
            var index = GlobalData.CurrentDataset.FindIndex(x => x.Name.Equals(name));
            if (index == -1) return;

            GlobalData.CurrentCurveIndex = index;

            // Reset point index
            //GlobalData.CurrentPointIndex = 0;

            if (GlobalData.WorldCurveViewController.CurrentView is AbstractCurveView worldView)
            {
                worldView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].WorldScalingFactor;
            }

            if (TableCurveViewController?.CurrentView is AbstractCurveView tableView)
            {
                tableView.ScalingFactor = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].TableScalingFactor;
            }
        
            // Display html resource
            browserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);
            infoWall.Update();

            GlobalData.WorldCurveViewController.UpdateViewsDelegate();
            TableCurveViewController?.CurrentView.UpdateView();
        }

    
        /// <summary>
        /// Static logging operations that are performed every frame, regardless of user interaction
        /// </summary>
        private static void StaticLogging()
        {
            // Head pose
            RigidPose headPose = VivePose.GetPoseEx(BodyRole.Head);
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

    }
}