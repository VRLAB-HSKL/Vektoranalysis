using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VR.Scripts.Behaviours.Button;

public class ButtonEventBehaviour : AbstractButtonBehaviour
{
    [Header("Event")]
    public UnityEvent invokeMethod;
    
    protected override void HandleButtonEvent()
    {
        invokeMethod.Invoke();
    }
}
