using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles Logic of the Upbutton 
/// </summary>
/// 

public class VRUpButton : AbstractButtonBehaviour
    //MonoBehaviour, IColliderEventPressEnterHandler
    //, IColliderEventPressExitHandler
{

    //[SerializeField]
    //private ColliderButtonEventData.InputButton m_activeButton = ColliderButtonEventData.InputButton.Trigger;
    
    /// <summary>
    /// This Vector shows how much the Button will be displaced while pressing
    /// </summary>
    //public Vector3 ButtonDownDisplacement = new Vector3(0f, 5f, 0f);
    
    /// <summary>
    /// This Vector shows how much the 3D-Model will be displaced while pressing
    /// </summary>
    //public Vector3 ObjectDisplacement;
    
    /// <summary>
    /// This is the Buttonobject
    /// </summary>
    //public Transform ButtonObject;
    
    /// <summary>
    /// 
    /// </summary>
    bool getUpwards = false;
    
    /// <summary>
    /// The current 3D-Model 
    /// </summary>
    private Transform volumeObject; //VolumeRenderedObject volumeObject;
    /// <summary>
    /// The Base of the Console
    /// </summary>
    private GameObject consoleBase;
    /// <summary>
    /// Sidepanels without the Sliders
    /// </summary>
    private GameObject regulator1;
    /// <summary>
    /// Sidepanels without the Sliders
    /// </summary>
    private GameObject regulator2;
    /// <summary>
    /// Sidepanels without the Sliders
    /// </summary>
    private GameObject regulator3;



    /// <summary>
    /// Finding the GameObjects in the Scene
    /// </summary>
    /// <remarks>
    /// 
    /// <ul>
    /// <li>Finding the  consoleBase</li>
    /// <li>Finding the left Regulator</li>
    /// <li>Finding the right Regulator</li>
    /// <li>Finding the front Regulator</li>
    /// </ul> 
    /// </remarks>
    /// <param name="void"></param>
    /// <returns>void</returns>
    /// 
    void Start()
    {
        HoldButton = true;
        FullObjectDisplacement = new Vector3(0f, 0.2f, 0f);

        consoleBase = GameObject.Find("ConsoleBase");
        regulator1 = GameObject.Find("Regulator");
        regulator2 = GameObject.Find("Regulator (1)");
        regulator3 = GameObject.Find("Regulator (2)");

    }




    /// <summary>
    /// Handles the buttonpress when the object is entered
    /// </summary>
    /// <remarks>
    /// Behaviour when the Button is pressed:
    /// <ul>
    /// 
    /// <li>The lower part of the Button will be displaced depending on the value of buttonDownDisplacement </li>
    /// <li>Finding the current VolumeObject</li>
    /// <li>Upward-Movement (getUpward) will be enabled</li>
    /// </ul> 
    /// </remarks>
    /// <param name="eventData"></param>
    /// <returns>void</returns>
    //public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    //{
    //    if (eventData.button == m_activeButton)
    //    {
    //        //volumeObject = GameObject.FindObjectOfType<VolumeRenderedObject>();

    //        //Debug.Log("ButtonHit");
    //        ButtonObject.localPosition += ButtonDownDisplacement;
    //        getUpwards = true;





    //    }
    //}

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

    //public void OnColliderEventPressExit(ColliderButtonEventData eventData)
    //{
    //    ButtonObject.localPosition -= ButtonDownDisplacement;
    //    getUpwards = false;
    //}



    // Update is called once per frame


    /// <summary>
    /// Handles the ObjectDisplacement
    /// </summary>
    /// <remarks>
    /// Behaviour when Upward-Movement is enabled, the VolumeObject is not null and the localPosition of the consoleBase is less or equal than -0.399 (Topposition)
    /// <ul>
    /// <li>The ConsoleBase, the Regulators and the 3D-Model wll be displaced depending on objectDisplacement and Time.deltaTime every Frame</li>
    /// </ul> 
    /// </remarks>
    /// <param name="void"></param>
    /// <returns>void</returns>
    /// 
    //void Update()
    //{
        
    //}

    public override void HandleButtonEvent()
    {
        if (true) //getUpwards)
        {
            if (true)//volumeObject != null)
            {

                if (true) //consoleBase.transform.localPosition.y <= -0.399f)
                {
                    Vector3 increaseVector = FullObjectDisplacement; // * Time.deltaTime;
                    

                    consoleBase.transform.localPosition += increaseVector;
                    regulator1.transform.localPosition += increaseVector;
                    regulator2.transform.localPosition += increaseVector;
                    regulator3.transform.localPosition += increaseVector;

                    //volumeObject.transform.localPosition += FullObjectDisplacement * Time.deltaTime;

                    Debug.Log("IncreaseVector: " + increaseVector);
                    //Debug.Log("localPosInc: " + consoleBase.transform.localPosition);

                    //Debug.Log("hoch");

                }



                /*  if (ConsoleBase.transform.localPosition.y <= -1.3f) {


                      volobj.transform.localPosition = new Vector3(volobj.transform.localPosition.z, volobj.transform.localPosition.y, volobj.transform.localPosition.x);
                      ConsoleBase.transform.localPosition = new Vector3(ConsoleBase.transform.localPosition.z, -1.3f, ConsoleBase.transform.localPosition.x);


                  }*/

            }


        }
    }
}

