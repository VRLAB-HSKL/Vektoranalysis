using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractButtonBehaviour : 
    MonoBehaviour, 
    IColliderEventPressEnterHandler, 
    IColliderEventPressExitHandler
{
    [SerializeField]
    protected ColliderButtonEventData.InputButton m_activeButton = ColliderButtonEventData.InputButton.Trigger;
    
    /// <summary>
    /// Controls how much the actual object being pressed will be displaced while pressing
    /// </summary>
    public Vector3 ButtonDownDisplacement = new Vector3(0f, 5f, 0f);    

    /// <summary>
    /// Button object being pressed
    /// </summary>
    public Transform ButtonObject;

    /// <summary>
    /// Controls wether button activation is dependent on pushing the trigger button
    /// or simple collision with the object
    /// </summary>
    public bool UseTriggerButton = false;

    /// <summary>
    /// Signals wether the associated handler function is triggered once (false)
    /// or as long as the button is being pressed (true)
    /// </summary>
    public bool HoldButton = false;


    private bool ButtonTriggered = false;






    /// <summary>
    /// Handles the buttonpress when the object is entered
    /// </summary>
    /// <param name="eventData"></param>
    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        if (UseTriggerButton)
        {
            if (eventData.button == m_activeButton)
            {
                ButtonObject.localPosition += ButtonDownDisplacement;
                ButtonTriggered = true;
            }
        }
        else
        {
            ButtonObject.localPosition += ButtonDownDisplacement;
            ButtonTriggered = true;
        }

        
    }

    /// <summary>
    /// Handles the buttonpress when the object is exited
    /// </summary>
    /// <remarks>
    /// Behaviour when the Button is pressed:
    /// <ul>
    /// 
    /// <li>Resets the Buttonposition </li>
    /// <li>Disables the UpWard-Movement</li>
    /// </ul> 
    /// </remarks>
    /// <param name="eventData"></param>
    /// <returns>void</returns>

    public void OnColliderEventPressExit(ColliderButtonEventData eventData)
    {
        ButtonObject.localPosition -= ButtonDownDisplacement;
        ButtonTriggered = false;
    }



    // Update is called once per frame


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (ButtonTriggered)
        {
            HandleButtonEvent();
            if (!HoldButton) 
            {
                ButtonTriggered = false;
            }
        }
        
    }

    public abstract void HandleButtonEvent();

}
