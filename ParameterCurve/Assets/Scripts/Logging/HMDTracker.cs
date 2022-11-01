//using log4net;
using HTC.UnityPlugin.Vive;
using UnityEngine;
using System.Text;

public class HMDTracker : MonoBehaviour
{
    //private static readonly ILog Log = LogManager.GetLogger(typeof(HMDTracker));

    private StringBuilder _stringBuilder;

    private void Start()
    {
        _stringBuilder = new StringBuilder();
    }

    // Update is called once per frame
    void Update()
    {
        // Head pose
        var headPose = VivePose.GetPoseEx(BodyRole.Head);
        _stringBuilder.AppendLine("Frame: " + Time.frameCount);
        _stringBuilder.AppendLine("Head position: " + headPose.pos);
        _stringBuilder.AppendLine("Head rotation: " + headPose.rot);
        _stringBuilder.AppendLine("Head up: " + headPose.up);
        _stringBuilder.AppendLine("Head forward: " + headPose.forward);
        _stringBuilder.AppendLine("Head right: " + headPose.right);

        // Head gaze
        var hitPoint = Vector3.positiveInfinity;
        if(Physics.Raycast(new Ray(headPose.pos, headPose.forward), out RaycastHit hit, float.MaxValue))
        {
            hitPoint = hit.point;
        }

        _stringBuilder.AppendLine("Gaze point: " + hitPoint);

        // ToDo: Log eye gaze if hardware supports it


        //Log.Info(_stringBuilder.ToString());
        _stringBuilder.Clear();
    }
}
