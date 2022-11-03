// using log4net.Appender;
// using log4net.Core;
// using UnityEngine;
//
// /// <summary> 
// /// Log4Net-Appender f�r die Unity Console.
// /// </summary>
// /// <remarks>
// ///  Alle Logging-Level werden mit Debug.Log ausgegeben,
// /// ohne Ver�nderung.
// ///
// /// Granularer kann man dies mit der Klasse
// /// <code>UnityConsoleAppender</code>
// /// durchf�hren.
// /// </remarks>
// public class UnityDebugAppender : AppenderSkeleton
// {
//     /// <summary>
//     /// �berschreiben der Funktion Append
//     /// </summary>
//     /// <param name="loggingEvent">
//     /// Logging-Event mit den Inhalten,
//     /// die wir ausgeben.
//     /// </param>
//   protected override void Append(LoggingEvent loggingEvent)
//   {
//     var message = RenderLoggingEvent(loggingEvent);
//     Debug.Log(message);
//   }
// }
