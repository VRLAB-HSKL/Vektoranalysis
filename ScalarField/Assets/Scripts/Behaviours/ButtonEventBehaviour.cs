using UnityEngine;
using UnityEngine.Events;
using VR.Scripts.Behaviours.Button;

// ToDo: Move this to MBVR package

namespace Behaviours
{
    /// <summary>
    /// Abstract button class that uses the inspector to assign concrete function from scripts currently active in the
    /// unity scene instead of using own subclass
    /// </summary>
    public class ButtonEventBehaviour : AbstractButtonBehaviour
    {
        /// <summary>
        /// Attached script function that will be called on button press
        /// </summary>
        [Header("Event"), Tooltip("Attach script instance and choose function")]
        public UnityEvent invokeMethod;
    
        /// <summary>
        /// Calls the function that was attached in the inspector of the unity editor
        /// </summary>
        protected override void HandleButtonEvent()
        {
            invokeMethod.Invoke();
        }
    }
}
