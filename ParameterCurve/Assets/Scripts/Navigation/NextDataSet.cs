using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;

public class NextDataSet : MonoBehaviour,
    HTC.UnityPlugin.ColliderEvent.IColliderEventHoverEnterHandler
{
    public GameObject Target;


    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        WorldStateController world = Target.GetComponent<WorldStateController>();
        if(world != null)
        {
            world.SwitchToNextDataset();
        }
    }
}
