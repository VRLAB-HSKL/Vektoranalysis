using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

namespace Controller
{
    public class GraphDisplayControl : MonoBehaviour
    {

        public GameObject TimeVelocityDiagramCanvas;
        public GameObject TimeDistanceDiagramCanvas;
        private bool diagramsVisible;

        // Start is called before the first frame update
        void Start()
        {
            diagramsVisible = true;
        }

        // Update is called once per frame
        void Update()
        {
            //toggle on/off time and velocity diagrams when bottom of touch pad is pressed
            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.DPadDown))
            {
                //Debug.Log("right dpad down");
                if (diagramsVisible)
                {
                    TimeDistanceDiagramCanvas.SetActive(false);
                    TimeVelocityDiagramCanvas.SetActive(false);
                    diagramsVisible = !diagramsVisible;
                }
                else
                {
                    TimeDistanceDiagramCanvas.SetActive(true);
                    TimeVelocityDiagramCanvas.SetActive(true);
                    diagramsVisible = !diagramsVisible;
                }
            }
        }
    }
}
