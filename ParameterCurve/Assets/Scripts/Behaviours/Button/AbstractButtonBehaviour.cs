using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;

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
        public Vector3 buttonDownDisplacement = new Vector3(0f, -0.02f, 0f);    

        /// <summary>
        /// Button object being pressed
        /// </summary>
        public Transform buttonObject;

        /// <summary>
        /// Controls whether button activation is dependent on pushing the trigger button
        /// or simple collision with the object
        /// </summary>
        public bool useTriggerButton;

        /// <summary>
        /// Signals whether the associated handler function is triggered once (false)
        /// or as long as the button is being pressed (true)
        /// </summary>
        public bool holdButton;
        
        private bool _buttonTriggered;

        private Vector3 _initButtonPosition;


        

        /// <summary>
        /// Handles the button press when the object is entered
        /// </summary>
        /// <param name="eventData"></param>
        public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
        {
            if (useTriggerButton)
            {
                if (eventData.button == mActiveButton)
                {
                    buttonObject.localPosition = _initButtonPosition + buttonDownDisplacement;
                    // Debug.Log("enterNewButtonPos: " + ButtonObject.localPosition + ", initPos: " + initButtonPosition);
                    _buttonTriggered = true;
                }
            }
            else
            {
                buttonObject.localPosition = _initButtonPosition + buttonDownDisplacement;
                // Debug.Log("enterNewButtonPos: " + ButtonObject.localPosition + ", initPos: " + initButtonPosition);
                _buttonTriggered = true;
            }

        
        }

        /// <summary>
        /// Handles the button press when the object is exited
        /// </summary>
        /// <remarks>
        /// Behaviour when the Button is pressed:
        /// <ul>
        /// 
        /// <li>Resets the button position </li>
        /// <li>Disables the UpWard-Movement</li>
        /// </ul> 
        /// </remarks>
        /// <param name="eventData"></param>
        /// <returns>void</returns>
        public void OnColliderEventPressExit(ColliderButtonEventData eventData)
        {
            buttonObject.localPosition = _initButtonPosition - buttonDownDisplacement;
            
            // Debug.Log("exitNewButtonPos: " + ButtonObject.localPosition + ", initPos: " + initButtonPosition);
            
            _buttonTriggered = false;
        }

        public void Start()
        {
            _initButtonPosition = buttonObject.localPosition;
            // Debug.Log("initButtonPos: " + initButtonPosition);
        }
            
        /// <summary>
        /// Update is called once per frame 
        /// </summary>
        public void Update()
        {
            if (_buttonTriggered)
            {
                HandleButtonEvent();
                if (!holdButton) 
                {
                    _buttonTriggered = false;
                }
            }
        
        }

        protected abstract void HandleButtonEvent();

    }
}
