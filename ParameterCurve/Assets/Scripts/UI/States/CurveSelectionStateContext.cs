using VRKL.MBU;

namespace UI.States
{
    /// <summary>
    /// Abstract state context
    /// </summary>
    public abstract class StateContext
    {
        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="s">Initial state</param>
        protected StateContext(State s)
        {
            //Debug.Log(s.ToString());
        }
    }

    /// <summary>
    /// State context related to the in-game curve selection menu
    /// </summary>
    public class CurveSelectionStateContext : StateContext
    {
        /// <summary>
        /// Private helper variable for <see cref="State"/>
        /// </summary>
        private AbstractCurveSelectionState _state;
        
        /// <summary>
        /// Current state of the FSM
        /// </summary>
        public AbstractCurveSelectionState State
        {
            get => _state;
            set
            {
                _state?.OnStateExit();
                value.OnStateEntered();
                _state = value;
            }
        }

        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="s">Initial state</param>
        public CurveSelectionStateContext(State s) : base(s) {}
    }
}