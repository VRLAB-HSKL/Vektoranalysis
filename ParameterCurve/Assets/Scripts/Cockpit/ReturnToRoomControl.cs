using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

namespace Controller
{
    public class ReturnToRoomControl : MonoBehaviour
    {

        public GameObject ExitConfirmationPanel;
        public GameObject YesButton;
        public GameObject CancelButton;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.DPadDown))
            {
                
            }
        }
    }
}
