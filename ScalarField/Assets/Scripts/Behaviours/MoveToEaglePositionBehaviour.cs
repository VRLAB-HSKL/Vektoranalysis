using UnityEngine;
using VR.Scripts.Behaviours.Button;

namespace Behaviours
{
    public class MoveToEaglePositionBehaviour : AbstractButtonBehaviour
    {
        [Header("Move Objects")]
        public GameObject Cockpit;
        public GameObject User;
        private Transform EaglePoint;
        
        private void Start()
        {
            Cockpit ??= GameObject.Find("UserConsole");
            User ??= GameObject.Find("ViveRig");
            EaglePoint = GameObject.Find("EaglePoint").transform;
        }
        
        protected override void HandleButtonEvent()
        {
            var position = EaglePoint.position;
            
            Cockpit.transform.position = position;
            Cockpit.transform.rotation = EaglePoint.rotation;
            
            User.transform.position = position;
        }
    }
}