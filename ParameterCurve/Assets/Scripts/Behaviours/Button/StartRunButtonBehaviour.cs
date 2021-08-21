
public class StartRunButtonBehaviour : AbstractButtonBehaviour
{
    public WorldStateController world;
    public override void HandleButtonEvent()
    {
        world.StartRun();
    }
}
