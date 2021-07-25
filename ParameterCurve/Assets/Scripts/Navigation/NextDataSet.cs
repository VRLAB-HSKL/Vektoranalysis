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
        WorldStateController pm = Target.GetComponent<WorldStateController>();
        if(pm != null)
        {
            pm.SwitchToNextDataset();
        }
    }
}
