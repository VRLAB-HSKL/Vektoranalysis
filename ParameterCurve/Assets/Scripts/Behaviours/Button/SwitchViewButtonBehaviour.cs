using Controller;

namespace Behaviours.Button
{
    public class SwitchViewButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;
        public int viewIndex;

        
        private void Start()
        {
            gameObject.SetActive(GlobalData.initFile.ApplicationSettings.TableSettings.ShowViewButtons);
        }
        
        
        public override void HandleButtonEvent()
        {        
            GlobalData.WorldCurveViewController.SwitchView(viewIndex);
            world.TableCurveViewController?.SwitchView(viewIndex);
        }
    }
}
