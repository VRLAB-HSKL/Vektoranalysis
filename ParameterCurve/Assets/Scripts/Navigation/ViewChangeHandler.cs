using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewChangeHandler : AbstractButtonCollisionHandler
{
    public int ViewIndex;

    public ViewChangeHandler()
    {
        OnHitFunc = HandleCollision;
    }

    private void HandleCollision()
    {
        WorldStateController world = Target.GetComponent<WorldStateController>();
        if (world != null)
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

        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if (msr != null)
        {
            msr.material = PressedMat;
        }
    }
}
