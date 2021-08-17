using HTC.UnityPlugin.ColliderEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractButtonCollisionHandler : MonoBehaviour,
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverEnterHandler,
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverExitHandler
{
    // Start is called before the first frame update
    public GameObject Target;

    public Material PressedMat;
    private Material InitMat;


    public Action OnHitFunc;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if (msr != null)
        {
            InitMat = msr.material;
        }
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        OnHitFunc();

        //WorldStateController world = Target.GetComponent<WorldStateController>();
        //if (world != null)
        //{
        //    if (!GlobalData.IsDriving)
        //    {
        //        OnHitFunc(); //world.StartRun();
        //    }

        //}

        //MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        //if (msr != null)
        //{
        //    //if (msr.material == InitMat)
        //    msr.material = PressedMat;
        //}
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if (msr != null)
        {
            //if (msr.material == PressedMat)
            msr.material = InitMat;
        }
    }

}
