using System.Collections;
using ParamCurve.Scripts.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParamCurve.Scripts.Behaviour.Button
{
    /// <summary>
    /// Button that will return to main display room from cockpit view
    /// </summary>
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
            YesButton.onClick.AddListener(Exit);
            CancelButton.onClick.AddListener(Cancel);
        }

        protected override void HandleButtonEvent()
        {
            ExitConfirmationPanel.SetActive(true);
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

        private void Exit()
        {
            StartCoroutine(LoadSceneAsync("SingleScene"));
        }
        
        private void Cancel()
        {
            ExitConfirmationPanel.SetActive(false);
        }
    }
}
