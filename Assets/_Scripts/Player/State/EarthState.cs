using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EarthState : AbilityState
{
    private EarthAbility earthPrimary;
    private EarthAbility earthSecondary;
    private int buildUpKillNumber;
    private float primaryRadius = 2f;
    private float secondaryRadius = 6f;
    private float shakeAmount = 0.5f;
    private bool canEnableSecondary = true;
    private Transform fire;
    private Collider2D playerCollider;
    public override void Enter()
    {
        base.Enter();
        this.earthPrimary = abilities.primary as EarthAbility;
        this.earthSecondary = abilities.secondary as EarthAbility;
        this.primaryCoolDown = earthPrimary.Cooldown.x;
        this.primaryTimer = 0;
        this.secondaryDuration = earthSecondary.Duration;
        this.secondaryCoolDown = earthSecondary.Cooldown.x;
        this.secondaryTimer = 0;
        buildUpKillNumber = 10;
        fire = _context.player.Find("Fire");
        playerCollider = _context.player.GetComponent<Collider2D>();
        _context.gamePlayManager.changeAbility += ChangeAbility;
        CameraShake.Instance.onFinishShake += DisableSecondary;
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
        CameraShake.Instance.onFinishShake -= DisableSecondary;
    }

    public override void Handle()
    {
        base.Handle();

        if (canShootPrimary && Input.GetKey(KeyCode.Mouse0))
        {
            MakeShootSound();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)_context.player.position);
            var lookDirection = mousePos;
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            GameObject.Instantiate(earthPrimary.Projectile.ProjectilePrefab, _context.player.position, Quaternion.identity).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(earthPrimary.Projectile, direction, _context.player, fire);
            projectile.onPrimaryHitEnemy += onPrimaryHit;

            canShootPrimary = false;
            primaryTimer = 0;
        }
        else if (canEnableSecondary && !secondaryActive && Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (buildUpKillNumber >= Constants.SecondaryThreshold)
            {
                EnableSecondary();
            }
        }
    }

    public override void HandleCoolDown()
    {
        if(!canEnableSecondary)
        {
            secondaryTimer += Time.deltaTime;
            if (secondaryTimer >= secondaryCoolDown)
            {
                canEnableSecondary = true;
                secondaryTimer = 0;
            }
        }
      
        primaryTimer += Time.deltaTime;
        if (primaryTimer >= primaryCoolDown)
        {
            canShootPrimary = true;
        }
    }

    public void EnableSecondary()
    {
        canEnableSecondary = false;
        secondaryActive = true;
        CameraShake.Shake(secondaryDuration, shakeAmount);
    }

    public override void DisableSecondary()
    {
        secondaryActive = false;
    }

    private void MakeShootSound()
    {
        if (earthPrimary.Sound != null)
        {
            _context.audioSource.PlayOneShot(earthPrimary.Sound);
        }
    }

    private void onPrimaryHit(Projectile projectile, EnemySeekController enemySeekController)
    {
        var explosionVFX = GameObject.Instantiate(earthPrimary.Vfx);
        explosionVFX.transform.position = projectile.transform.position;

        projectile.GetRigidbody.velocity = Vector2.zero;
        projectile.GetCollider.enabled = false;

        var sprite = projectile.transform.Find("Sprite");
        if (sprite != null)
        {
            sprite.gameObject.SetActive(false);
        }

        projectile.onPrimaryHitEnemy -= onPrimaryHit;
        GameObject.Destroy(projectile.gameObject, 0.5f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(projectile.transform.position, primaryRadius);
        foreach (var hitEnemy in hitColliders)
        {
            if (hitEnemy.TryGetComponent(out EnemySeekController hitEnemyController))
            {
                hitEnemyController.TakeDamage(Elements.Earth, 1);
            }
        }
    }
}
