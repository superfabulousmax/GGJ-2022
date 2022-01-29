using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext
{
    // A reference to the current state of the Context.
    private State _state = null;

    public PlayerContext(State state)
    {
        this.TransitionTo(state);
    }

    public void TransitionTo(State state)
    {
        this._state = state;
        this._state.SetContext(this);
    }

    // The Context delegates part of its behavior to the current State
    // object.
    public void Enter()
    {
        this._state.Enter();
    }

    public void Handle()
    {
        this._state.Handle();
    }

    public void Exit()
    {
        this._state.Exit();
    }
}
