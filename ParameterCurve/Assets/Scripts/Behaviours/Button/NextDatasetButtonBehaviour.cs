
namespace Behaviours.Button
{
    public class NextDatasetButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;
    
        public override void HandleButtonEvent()
        {
            world.SwitchToNextDataset();
        }
    }
}
