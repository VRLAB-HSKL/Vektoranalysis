﻿/// <summary>
/// State für "Ampel ist rot"
/// </summary>
sealed class StateStop : TrafficState
{
    /// <summary>
    /// Instanz-Variable für das Abfragen. 
    /// <remarks>
    /// Wir verwenden die Instanz mit
    /// <code>StateGo s = StateGo.Instance;</code>.
    /// </remarks>
    /// </summary>
    public static readonly StateStop Instance = new StateStop();

    /// <summary>
    /// Private Konstruktor für das Singleton-Pattern.
    /// <remarks>
    /// Quelle für die Implementierung des Patterns:
    /// https://wiki.byte-welt.net/wiki/Singleton_Beispiele_(Design_Pattern)#Eager_Creation
    /// </remarks>
    /// </summary>
    private StateStop()
    {
        _state = TrafficLightStates.Stop;
    }
    
    /// <summary>
    /// Wird während des Eintretens in einen State aufgerufen
    /// </summary>
    public override void OnStateEntered()
    {    }

    /// <summary>
    /// Wird aufgerufen während der State aktiv ist
    /// </summary>
    public override void OnStateUpdate()
    {    }

    /// <summary>
    /// Wird während des Verlassens eines States aufgerufen
    /// </summary>
    public override void OnStateQuit()
    {    }

    /// <summary>
    /// Wir wechseln von "Rot" auf "Rot und Gelb"
    /// </summary>
    public override TrafficState ChangeState()
    {
        OnStateQuit();
        StateWait.Instance.OnStateEntered();
        return StateWait.Instance;
    }
}
