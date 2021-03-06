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
    private float primaryRadius = 2f;
    private Transform fire;
    private Collider2D playerCollider;
    private FlameThrower flameThrower;

    public override void Enter()
    {
        base.Enter();
        this.firePrimary = abilities.primary as FireAbility;
        this.fireSecondary = abilities.secondary as FireAbility;
        this.primaryCoolDown = firePrimary.Cooldown.x;
        this.primaryTimer = 0;
        this.secondaryCoolDown = fireSecondary.Cooldown.x;
        this.secondaryTimer = 0;
        this.secondaryDuration = fireSecondary.Duration;
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
        DisableSecondary();
        buildUpKillNumber = 0;
        primaryTimer = 0;
        secondaryTimer = 0;
        _context.gamePlayManager.changeAbility -= ChangeAbility;
    }

    public override void Handle()
    {
        base.Handle();

        if (!secondaryActive && canShootPrimary && Input.GetKey(KeyCode.Mouse0))
        {
            MakeShootSound();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)_context.player.position);
            var lookDirection = mousePos;
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            GameObject.Instantiate(firePrimary.Projectile.ProjectilePrefab, _context.player.position, Quaternion.identity).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(playerCollider, projectile.GetCollider);
            projectile.Instantiate(firePrimary.Projectile, direction, _context.player, fire);
            projectile.onHitEnemy += OnPrimaryHit;

            canShootPrimary = false;
            primaryTimer = 0;
        }
        else if (!secondaryActive && Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (buildUpKillNumber >= Constants.SecondaryThreshold)
            {
                EnableSecondary();
            }
        }
    }

    private void MakeShootSound()
    {
        if(firePrimary.Sound != null)
        {
            _context.audioSource.PlayOneShot(firePrimary.Sound);
        }
    }

    private void EnableSecondary()
    {
        UIManager.Instance.SetSecondaryCooldown(Elements.Fire);
        secondaryActive = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)_context.player.position);
        var lookDirection = mousePos;
        var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        fire.rotation = Quaternion.Euler(0, 0, lookAngle);
        direction.Normalize();
        var hasProjectile = GameObject.Instantiate(fireSecondary.Projectile.ProjectilePrefab, _context.player.position, fire.rotation).TryGetComponent<FlameThrower>(out flameThrower);
        if(hasProjectile == false)
        {
            Debug.LogWarning("Projectile not found for fire secondary");
            return;
        }
        flameThrower.gameObject.transform.parent = _context.player;
        var flames = flameThrower.GetComponent<ParticleSystem>();
        var colliderNumber = flames.trigger.colliderCount;
        for(int i = 0; i< colliderNumber; ++i)
        {
            Physics2D.IgnoreCollision(playerCollider, flames.trigger.GetCollider(i).GetComponent<Collider2D>());
        }

        flameThrower.Instantiate(firePrimary.Projectile, direction, _context.player, fire);
    }

    public override void DisableSecondary()
    {
        secondaryTimer = 0;
        secondaryActive = false;
        if (flameThrower != null)
        {
            GameObject.Destroy(flameThrower.gameObject);
        }
    }

    private void OnPrimaryHit(Projectile projectile, EnemySeekController enemySeekController)
    {
        var explosionVFX = GameObject.Instantiate(firePrimary.Vfx);
        explosionVFX.transform.position = projectile.transform.position;

        projectile.GetRigidbody.velocity = Vector2.zero;
        projectile.GetCollider.enabled = false;

        var sprite = projectile.transform.Find("Sprite");
        if (sprite != null)
        {
            sprite.gameObject.SetActive(false);
        }

        projectile.onHitEnemy -= OnPrimaryHit;
        GameObject.Destroy(projectile.gameObject, 0.5f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(projectile.transform.position, primaryRadius);
        foreach (var hitEnemy in hitColliders)
        {
            if (hitEnemy.TryGetComponent(out EnemySeekController hitEnemyController))
            {
                hitEnemyController.TakeDamage(Elements.Fire, 1);
            }
        }
    }
}
