using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterState : AbilityState
{
    private WaterAbility waterPrimary;
    private WaterAbility waterSecondary;
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
        this.waterPrimary = abilities.primary as WaterAbility;
        this.waterSecondary = abilities.secondary as WaterAbility;
        this.primaryCoolDown = waterPrimary.Cooldown.x;
        this.primaryTimer = 0;
        this.secondaryCoolDown = waterSecondary.Cooldown.x;
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
        if (primaryTimer >= primaryCoolDown)
        {
            canShootPrimary = true;
        }
        if (canShootPrimary && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - (Vector2)_context.player.position);
            var lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            GameObject.Instantiate(waterPrimary.Projectile.ProjectilePrefab, _context.player.position, fire.rotation).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(waterPrimary.Projectile, direction, _context.player, fire);
            canShootPrimary = false;
            primaryTimer = 0;
        }
    }
}

