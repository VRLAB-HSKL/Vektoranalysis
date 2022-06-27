using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Controller;

namespace Behaviour.Button
{
    public class ReturnToRoomButtonBehavior : AbstractButtonBehaviour
    {
        public CockpitWorldStateController world;

        public GameObject ExitConfirmationPanel;
        public UnityEngine.UI.Button YesButton;
        public UnityEngine.UI.Button CancelButton;

        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();
            YesButton.onClick.AddListener(exit);
            CancelButton.onClick.AddListener(cancel);
        }

        protected override void HandleButtonEvent()
        {
            ExitConfirmationPanel.SetActive(true);
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
