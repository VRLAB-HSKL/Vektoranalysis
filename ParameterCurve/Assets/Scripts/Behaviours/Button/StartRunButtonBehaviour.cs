
using Controller;

namespace Behaviours.Button
{
    public class StartRunButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;
        
        
        private new void Start()
        {
            base.Start();
            gameObject.SetActive(GlobalData.initFile.ApplicationSettings.TableSettings.ShowRunButton);
        }


        protected override void HandleButtonEvent()
        {
            WorldStateController.StartRun();
        }
    }
}
