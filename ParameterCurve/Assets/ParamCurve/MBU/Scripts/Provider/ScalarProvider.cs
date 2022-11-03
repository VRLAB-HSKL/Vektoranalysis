//========= 2020 - 2022 - Copyright Manfred Brill. All rights reserved. ===========
using UnityEngine;

namespace VRKL.MBU
{
    /// <summary>
    /// Klasse, die einen float-Wert verwaltet.
    /// Es gibt Funktionen f�r die Ver�nderung des Werts, und es wird
    /// ein zul�ssiges Intervall definiert, das mit Hilfe von Clamp eingehalten wird.
    ///
    /// Man k�nnte ein Subject daraus machen, darauf wurde erstmal verzichtet.
    /// </summary>
    public class ScalarProvider
    {
        /// <summary>
        /// Set und Get f�r den skalaren Wert
        /// </summary>
        public float value
        {
            get => _value;
            set => _value = this.value;
        }
        
        /// <summary>
        /// Set und Get f�r das Delta zum
        /// Ver�ndern des Werts
        /// </summary>
        public float delta
        {
            get => _delta;
            set => _delta = this.delta;
        }

        /// <summary>
        /// Set und Get f�r das Minimum des Werts
        /// </summary>
        public float minimum
        {
            get => _min;
            set => _min = this.minimum;
        }
        
        /// <summary>
        /// Set und Get f�r das Maximum des Werts
        /// </summary>
        public float maximum
        {
            get => _max;
            set => _max = this.maximum;
        }
        
        /// <summary>
        /// Den Wert um ein Delta erh�hen
        /// </summary>
        public void Increase()
        {
            _value = Mathf.Clamp(_value + _delta, _min, _max);
        }
        
        /// <summary>
        /// Den Wert um ein Delta erniedrigen
        /// </summary>
        public void Decrease()
        {
            _value = Mathf.Clamp(_value - _delta, _min, _max);
        }

        /// <summary>
       /// Wert und Delta setzen.
       /// <remarks>
       /// Minimum wird auf 0 gesetzt, Maximum auf 10.0.
       /// </remarks>
       /// <param name="theValue">Anfangswert</param>
       /// <param name="theDelta">Wert f�r die Ver�nderung </param>
       /// </summary>
       public ScalarProvider(float theValue, float theDelta)
       {
           _value = theValue;
           _delta = theDelta;
           _min = 0.0f;
           _max = 10.0f;
       }
  
        /// <summary>
        /// Wert,  Delta, Minimum und Maximum setzen.
        /// <param name="theValue">Anfangswert</param>
        /// <param name="theDelta">Wert f�r die Ver�nderung </param>
        /// <param name="theMin">Minimaler Wert</param>
        /// <param name="theDelta">Maximaler Wert </param>
        /// </summary>
        public ScalarProvider(float theValue, float theDelta,
                                       float theMin, float theMax)
        {
            _value = theValue;
            _delta = theDelta;
            _min = theMin;
            _max = theMax;
        }

        /// <summary>
       /// Der skalare Float-Wert, den diese Klasse liefert.
       /// </summary>
       private float _value;
       
       /// <summary>
       /// Der skalare Float-Wert, den wir f�r die Ver�nderung
       /// des skalaren Werts einsetzen.
       /// </summary>
       private float _delta;

       /// <summary>
       /// Minimaler Wert, der angenommen werden kann.
       /// In Increase und Decrease wird ein Clamp durchgef�hrt.
       /// Damit ist garantiert, dass der Wert immer im zul�ssigen
       /// Intervall liegt.
       /// </summary>
       private float _min;
  
       /// <summary>
       /// Maximaler Wert, der angenommen werden kann.
       /// In Increase und Decrease wird ein Clamp durchgef�hrt.
       /// Damit ist garantiert, dass der Wert immer im zul�ssigen
       /// Intervall liegt.
       /// </summary>
       private float _max;       
    }
}
