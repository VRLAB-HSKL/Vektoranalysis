using Controller;
using Model;

namespace Behaviour.Button
{
    /// <summary>
    /// Button Behaviour used to start runs of travel objects along the curve line
    /// </summary>
    public class StartRunButtonBehaviour : AbstractButtonBehaviour
    {
        /// <summary>
        /// Single world state controller instance <see cref="WorldStateController"/>
        /// </summary>
        public WorldStateController world;
        
        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update
        /// </summary>
        private new void Start()
        {
            base.Start();
            gameObject.SetActive(GlobalDataModel.InitFile.ApplicationSettings.TableSettings.ShowRunButton);
        }

        /// <summary>
        /// Starts run of travel objects when the button is activated
        /// </summary>
        protected override void HandleButtonEvent()
        {
            WorldStateController.StartRun();
        }
    }
}
