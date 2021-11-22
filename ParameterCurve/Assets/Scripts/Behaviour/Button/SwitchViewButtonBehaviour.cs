using Controller;
using Model;

namespace Behaviour.Button
{
    /// <summary>
    /// Button Behaviour used to switch to another view on curve displays.
    /// </summary>
    public class SwitchViewButtonBehaviour : AbstractButtonBehaviour
    {
        /// <summary>
        /// Single world state controller instance <see cref="WorldStateController"/>
        /// </summary>
        public WorldStateController world;
        
        /// <summary>
        /// View identifier. Used to provide general marker to specify which view should be switched to
        /// </summary>
        public int viewIndex;

        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update
        /// </summary>
        private new void Start()
        {
            base.Start();
            gameObject.SetActive(GlobalData.InitFile.ApplicationSettings.TableSettings.ShowViewButtons);
        }

        /// <summary>
        /// Switches to another view on curve displays, based on <see cref="viewIndex"/>
        /// </summary>
        protected override void HandleButtonEvent()
        {        
            GlobalData.WorldCurveViewController.SwitchView(viewIndex);
            GlobalData.TableCurveViewController?.SwitchView(viewIndex);
        }
    }
}
