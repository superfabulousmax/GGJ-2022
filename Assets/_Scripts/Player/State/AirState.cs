using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AirState : AbilityState
{
    private AirAbility airPrimary;
    private AirAbility airSecondary;
    private int buildUpKillNumber;
    private const int maxHitCount = 10;
    private Transform fire;
    private Collider2D playerCollider;
    public override void Enter()
    {
        base.Enter();
        this.airPrimary = abilities.primary as AirAbility;
        this.airSecondary = abilities.secondary as AirAbility;
        this.primaryCoolDown = airPrimary.Cooldown.x;
        this.primaryTimer = 0;
        this.secondaryCoolDown = airSecondary.Cooldown.x;
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
        base.Exit();
        buildUpKillNumber = 0;
        primaryTimer = 0;
        secondaryTimer = 0;
        _context.gamePlayManager.changeAbility -= ChangeAbility;
    }

    public override void Handle()
    {
        base.Handle();

        if (canShootPrimary && Input.GetKey(KeyCode.Mouse0))
        {
            MakeShootSound();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)_context.player.position);
            var lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            GameObject.Instantiate(airPrimary.Projectile.ProjectilePrefab, _context.player.position, fire.rotation).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(airPrimary.Projectile, direction, _context.player, fire);
            projectile.onPrimaryHitEnemy += onPrimaryHit;

            canShootPrimary = false;
            primaryTimer = 0;
        }
    }

    private void MakeShootSound()
    {
        if (airPrimary.Sound != null)
        {
            _context.audioSource.PlayOneShot(airPrimary.Sound);
        }
    }

    private void onPrimaryHit(Projectile projectile, EnemySeekController enemySeekController)
    {
        projectile.hitCount++;
        projectile.KeepVelocity();
        Physics2D.IgnoreCollision(projectile.GetCollider, enemySeekController.GetCollider);
        enemySeekController.TakeDamage(Elements.Air, 1);

        if (projectile.hitCount > maxHitCount)
        {
            GameObject.Destroy(projectile.gameObject, 0.5f);
            projectile.GetRigidbody.velocity = Vector2.zero;
            projectile.GetCollider.enabled = false;

            var sprite = projectile.transform.Find("Sprite");
            if (sprite != null)
            {
                sprite.gameObject.SetActive(false);
            }

            projectile.onPrimaryHitEnemy -= onPrimaryHit;
        }
    }
}
