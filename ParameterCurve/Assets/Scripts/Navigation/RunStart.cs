using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
using UnityEngine;


public class RunStart : AbstractButtonCollisionHandler
{

    public RunStart()
    {
        OnHitFunc = HandleCollision;
    }

    private void HandleCollision()
    {
        //Debug.Log("RunStartHit!");

        WorldStateController world = Target.GetComponent<WorldStateController>();
        if (world != null)
        {
            if (!GlobalDataModel.IsRunning)
            {
                //world.StartRun();
            }

        }

        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if (msr != null)
        {
            //if (msr.material == InitMat)
            msr.material = PressedMat;
        }
    }


    //public GameObject Target;

    //public Material PressedMat;
    //private Material InitMat;
    

    //// Start is called before the first frame update
    //void Start()
    //{
    //    MeshRenderer msr = Target.GetComponent<MeshRenderer>();
    //    if (msr != null)
    //    {
    //        InitMat = msr.material;
    //    }
    //}

    //public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    //{
    //    WorldStateController world = Target.GetComponent<WorldStateController>();        
    //    if(world != null)
    //    {
    //        if(!GlobalData.IsDriving)
    //        {
    //            world.StartRun();
    //        }
            
    //    }

    //    MeshRenderer msr = Target.GetComponent<MeshRenderer>();
    //    if(msr != null)
    //    {
    //        //if (msr.material == InitMat)
    //            msr.material = PressedMat;
    //    }
    //}

    //public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    //{
    //    MeshRenderer msr = Target.GetComponent<MeshRenderer>();
    //    if (msr != null)
    //    {
    //        //if (msr.material == PressedMat)
    //            msr.material = InitMat;
    //    }
    //}




    // Update is called once per frame
    void Update()
    {
        
    }


}
