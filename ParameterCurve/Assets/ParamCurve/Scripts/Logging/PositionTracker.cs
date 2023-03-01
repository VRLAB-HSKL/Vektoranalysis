//using log4net;

using UnityEngine;

namespace ParamCurve.Scripts.Logging
{
    public class PositionTracker : MonoBehaviour
    {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(PositionTracker));

        private Transform playerTransform;

        private void Start()
        {
            var cam = FindObjectOfType<Camera>();
            playerTransform = cam.transform;
        }

        private void Update()
        {
            // Log.Info(
            //     "Frame: " + Time.frameCount + "\n" +
            //     "Player position: " + playerTransform.position
            // );
        }
    }
}
