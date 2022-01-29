using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : AbilityState
{
    private FireAbility firePrimary;
    private FireAbility fireSecondary;
    public override void Enter()
    {
        this.firePrimary = abilities.primary as FireAbility;
        this.fireSecondary = abilities.secondary as FireAbility;
    }

    public override void Exit()
    {
    }

    public override void Handle()
    {
    }
}
