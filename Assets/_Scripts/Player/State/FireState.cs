using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class FireState : AbilityState
{
    private FireAbility firePrimary;
    private FireAbility fireSecondary;
    private int buildUpKillNumber;
    private float primaryCoolDown;
    private float primaryTimer;
    private float secondaryCoolDown;
    private float secondaryTimer;
    private Transform fire;
    private Collider2D playerCollider;
    private bool canShootPrimary;
    private bool canShootSecondary;
    public override void Enter()
    {
        this.firePrimary = abilities.primary as FireAbility;
        this.fireSecondary = abilities.secondary as FireAbility;
        this.primaryCoolDown = firePrimary.Cooldown.x;
        this.primaryTimer = 0;
        this.secondaryCoolDown = fireSecondary.Cooldown.x;
        this.secondaryTimer = 0;
        canShootPrimary = true;
        canShootSecondary = false;
        buildUpKillNumber = 10;
        fire = _context.player.Find("Fire");
        playerCollider = _context.player.GetComponent<Collider2D>();
        _context.gamePlayManager.changeAbility += ChangeAbility;
    }

    private void ChangeAbility(AbilitySet abilities, AbilityState newState)
    {
        Debug.Log($"Change Ability to {abilities.primary.Title}");
    }

    public override void Exit()
    {
        buildUpKillNumber = 0;
        primaryTimer = 0;
        secondaryTimer = 0;
        _context.gamePlayManager.changeAbility -= ChangeAbility;
    }

    public override void Handle()
    {
        primaryTimer += Time.deltaTime;
        if(primaryTimer >= primaryCoolDown)
        {
            canShootPrimary = true;
        }
        if (canShootPrimary && Input.GetKeyDown(KeyCode.Mouse0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - _context.player.position);
            var lookDirection = mousePos;
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            GameObject.Instantiate(firePrimary.Projectile.ProjectilePrefab, _context.player.position, Quaternion.identity).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(firePrimary.Projectile, fire.right);
            canShootPrimary = false;
            primaryTimer = 0;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (buildUpKillNumber >= Constants.SecondaryThreshold)
            {
                EnableSecondary();
            }
        }
    }

    private void EnableSecondary()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePos - _context.player.position);
        var lookDirection = mousePos;
        var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        fire.rotation = Quaternion.Euler(0, 0, lookAngle);
        direction.Normalize();
        GameObject.Instantiate(fireSecondary.Projectile.ProjectilePrefab, _context.player.position, Quaternion.identity).TryGetComponent<FlameThrower>(out var flameThrower);
        var flames = flameThrower.GetComponent<ParticleSystem>();
        var colliderNumber = flames.trigger.colliderCount;
        for(int i = 0; i< colliderNumber; ++i)
        {
            Physics2D.IgnoreCollision(playerCollider, flames.trigger.GetCollider(i).GetComponent<Collider2D>());
        }
       
        //projectile.Instantiate(currentAbilities.primary.Projectile, fire.right);
    }
}
