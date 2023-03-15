# Button Behaviours



In this subfolder we gather all button behaviour classes. Most of them are subclasses of the `AbstractButtonBehaviour` class which implements the visual behaviour of the 3D button object and wires the triggered event on button push to the specific `HandleButtonEvent()` method. This abstract method is implemented individually in each subclass, resulting in small subclasses for the most part only containing their `HandleButtonEvent()` implementation.



## Example: StartRunButtonBehaviour

```cs
public class StartRunButtonBehaviour : AbstractButtonBehaviour
{
    /// <summary>
    /// Single world state controller instance <see cref="WorldStateController"/>
    /// </summary>
    public WorldStateController world;       
    
    /// <summary>
    /// Starts run of travel objects when the button is activated
    /// </summary>
    protected override void HandleButtonEvent()
    {
        WorldStateController.StartRun();
    }
}
```


















