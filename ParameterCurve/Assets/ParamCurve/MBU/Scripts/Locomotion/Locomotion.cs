//========= 2020 - 2022 - Copyright Manfred Brill. All rights reserved. ===========
using UnityEngine;

namespace VRKL.MBU
{
    /// <summary>
    /// Abstrakte Basisklasse f�r die Fortbewegung auf dem Desktop
    /// und in VR.
    /// </summary>
    /// <remarks>
    /// Davon abgeleitet gibt es die ebenfalls virtuellen Klassen
    /// MBU.Locomotion  und MBVR.ImmersiveLocomotion.
    /// </remarks>
    public abstract class Locomotion : MonoBehaviour
    {
        /// <summary>
        /// Festlegen der Bewegungsrichtung.
        /// </summary>
        /// <remarks>
        /// Bewegungsrichtung als normalisierte Vector3-Instanz.
        /// Wenn diese Funktion nicht �berschrieben wird verwenden
        /// wir forward des GameObjects, an dem die Komponente
        /// h�ngt.
        /// </remarks>
        protected virtual void InitializeDirection()
        {
            Direction = transform.forward;
        }

        /// <summary>
        /// Orientierung initialiseren. Wir �berschreiben diese
        /// Funktion in den abgeleiteten Klassen und rufen
        /// diese Funktion in Locomotion::Awake  auf.
        /// </summary>
        protected virtual void InitializeOrientation()
        {
            Orientation = new Vector3(0.0f, 0.0f, 0.0f);
        }
        
        /// <summary>
        /// Update  der Bewegungsrichtung.
        /// </summary>
        protected virtual void UpdateDirection()
        {
            Direction = transform.forward;
        }
        
        /// <summary>
        /// Berechnung der Geschwindigkeit der Fortbewegung
        /// </summary>
        protected abstract void UpdateSpeed();

        /// <summary>
        /// Orientierung f�r die Bewegung als Eulerwinkel.
        /// </summary>
        /// <remarks>
        /// Orientierungen als Instanz von Vector3.
        /// </remarks>
        protected abstract void UpdateOrientation();

        /// <summary>
        /// Geschwindigkeit initialiseren. Wir �berschreiben diese
        /// Funktion in den abgeleiteten Klassen und rufen
        /// diese Funktionin Locomotion::Awake auf.
        /// </summary>
        protected abstract void InitializeSpeed();

        /// <summary>
        /// Initialisieren
        /// </summary>
        protected virtual void Awake()
        {
            // Bewegungsrichtung, Orientierung und Bahngeschwindigkeit initialisieren
            InitializeDirection();
            InitializeOrientation();
            InitializeSpeed();
        }

        /// <summary>
        /// Die Bewegung durchf�hren.
        ///
        /// Wir bewegen uns in Richtung des Vektors Direction,
        /// er typischer Weise auf forward des GameObjects gesetzt wird.
        ///
        /// Wir orientieren das Objekt mit Hilfe der Eulerwinkel in Orientation
        /// und f�hren anschlie�end eine Translation in Richtung Direction durch.
        /// </summary>
        protected virtual void Move()
        {
            transform.eulerAngles = Orientation;
            transform.Translate(Speed * Time.deltaTime * Direction);
        }

        /// <summary>
        /// Betrag der Geschwindigkeit f�r die Bewegung
        /// <remarks>
        /// Einheit dieser Variable ist m/s.
        /// </remarks>
        /// </summary>
        protected float Speed;

        /// <summary>
        /// Vektor mit den Eulerwinkeln f�r die Kamera
        /// </summary>
        protected Vector3 Orientation;

        /// <summary>
        /// Klasse f�r die Verwaltung der Bahngeschwindigkeit.
        /// </summary>
        protected ScalarProvider Velocity;

        /// <summary>
        /// Normierter Richtungsvektor f�r die Fortbewegung.
        /// </summary>
        /// <remarks>
        /// In den VR-Varianten wird die Richtung direkt
        /// aus dem forward-Vektor des Orientierungsobjekts
        /// gesetzt.
        /// </remarks>
        protected Vector3 Direction;
    }
}
