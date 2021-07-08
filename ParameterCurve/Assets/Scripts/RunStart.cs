using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunStart : MonoBehaviour, 
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverEnterHandler,
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverExitHandler

{
    public ParamCurve Target;

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        //ParamCurve pm = Target.GetComponent<ParamCurve>();
        
        if(Target != null)
        {
            if(!Target.IsDriving)
            {
                Target.StartRun();
            }                
        }
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
