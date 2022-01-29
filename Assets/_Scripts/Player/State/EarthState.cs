using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthState : AbilityState
{
    private EarthAbility earthPrimary;
    private EarthAbility earthSecondary;
    private int buildUpKillNumber;
    private float secondaryCoolDown;
    private float secondaryTimer;
    private Transform fire;
    private Collider2D playerCollider;
    public override void Enter()
    {
        this.earthPrimary = abilities.primary as EarthAbility;
        this.earthSecondary = abilities.secondary as EarthAbility;
        this.secondaryCoolDown = earthSecondary.Cooldown.x;
        this.secondaryTimer = 0;
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
        secondaryTimer = 0;
        _context.gamePlayManager.changeAbility -= ChangeAbility;
    }

    public override void Handle()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - _context.player.position);
            var lookDirection = mousePos;
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            GameObject.Instantiate(earthPrimary.Projectile.ProjectilePrefab, _context.player.position, Quaternion.identity).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(earthPrimary.Projectile, fire.right);
        }
    }
}
