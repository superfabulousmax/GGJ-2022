using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Fire", fileName ="New Fire Ability")]
public class FireAbility : Ability
{
    GameObject gameObject;
    public override void Initialise(GameObject gameObject) 
    {
        this.gameObject = gameObject;
    }

    public override void TriggerAbility() { }
    public override void EndAbility() {}
}