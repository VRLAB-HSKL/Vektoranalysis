using System.Collections;
using System.Collections.Generic;
using Controller;
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
            GlobalData.WorldCurveViewController.SwitchView(ViewIndex);
            world.TableCurveViewController?.SwitchView(ViewIndex);
        }

        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if (msr != null)
        {
            msr.material = PressedMat;
        }
    }
}
