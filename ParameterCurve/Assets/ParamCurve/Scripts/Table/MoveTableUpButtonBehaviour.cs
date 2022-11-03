using Behaviour.Button;
using UnityEngine;

namespace Table
{
    /// <summary>
    /// This class handles Logic of the Up button 
    /// Source: ImmersiveVolume Project
    /// </summary>
    public class MoveTableUpButtonBehaviour : AbstractButtonBehaviour
        //MonoBehaviour, IColliderEventPressEnterHandler
        //, IColliderEventPressExitHandler
    {

        //[SerializeField]
        //private ColliderButtonEventData.InputButton m_activeButton = ColliderButtonEventData.InputButton.Trigger;

        /// <summary>
        /// Shows how much the whole game object will be displaced while pressing
        /// </summary>
        public Vector3 fullObjectDisplacement;
 
        /// <summary>
        /// The Base of the Console
        /// </summary>
        private GameObject _consoleBase;
        /// <summary>
        /// Side panels without the Sliders
        /// </summary>
        private GameObject _regulator1;
        /// <summary>
        /// Side panels without the Sliders
        /// </summary>
        private GameObject _regulator2;
        /// <summary>
        /// Side panels without the Sliders
        /// </summary>
        private GameObject _regulator3;



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
        /// <returns>void</returns>
        /// 
        private new void Start()
        {
            base.Start();
            
            holdButton = true;
            useTriggerButton = false;
            fullObjectDisplacement = new Vector3(0f, 0.002f, 0f);

            
            
            _consoleBase = GameObject.Find("ConsoleBase");
            _regulator1 = GameObject.Find("Regulator");
            _regulator2 = GameObject.Find("Regulator (1)");
            _regulator3 = GameObject.Find("Regulator (2)");
        }

        protected override void HandleButtonEvent()
        {
            if (true) //getUpwards)
            {
                if (true)//volumeObject != null)
                {

                    if (true) //consoleBase.transform.localPosition.y <= -0.399f)
                    {
                        Vector3 increaseVector = fullObjectDisplacement; // * Time.deltaTime;
                        _consoleBase.transform.localPosition += increaseVector;
                        _regulator1.transform.localPosition += increaseVector;
                        _regulator2.transform.localPosition += increaseVector;
                        _regulator3.transform.localPosition += increaseVector;

                        //volumeObject.transform.localPosition += FullObjectDisplacement * Time.deltaTime;

                        Debug.Log("IncreaseVector: " + increaseVector);
                        //Debug.Log("localPosInc: " + consoleBase.transform.localPosition);

                        //Debug.Log("going up");

                    }



                    /*  if (ConsoleBase.transform.localPosition.y <= -1.3f) {


                      vol_obj.transform.localPosition = new Vector3(vol_obj.transform.localPosition.z, 
                      vol_obj.transform.localPosition.y, vol_obj.transform.localPosition.x);
                      ConsoleBase.transform.localPosition = 
                      new Vector3(ConsoleBase.transform.localPosition.z, -
                      1.3f, 
                      ConsoleBase.transform.localPosition.x);


                  }*/

                }


            }
        }
    }
}

