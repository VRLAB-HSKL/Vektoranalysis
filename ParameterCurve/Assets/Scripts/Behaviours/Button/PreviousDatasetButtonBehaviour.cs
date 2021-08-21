
public class PreviousDatasetButtonBehaviour : AbstractButtonBehaviour
{
    public WorldStateController world;

    public override void HandleButtonEvent()
    {
        world.SwitchToPreviousDataset();
    }
}
