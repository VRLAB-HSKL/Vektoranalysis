﻿using UnityEngine;
using VRKL.MBU;

/// <summary>
/// View/Observer für eine eine analoge Uhr
/// mit Stunden- und Minutenzeiger
/// </summary>
public class AnalogSimple : Observer
{
    /// <summary>
    /// Das beobachtete Objekt
    /// </summary>
    private Clock Model;

    /// <summary>
    /// Die Rotationsmatrix für den Stundenzeiger
    /// </summary>
    public Transform hourHand;
    /// <summary>
    /// Die Rotationsmatrix für den Minutenzeiger
    /// </summary>
    public Transform minuteHand;

    /// <summary>
    /// In Awake stellen die Verbindung zur Subject-Klasse her.
    /// </summary>
    private void Awake()
    {
        Model =  Clock.Instance;
        Model.Attach(this);
    }

    /// <summary>
    /// Die Winkel für die Zeiger berechnen, anwenden
    /// und damit die grafische Ausgabe durchführen.
    /// 
    /// Der Winkel für den Stundenzeiger erwartet die Stunden
    /// im 12-Stundenformat und berechnet sich als -360*(minuten/12).
    /// 
    /// Der Winkel für den Minutenzeiger berechnet sich als
    /// -360*(minuten/60).
    /// </summary>
    public override void Refresh()
    {
        // Winkel für den Minutenzeiger
        float angleMinutes = - 360.0f * ((float)Model.Minute/60.0f);
        // Winkel für den Stundenzeiger
        float angleHours = - 360.0f * ((float)(Model.Hour % 12)/12.0f);
        // Jetzt führen wir die Rotation durch
        minuteHand.localRotation = Quaternion.Euler(0.0f, 0.0f, angleMinutes);
        hourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, angleHours);
    }
}
