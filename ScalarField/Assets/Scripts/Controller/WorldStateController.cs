using Model.ScriptableObjects;
using Travel;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Global world state controller to initialize the scene correctly and handle global operations triggered by the user
    /// </summary>
    public class WorldStateController : MonoBehaviour
    {
        public ViewControllerManager ViewControllerManager;
        public ScalarFieldManager ScalarFieldManager;
    
        public GameObject FieldMesh;
        public GameObject FieldBoundingBox;
    
        public MapPlacement TableMap;
        public InformationControl InfoWall;
        public CreateColorScale ColorScale;
    
        /// <summary>
        /// Single awake in application to ensure init file was parsed
        /// ToDo: Alternatively, just place it at the front of the script execution order in unity editor settings
        /// </summary>
        private void Awake()
        {
            InitializeModel();
        }

        // [RuntimeInitializeOnLoadMethod]
        // private void OnRuntimeMethodLoad()
        // {
        //     // Set index to 0 on application start
        //     ScalarFieldManager.CurrentFieldIndex = 0;
        // }
    
    
        /// <summary>
        /// Initializes the data model of the application 
        /// </summary>
        private void InitializeModel()
        {
            // Parse scalar fields data into scriptable object
            //ScalarFieldManager.ParseInitFile();
        
            InitializeViewControllers();
        }

    
    
        private void InitializeViewControllers()
        {
            ViewControllerManager.FieldViewController = 
                new FieldViewController(ScalarFieldManager, FieldMesh, FieldBoundingBox);
        }

        private void UpdateRoom()
        {
            InfoWall.UpdateInformation();
            TableMap.SetTexture();
            ColorScale.UpdateScale();
        }

        public void NextDataset()
        {
            SetNextValidIndex(true);

            ViewControllerManager.FieldViewController.UpdateViews();
        
            UpdateRoom();
        }

        public void PreviousDataset()
        {
            SetNextValidIndex(false);

            ViewControllerManager.FieldViewController.UpdateViews();
        
            UpdateRoom();
        }
    
        /// <summary>
        /// Move to the next valid field index in the imported dataset
        /// </summary>
        /// <param name="isIncrement">Signals whether field index is in-/decremented when applicable</param>
        private void SetNextValidIndex(bool isIncrement)
        {
            int newIndex;
            var oldIndex = ScalarFieldManager.CurrentFieldIndex;
            if (oldIndex == 0 && !isIncrement)
            {
                // Debug.Log("reset to count - 1");
                newIndex = ScalarFieldManager.ScalarFields.Count - 1;
            }
            else if (oldIndex == ScalarFieldManager.ScalarFields.Count - 1 && isIncrement)
            {
                // Debug.Log("reset to 0");
                newIndex = 0;
            }
            else
            {
                if (isIncrement)
                {
                    //Debug.Log("increment");
                    newIndex = ScalarFieldManager.CurrentFieldIndex + 1;
                }
                else
                {
                    //Debug.Log("decrement");
                    newIndex = ScalarFieldManager.CurrentFieldIndex - 1;
                }
            }

            //Debug.Log("oldIndex: " + oldIndex + ", newIndex: " + newIndex + ", isIncrement:" + isIncrement);
        
            ScalarFieldManager.CurrentFieldIndex = newIndex;
            //Debug.Log("sf_index: " + ScalarFieldManager.CurrentFieldIndex);
        }
    
    }
}