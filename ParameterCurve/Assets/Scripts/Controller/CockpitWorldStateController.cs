using Model;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// World state controller for the cockpit scene
    /// </summary>
    public class CockpitWorldStateController : MonoBehaviour
    {
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
            //InitializeViewControllers();
        }
        
        /// <summary>
        /// Initialize global data model <see cref="GlobalDataModel"/>
        /// </summary>
        private void InitializeModel()
        {
            // Initialize info wall plot lengths first because the rendered size of some game objects
            // is needed in the model calculation
            //infoWall.InitPlotLengths();
            
            // Initialize global model
            GlobalDataModel.InitializeData();
        }
        
        
      
        
        
    }
}