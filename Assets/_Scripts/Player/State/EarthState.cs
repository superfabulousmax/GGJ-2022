using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EarthState : AbilityState
{
    private EarthAbility earthPrimary;
    private EarthAbility earthSecondary;
    private int buildUpKillNumber;
    private float primaryCoolDown;
    private float primaryTimer;
    private float primaryRadius = 1.25f;
    private float secondaryCoolDown;
    private float secondaryTimer;
    private Transform fire;
    private Collider2D playerCollider;
    private bool canShootPrimary;
    private bool canShootSecondary;
    public override void Enter()
    {
        this.earthPrimary = abilities.primary as EarthAbility;
        this.earthSecondary = abilities.secondary as EarthAbility;
        this.primaryCoolDown = earthPrimary.Cooldown.x;
        this.primaryTimer = 0;
        this.secondaryCoolDown = earthSecondary.Cooldown.x;
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
    }

    private void onPrimaryHit(Projectile projectile, EnemySeekController enemySeekController)
    {
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
