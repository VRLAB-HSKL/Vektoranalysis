using Controller;
using Model;

namespace Behaviour.Button
{
    /// <summary>
    /// Button Behaviour used to switch to the previous curve in the current dataset
    /// </summary>
    public class PreviousCurveButtonBehaviour : AbstractButtonBehaviour
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
        protected new void Start()
        {
            base.Start();
            gameObject.SetActive(GlobalDataModel.InitFile.ApplicationSettings.TableSettings.ShowNavButtons);
        }

        /// <summary>
        /// Switches to the previous curve when the button is activated
        /// </summary>
        protected override void HandleButtonEvent()
        {
            world.SwitchToPreviousDataset();
        }
    }
}
