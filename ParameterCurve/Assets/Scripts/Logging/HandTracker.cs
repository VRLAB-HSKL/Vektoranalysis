using System.Collections;
using System.Collections.Generic;
using System.Text;
using HTC.UnityPlugin.Vive;
using log4net;
using UnityEngine;

public class HandTracker : MonoBehaviour
{
    public enum HandEnum { RIGHT = 0, LEFT = 1};

    public HandEnum Hand; 


    private static readonly ILog Log = LogManager.GetLogger(typeof(HandTracker));

    private HandRole _role;
    private string _prefix;

    private StringBuilder _stringBuilder;


    private void Start()
    {
        _stringBuilder = new StringBuilder();

        switch(Hand)
        {
            case HandEnum.LEFT:
                _prefix = "Left";
                _role = HandRole.LeftHand;
                break;

            case HandEnum.RIGHT:
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

        Log.Info(_stringBuilder.ToString());
        _stringBuilder.Clear();
    }
}
