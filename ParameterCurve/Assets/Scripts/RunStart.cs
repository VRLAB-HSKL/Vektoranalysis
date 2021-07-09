using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunStart : MonoBehaviour, 
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverEnterHandler,
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverExitHandler

{
    public GameObject Target;

    public Material PressedMat;
    private Material InitMat;
    

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
        ParamCurve pm = Target.GetComponent<ParamCurve>();        
        if(pm != null)
        {
            if(!pm.IsDriving)
            {
                pm.StartRun();
            }
            
        }

        MeshRenderer msr = Target.GetComponent<MeshRenderer>();
        if(msr != null)
        {
            //if (msr.material == InitMat)
                msr.material = PressedMat;
        }
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




    // Update is called once per frame
    void Update()
    {
        
    }


}
