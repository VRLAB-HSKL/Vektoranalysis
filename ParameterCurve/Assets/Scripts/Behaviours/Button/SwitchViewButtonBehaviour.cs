using Controller;

namespace Behaviours.Button
{
    public class SwitchViewButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;
        public int viewIndex;

        
        private new void Start()
        {
            base.Start();
            gameObject.SetActive(GlobalData.initFile.ApplicationSettings.TableSettings.ShowViewButtons);
        }


        protected override void HandleButtonEvent()
        {        
            GlobalData.WorldCurveViewController.SwitchView(viewIndex);
            world.TableCurveViewController?.SwitchView(viewIndex);
        }
    }
}
