using System.Runtime.InteropServices;
using System.Text;
using HTC.UnityPlugin.Vive;
using NLog;
using UnityEditor.Build.Content;
using UnityEngine;

namespace Logging
{
    public class NLogTest : MonoBehaviour
    {
        public bool activatePoseLogging;
        
        // The logger object should be a static variable in every class that it is needed in
        // ToDo: If using NLog framework, remove static global model variable in ParamCurve application
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static StringBuilder _sb = new StringBuilder();
        
        private void Start()
        {
            // Create custom target and get config instance
            var unityConsoleTarget = new UnityConsoleTarget();
            var cfg = new NLog.Config.LoggingConfiguration();
            
            // Add target
            cfg.AddTarget(nameof(UnityConsoleTarget), unityConsoleTarget);
            
            // Add logging rule 
            cfg.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Trace, unityConsoleTarget));
            
            // Assign configuration
            LogManager.Configuration = cfg;
        }


        private void Update()
        {
            if (activatePoseLogging)
            {
                StaticLogging();
            }
        }
        
        
        /// <summary>
        /// Static logging operations that are performed every frame, regardless of user interaction
        /// ToDo: Duplicate of function in ParamCurve, in the future move to MBVR package
        /// </summary>
        private void StaticLogging()
        {
            _sb.AppendLine();
            
            // Head pose
            var headPose = VivePose.GetPoseEx(BodyRole.Head);
            _sb.AppendLine("Head position: " + headPose.pos);
            _sb.AppendLine("Head rotation: " + headPose.rot);
            _sb.AppendLine("Head up: " + headPose.up);
            _sb.AppendLine("Head forward: " + headPose.forward);
            _sb.AppendLine("Head right: " + headPose.right);

            // Left hand pose
            var leftPose = VivePose.GetPoseEx(HandRole.LeftHand);
            _sb.AppendLine("Left hand position: " + leftPose.pos);
            _sb.AppendLine("Left hand rotation: " + leftPose.rot);
            _sb.AppendLine("Left hand up: " + leftPose.up);
            _sb.AppendLine("Left hand forward: " + leftPose.forward);
            _sb.AppendLine("Left hand right: " + leftPose.right);
        
            // Right hand pose
            var rightPose = VivePose.GetPoseEx(HandRole.RightHand);
            _sb.AppendLine("Right hand position: " + rightPose.pos);
            _sb.AppendLine("Right hand rotation: " + rightPose.rot);
            _sb.AppendLine("Right hand up: " + rightPose.up);
            _sb.AppendLine("Right hand forward: " + rightPose.forward);
            _sb.AppendLine("Right hand right: " + rightPose.right);
            
            
            Logger.Info(_sb);
            _sb.Clear();
        }
    }

    public class UnityConsoleTarget : NLog.Targets.TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            Debug.Log(logEvent.FormattedMessage);
        }
    }
}