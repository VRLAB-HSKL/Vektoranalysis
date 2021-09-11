namespace Behaviours.Button
{
    public class SwitchViewButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;
        public int viewIndex;

        public override void HandleButtonEvent()
        {        
            world.WorldViewController.SwitchView(viewIndex);
            world.TableViewController.SwitchView(viewIndex);
        }
    }
}
