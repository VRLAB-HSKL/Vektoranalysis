using System.Text;
using HTC.UnityPlugin.Vive;
using UnityEngine;
//using log4net;

namespace ParamCurve.Scripts.Logging
{
    public class HandTracker : MonoBehaviour
    {
        public enum HandEnum { Right = 0, Left = 1};

        public HandEnum hand; 

        private HandRole _role;
        private string _prefix;

        private StringBuilder _stringBuilder;
        
        //private static readonly ILog Log = LogManager.GetLogger(typeof(HandTracker));

        private void Start()
        {
            _stringBuilder = new StringBuilder();

            switch(hand)
            {
                case HandEnum.Left:
                    _prefix = "Left";
                    _role = HandRole.LeftHand;
                    break;

                case HandEnum.Right:
                    _prefix = "Right";
                    _role = HandRole.RightHand;
                    break;
            }
        }

        void Update()
        {
            var handPose = VivePose.GetPoseEx(_role);
            _stringBuilder.AppendLine("Frame: " + Time.frameCount);
            _stringBuilder.AppendLine(_prefix + " hand position: " + handPose.pos);
            _stringBuilder.AppendLine(_prefix + " hand rotation: " + handPose.rot);
            _stringBuilder.AppendLine(_prefix + " hand up: " + handPose.up);
            _stringBuilder.AppendLine(_prefix + " hand forward: " + handPose.forward);
            _stringBuilder.AppendLine(_prefix + " hand right: " + handPose.right);

            //Log.Info(_stringBuilder.ToString());
            _stringBuilder.Clear();
        }
    }
}
