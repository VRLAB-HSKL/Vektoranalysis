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
