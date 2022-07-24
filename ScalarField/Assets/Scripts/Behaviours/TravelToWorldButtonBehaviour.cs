using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using VR.Scripts.Behaviours.Button;

namespace Behaviours
{
    /// <summary>
    /// Button behaviour to travel to the detail scene after choosing a point in the scalar field to travel to
    /// </summary>
    public class TravelToWorldButtonBehaviour : AbstractButtonBehaviour
    {
        /// <summary>
        /// Starts process of traveling to the next scene, if a target point on the map was chosen
        /// </summary>
        protected override void HandleButtonEvent()
        {
            // Attempt to get created travel point on map 
            var travelObj = GameObject.Find("TravelTarget");

            // Return if no location was chosen yet
            if (travelObj is null)
            {
                Debug.Log("No location chosen yet!");
                return;
            }
        
            // Start loading of the next scene
            StartCoroutine(LoadSceneAsync("Detail"));
        }

        /// <summary>
        /// Coroutine to load the next scene in the background
        /// </summary>
        /// <param name="sceneName">Name of the scene asset</param>
        /// <returns>Enumerator for the coroutine</returns>
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            //DontDestroyOnLoad(this);
            
            // Load next scene asynchronously
            var asyncOp = SceneManager.LoadSceneAsync(sceneName);

            // Give control back to the unity game loop until the next scene is loaded and can be traveled to
            while (!asyncOp.isDone)
            {
                yield return null;
            }

        }

    }
}
