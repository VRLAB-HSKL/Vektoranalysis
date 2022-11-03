using UnityEngine.UI;
using VRKL.MBU;

/// <summary>
/// Eine Anzeige einer Digitaluhr mit Stunden- und Minutenangaben.
/// 
/// Diese Klasse verwendet die Basisklasse Observer im MBU-Package!
/// </summary>
public class DigitalSimple : Observer
{
    /// <summary>
    /// Das beobachtete Objekt
    /// </summary>
    private Clock Model;

    /// <summary>
    /// Text-Feld f�r die Ausgabe der Digitaluhr
    /// </summary>
    private Text m_txt;

    /// <summary>
    /// <summary>
    /// In Awake stellen die Verbindung zur Subject-Klasse her.
    /// </summary>
    /// </summary>
    private void Awake()
    {
        Model = Clock.Instance;
        Model.Attach(this);
    }

    /// <summary>
    /// Wir verbinden die Variable txt mit einer Text-Component des GameObjects.
    /// </summary>
    private void Start()
    {
        m_txt = gameObject.GetComponent<Text>();
    }

    /// <summary>
    /// wir bauen den Text den wir ausgeben zusammen.
    /// </summary>
    /// <returns></returns>
    public override void Refresh()
    {
        string timeOutput;

        timeOutput = Model.Hour + " : " + Model.Minute;
        m_txt.text = timeOutput;
    }
}


