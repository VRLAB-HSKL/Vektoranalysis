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
            switch(ViewIndex)
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

        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if (msr != null)
        {
            msr.material = PressedMat;
        }
    }
}
