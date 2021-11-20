using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;

namespace Behaviours.Button
{
    /// <summary>
    /// Abstract Button Behaviour class for all button behaviours
    ///
    /// Derived classes only have to implement their own version of <see cref="HandleButtonEvent"/>
    /// </summary>
    public abstract class AbstractButtonBehaviour : 
        MonoBehaviour, 
        IColliderEventPressEnterHandler, 
        IColliderEventPressExitHandler
    {
        #region Public members
        
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
        
        #endregion Public members
        
        #region Protected members
        
        /// <summary>
        /// Button used to activate the button <see cref="ColliderButtonEventData.InputButton"/>
        /// </summary>
        [SerializeField]
        protected ColliderButtonEventData.InputButton mActiveButton = ColliderButtonEventData.InputButton.Trigger;
        
        #endregion Protected members
        
        #region Private members
        
        /// <summary>
        /// Signals whether the button is being triggered
        /// </summary>
        private bool _buttonTriggered;

        /// <summary>
        /// Initial position of the cylinder object being pressed down
        /// </summary>
        private Vector3 _initButtonPosition;

        #endregion Private members
        
        #region Public functions
        
        /// <summary>
        /// Handles the button press when the object is entered
        /// </summary>
        /// <param name="eventData"></param>
        public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
        {
            if (useTriggerButton)
            {
                // Check if associated button was pressed
                if (eventData.button != mActiveButton) return;
                
                // Move cylinder down
                buttonObject.localPosition = _initButtonPosition + buttonDownDisplacement;
                _buttonTriggered = true;
            }
            else
            {
                // Move cylinder down
                buttonObject.localPosition = _initButtonPosition + buttonDownDisplacement;
                _buttonTriggered = true;
            }
        }

        /// <summary>
        /// Handles the button press when the object is exited
        /// </summary>
        /// <remarks>
        /// Behaviour when the Button is pressed:
        /// <ul>
        ///     <li>Resets the button position </li>
        ///     <li>Disables the UpWard-Movement</li>
        /// </ul> 
        /// </remarks>
        /// <param name="eventData"></param>
        /// <returns>void</returns>
        public void OnColliderEventPressExit(ColliderButtonEventData eventData)
        {
            // Move cylinder up
            buttonObject.localPosition = _initButtonPosition - buttonDownDisplacement;
            _buttonTriggered = false;
        }

        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update
        /// </summary>
        protected void Start()
        {
            _initButtonPosition = buttonObject.localPosition;
        }
            
        /// <summary>
        /// Unity Update function
        /// =====================
        ///
        /// Core game loop, is called once per frame
        /// </summary>
        public void Update()
        {
            // Abort if button wasn't triggered
            if (!_buttonTriggered) return;
            
            HandleButtonEvent();
            if (!holdButton) 
            {
                _buttonTriggered = false;
            }

        }

        #endregion Public functions
        
        #region Protected functions
        
        /// <summary>
        /// Procedure that is executed when the button gets activated. Has to be implemented by the derived class.
        /// </summary>
        protected abstract void HandleButtonEvent();

        
        
        
        #endregion Protected functions
    }
}
