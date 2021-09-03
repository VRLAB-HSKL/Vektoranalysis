namespace Behaviours.Button
{
    public class SwitchViewButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;
        public int viewIndex;

        public override void HandleButtonEvent()
        {        
            switch (viewIndex)
            {
                default:
                    
                    world.WorldViewController.CurrentView = world.WorldViewController.simpleView;
                    world.TableViewController.CurrentView = world.TableViewController.simpleView;
                    break;

                case 1:
                    world.WorldViewController.CurrentView = world.WorldViewController.simpleRunView;
                    world.TableViewController.CurrentView = world.TableViewController.simpleRunView;
                    break;

                case 2:
                    world.WorldViewController.CurrentView = world.WorldViewController.simpleRunWithArcLengthView;
                    world.TableViewController.CurrentView = world.TableViewController.simpleRunWithArcLengthView;
                    break;
            }
        }
    }
}
