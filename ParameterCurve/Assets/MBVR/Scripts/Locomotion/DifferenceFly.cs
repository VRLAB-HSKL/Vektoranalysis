//========= 2021 - 2022 - Copyright Manfred Brill. All rights reserved. ===========

namespace VRKL.MBVR
{
    /// <summary>
    /// Fly als Locomotion in einer VR-Anwendung, mit zwei Objekten f�r
    /// die Definition der Bewegungsrichtung.
    /// </summary>
    /// <remarks>
    /// Fly bedeutet, dass wir die Bewegungsrichtung in allen drei
    /// Koordinatenachsen ver�ndern k�nnen.
    ///
    /// Wir verwenden einen Trigger-Button. So lange dieser Button
    /// gedr�ckt ist wird die Bewegung ausgef�hrt.
    /// 
    /// Als Bewegungsrichtung verwenden wir den Differenzvektor
    /// zweier Objekte, typischer Weise die Controller. M�glich ist
    /// auch den Kopf als einer der Objekte zu verwenden.
    ///
    /// Die Geschwindigkeit wird mit Buttons auf einem Controller
    /// ver�ndert.
    /// </remarks>
    public class DifferenceFly : TwoObjectsDirection
    {
        /// <summary>
        /// Bewegungsrichtung als Differenz der forward-Vektoren
        /// der beiden definierenden Objekte setzen.
        /// </summary>
        /// <remarks>
        /// Implementierung stimmt aktuell mit InitializeDirection �berein.
        /// </remarks>
        protected override void UpdateDirection()
        {
            Direction = endObject.transform.position - startObject.transform.position;
            Direction.Normalize();
        }

        protected override void UpdateOrientation()
        {
            throw new System.NotImplementedException();
        }
    }
}
