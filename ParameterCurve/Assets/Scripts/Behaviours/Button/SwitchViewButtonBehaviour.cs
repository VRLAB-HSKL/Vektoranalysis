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
            GlobalData.WorldViewController.SwitchView(viewIndex);
            world.TableViewController?.SwitchView(viewIndex);
        }
    }
}
