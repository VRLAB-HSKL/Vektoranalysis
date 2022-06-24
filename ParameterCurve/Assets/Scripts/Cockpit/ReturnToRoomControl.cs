using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class ReturnToRoomControl : MonoBehaviour
    {

        public GameObject ExitConfirmationPanel;
        public Button YesButton;
        public Button CancelButton;

        // Start is called before the first frame update
        void Start()
        {
            YesButton.onClick.AddListener(exit);
            CancelButton.onClick.AddListener(cancel);
        }

        // Update is called once per frame
        void Update()
        {
            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.DPadCenter))
            {
                ExitConfirmationPanel.SetActive(true); 
            }
        }

        private void exit()
        {
            StartCoroutine(LoadSceneAsync("SingleScene"));
        }

        private static IEnumerator LoadSceneAsync(string sceneName)
        { 
            //load next scene
            var asyncOp = SceneManager.LoadSceneAsync(sceneName);

            //go back to game until scene is done loading
            while(!asyncOp.isDone)
            {
                yield return null;
            }
        }

        private void cancel()
        {
            ExitConfirmationPanel.SetActive(false);
        }
    }
}
