using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityState
{
    public AbilitySet abilities;
    protected PlayerContext _context;
    protected AudioClip _clip;
    protected AudioSource _source;
    protected float primaryCoolDown = 0;
    protected float primaryTimer = 0;
    protected float secondaryCoolDown = 0;
    protected float secondaryTimer = 0;
    protected float secondaryDuration = 0;
    protected bool canShootPrimary;
    protected bool canShootSecondary;
    protected bool secondaryActive;

    public void SetAbilities(AbilitySet abilities)
    {
        this.abilities = abilities;
    }

    public void SetAudio(AudioSource source, AudioClip clip)
    {
        _source = source;
        _clip = clip;
    }

    public void SetContext(PlayerContext context)
    {
        this._context = context;
    }

    public virtual void Enter()
    {
        canShootPrimary = false;
        canShootSecondary = false;
        secondaryActive = false;
    }

    public virtual void Handle()
    {
        if (GamePlayManager.Instance.IsGameOver)
            return;

    }

    public virtual void HandleCoolDown()
    {
        if (secondaryActive)
        {
            secondaryTimer += Time.deltaTime;
            if (secondaryTimer >= secondaryDuration)
            {
                DisableSecondary();
            }
        }
        else
        {
            primaryTimer += Time.deltaTime;
            if (primaryTimer >= primaryCoolDown)
            {
                canShootPrimary = true;
            }
        }
    }

    public virtual void DisableSecondary()
    {

    }
    public virtual void Exit()
    {
        canShootPrimary = false;
        canShootSecondary = false;
        secondaryActive = false;
    }
}
