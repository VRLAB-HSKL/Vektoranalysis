using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKL.MBU;

public abstract class StateContext
{
    protected StateContext(State s)
    {
        
    }
}


public class CurveSelectionStateContext : StateContext
{
    private AbstractCurveSelectionState _state;
    public new AbstractCurveSelectionState State
    {
        get { return _state; }
        set
        {
            if(_state != null)
                _state.OnStateQuit();
            
            value.OnStateEntered();
            _state = value;
        }
    }

    public CurveSelectionStateContext(State s) : base(s) {}
}
