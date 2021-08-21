using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchViewButtonBehaviour : AbstractButtonBehaviour
{
    public WorldStateController world;
    public int ViewIndex;

    public override void HandleButtonEvent()
    {        
        switch (ViewIndex)
        {
            default:
            case 0:
                world.CurrentView = world.simpleView;
                break;

            case 1:
                world.CurrentView = world.simpleRunView;
                break;

            case 2:
                world.CurrentView = world.simpleRunWithArcLengthView;
                break;
        }
    }
}
