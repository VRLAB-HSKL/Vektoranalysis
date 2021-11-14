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


        private Vector3 initButtonPosition = Vector3.negativeInfinity;

        public Vector3 ButtonPos
        {
            get
            {
                if (initButtonPosition == Vector3.negativeInfinity)
                {
                    initButtonPosition = ButtonObject.localPosition;
                    Debug.Log("initButtonPos: " + initButtonPosition);
                }

                return initButtonPosition;
            }
        }
        


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
                    ButtonObject.localPosition = ButtonPos + ButtonDownDisplacement;
                    Debug.Log("enterNewButtonPos: " + ButtonObject.localPosition);
                    ButtonTriggered = true;
                }
            }
            else
            {
                ButtonObject.localPosition = ButtonPos + ButtonDownDisplacement;
                Debug.Log("enterNewButtonPos: " + ButtonObject.localPosition);
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
            ButtonObject.localPosition = ButtonPos - ButtonDownDisplacement;
            
            Debug.Log("exitNewButtonPos: " + ButtonObject.localPosition);
            
            ButtonTriggered = false;
        }


        void Start()
        {
            // initButtonPosition = ButtonObject.localPosition;
            // Debug.Log("initButtonPos: " + initButtonPosition);
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
}
