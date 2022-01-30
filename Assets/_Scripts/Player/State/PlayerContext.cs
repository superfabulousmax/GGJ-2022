using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext
{
    // A reference to the current state of the Context.
    private AbilityState _state = null;
    public Transform player;
    public GamePlayManager gamePlayManager;
    public AudioSource audioSource;
    public PlayerContext(AbilityState state, Transform player, GamePlayManager gamePlayManager, AudioSource audioSource)
    {
        this.audioSource = audioSource;
        this.player = player;
        this.gamePlayManager = gamePlayManager;
        this.TransitionTo(state);
    }

    public void TransitionTo(AbilityState state)
    {
        _state?.Exit();
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
