using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext
{
    // A reference to the current state of the Context.
    private AbilityState _state = null;

    public PlayerContext(AbilityState state)
    {
        this.TransitionTo(state);
    }

    public void TransitionTo(AbilityState state)
    {
        this._state = state;
        this._state.SetContext(this);
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
