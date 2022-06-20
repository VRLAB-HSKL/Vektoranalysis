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
            // Generate default config
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            //var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            //var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            //var unityLogFile = new NLog.Targets.

            NLog.LogManager.Setup().SetupExtensions(s =>
                s.RegisterTarget<Logging.UnityConsoleTarget>("UnityConsole")
            );
            
            
            //var logconsole = new NLog.Targets.ConsoleTarget();
            
            // Rules for mapping loggers to targets            
            //config.AddRule(Info, Fatal, logconsole);
            //config.AddRule(LogLevel.Debug, Fatal, logfile);
            
            // Apply config           
            LogManager.Configuration = config;
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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        protected override void Write(LogEventInfo logEvent)
        {
            var msg = logEvent.Message;
            var level = logEvent.Level;

            if (level == LogLevel.Debug)
            {
                Debug.Log(msg);    
            }
                
        }
    }
}