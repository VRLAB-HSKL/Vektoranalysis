
using Controller;

namespace Behaviours.Button
{
    public class StartRunButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;
        
        
        private void Start()
        {
            gameObject.SetActive(GlobalData.initFile.ApplicationSettings.TableSettings.ShowRunButton);
        }
        
        
        public override void HandleButtonEvent()
        {
            world.StartRun();
        }
    }
}
