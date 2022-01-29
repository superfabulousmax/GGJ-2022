using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityState
{
    public AbilitySet abilities;
    protected PlayerContext _context;
    public void SetAbilities(AbilitySet abilities)
    {
        this.abilities = abilities;
    }

    public void SetContext(PlayerContext context)
    {
        this._context = context;
    }

    public abstract void Enter();
    public abstract void Handle();

    public abstract void Exit();
}
