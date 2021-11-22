using UnityEngine;
using VRKL.MBU;

namespace UI.States
{
    /// <summary>
    /// Abstract state context
    /// </summary>
    public abstract class StateContext
    {
        protected StateContext(State s)
        {
            Debug.Log(s.ToString());
        }
    }

    /// <summary>
    /// State context related to the ingame curve selection menu
    /// </summary>
    public class CurveSelectionStateContext : StateContext
    {
        private AbstractCurveSelectionState _state;
        public AbstractCurveSelectionState State
        {
            get => _state;
            set
            {
                _state?.OnStateQuit();

                value.OnStateEntered();
                _state = value;
            }
        }

        public CurveSelectionStateContext(State s) : base(s) {}
    }
}