using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDataSet : MonoBehaviour,
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverEnterHandler
{
    public GameObject Target;

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        ParamCurve pm = Target.GetComponent<ParamCurve>();
        if(pm != null)
        {
            pm.SwitchToNextDataset();
        }
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
