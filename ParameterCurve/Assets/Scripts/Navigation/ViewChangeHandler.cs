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
            world.WorldViewController.SwitchView(ViewIndex);
            world.TableViewController?.SwitchView(ViewIndex);
        }

        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if (msr != null)
        {
            msr.material = PressedMat;
        }
    }
}
