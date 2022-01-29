using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerContext _context;

    public void SetContext(PlayerContext context)
    {
        this._context = context;
    }

    public abstract void Enter();
    public abstract void Handle();

    public abstract void Exit();
}
