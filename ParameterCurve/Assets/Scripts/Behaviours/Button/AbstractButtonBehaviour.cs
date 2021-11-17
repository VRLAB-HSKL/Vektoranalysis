using System.Runtime.CompilerServices;
using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviours.Button
{
    public abstract class AbstractButtonBehaviour : 
        MonoBehaviour, 
        IColliderEventPressEnterHandler, 
        IColliderEventPressExitHandler
    {
        [SerializeField]
        protected ColliderButtonEventData.InputButton mActiveButton = ColliderButtonEventData.InputButton.Trigger;
    
        /// <summary>
        /// Controls how much the actual object being pressed will be displaced while pressing
        /// </summary>
        public Vector3 ButtonDownDisplacement = new Vector3(0f, -0.02f, 0f);    

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

        protected Vector3 initButtonPosition;

        


        /// <summary>
        /// Handles the buttonpress when the object is entered
        /// </summary>
        /// <param name="eventData"></param>
        public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
        {
            if (UseTriggerButton)
            {
                if (eventData.button == mActiveButton)
                {
                    ButtonObject.position = initButtonPosition + ButtonDownDisplacement;
                    Debug.Log("enterNewButtonPos: " + ButtonObject.position + ", initPos: " + initButtonPosition);
                    ButtonTriggered = true;
                }
            }
            else
            {
                ButtonObject.position = initButtonPosition + ButtonDownDisplacement;
                Debug.Log("enterNewButtonPos: " + ButtonObject.position + ", initPos: " + initButtonPosition);
                ButtonTriggered = true;
            }

        
        }

        /// <summary>
        /// Handles the button press when the object is exited
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
            ButtonObject.localPosition = initButtonPosition - ButtonDownDisplacement;
            
            Debug.Log("exitNewButtonPos: " + ButtonObject.position + ", initPos: " + initButtonPosition);
            // Debug.Log("exitNewButtonPos: " + ButtonObject.localPosition);
            
            ButtonTriggered = false;
        }


        public void Start()
        {
            initButtonPosition = ButtonObject.position;
            Debug.Log("initButtonPos: " + initButtonPosition);
        }
            
        /// <summary>
        /// Update is called once per frame 
        /// </summary>
        public void Update()
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
}
