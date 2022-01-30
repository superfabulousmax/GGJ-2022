using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class WaterState : AbilityState
{
    private WaterAbility waterPrimary;
    private WaterAbility waterSecondary;
    private int buildUpKillNumber;
    private float primaryCoolDown;
    private float primaryTimer;
    private const int maxHitCount = 7;
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
        base.Handle();
        primaryTimer += Time.deltaTime;
        if (primaryTimer >= primaryCoolDown)
        {
            canShootPrimary = true;
        }
        if (canShootPrimary && Input.GetKey(KeyCode.Mouse0))
        {
            MakeShootSound();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - (Vector2)_context.player.position);
            var lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            GameObject.Instantiate(waterPrimary.Projectile.ProjectilePrefab, _context.player.position, fire.rotation).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(waterPrimary.Projectile, direction, _context.player, fire);
            projectile.onPrimaryHitEnemy += onPrimaryHit;

            //_source.PlayOneShot(_clip);

            canShootPrimary = false;
            primaryTimer = 0;
        }
    }
    private void MakeShootSound()
    {
        if (waterPrimary.Sound != null)
        {
            _context.audioSource.PlayOneShot(waterPrimary.Sound);
        }
    }

    private void onPrimaryHit(Projectile projectile, EnemySeekController enemySeekController)
    {
        projectile.hitCount++;
        projectile.KeepVelocity();
        Physics2D.IgnoreCollision(projectile.GetCollider, enemySeekController.GetCollider);
        enemySeekController.TakeDamage(Elements.Water, 1);

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

