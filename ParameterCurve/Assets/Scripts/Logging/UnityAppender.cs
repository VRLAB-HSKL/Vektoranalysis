using System.Collections;
using System.Collections.Generic;
using log4net.Appender;
using log4net.Core;
using UnityEngine;

/// <summary>
/// Custom log4net appender to show logging messages in unity output console
/// </summary>
public class UnityAppender : AppenderSkeleton
{
    public bool ShowLogging = false;
    
    protected override void Append(LoggingEvent loggingEvent)
    {
        if (!ShowLogging) return;
        Debug.Log(RenderLoggingEvent(loggingEvent));
    }
}
