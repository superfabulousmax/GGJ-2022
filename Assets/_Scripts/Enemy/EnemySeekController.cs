using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
public class EnemySeekController : MonoBehaviour
{
    private float _moveSpeed = 2.2f;
    private float _separationSpeed = 1f;
    private Transform _playerTarget;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private const float OffsetToPlayer = 0.65f;
    private const float OffsetToEnemy = 1f;
    private const float DistanceLimit = 20f;
    private int health;
    private Elements element;

    internal void RespawnEnemy()
    {
        Debug.Log("Respawn Enemy");
        ReleaseResource();
    }

    private GameObject damageVFX;
    private GameObject healVFX;
    private AudioClip primaryDamage;
    private AudioClip secondaryDamage;
    private AudioClip heal;
    private EnemyManager enemyManager;
    private const int fullPrimaryDamage = 50;
    private float damageFxTimer;
    private float healFxTimer;
    private float particleTimer;

    private const float fxCoolDown = 0.4f;
    private const float particleCoolDown = 2f;
    private const float healFxCoolDown = 0.2f;
    private const int maxHealth = 200;
    private AudioSource _audioSource;

    private bool canMove;
    public Rigidbody2D GetRigidBody => _rigidBody;
    public Collider2D GetCollider => _collider;

    public event Action<int> onKill;
    public event Action<int> onHeal;
    public event Action<int> onDamage;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SpawnAndSeek(EnemyManager enemyManager, Vector3 spawnPosition, Transform playerTransform, Elements element, GameObject damageVFX, GameObject healVFX, AudioSource audioSource, AudioClip primaryDamage, AudioClip secondaryDamage, AudioClip heal)
    {
        this.enemyManager = enemyManager;
        transform.position = spawnPosition;
        _playerTarget = playerTransform;
        this.element = element;
        this.damageVFX = damageVFX;
        this.healVFX = healVFX;
        _audioSource = audioSource;
        this.primaryDamage = primaryDamage;
        this.secondaryDamage = secondaryDamage;
        this.heal = heal;
        health = 100;
        damageFxTimer = 0;
        healFxTimer = 0;
        canMove = true;
        onKill = enemyManager.OnKill;
        onHeal = enemyManager.OnHeal;
        onDamage = enemyManager.OnDamage;
        CameraShake.Instance.onStartShake += OnStartShake;
        CameraShake.Instance.onFinishShake += OnFinishShake;
    }

    private void OnDisable()
    {
        CameraShake.Instance.onStartShake -= OnStartShake;
        CameraShake.Instance.onFinishShake -= OnFinishShake;
    }

    private void OnStartShake(float duration)
    {
        canMove = false;
        if (_spriteRenderer.isVisible)
        {
            DealDamage(Elements.Earth, 10);
            PlayDamageSFX(2);
        }
    }

    private void OnFinishShake()
    {
        canMove = true;
    }

    void FixedUpdate()
    {
        if (canMove && _playerTarget)
        {
            Vector2 distance = _playerTarget.position - transform.position;
            if(distance.sqrMagnitude >= DistanceLimit * DistanceLimit)
            {
                RespawnEnemy();
      
            }
            Vector2 direction = distance.normalized;

            if (distance.magnitude >= OffsetToPlayer)
            {
                _rigidBody.MovePosition(_rigidBody.position + _moveSpeed * direction * Time.fixedDeltaTime);
            }
        }
    }

    private IEnumerator DamageOverTime(float time, float damage)
    {
        var timer = 0f;
        PlayDamageSFX(2);
        while (timer < time)
        {
            timer += Time.deltaTime;
            DealDamage(Elements.Earth, (int)damage);
            yield return null;
        }
    }

    private void Update()
    {
        damageFxTimer += Time.deltaTime;
        healFxTimer += Time.deltaTime;
        particleTimer += Time.deltaTime;
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Secondary Fire Projectile"))
        {
            if (particleTimer >= particleCoolDown)
            {
                CalculateDamage(Elements.Fire, 2);
                particleTimer = 0;
            }
        }
    }

    public void TakeDamage(Elements hitType, int factor = 1)
    {
        CalculateDamage(hitType, factor);
    }

    private void CalculateDamage(Elements hitWith, int factor = 1)
    {
        if(element == hitWith)
        {
            var amount = fullPrimaryDamage * factor;
            var oldHeath = health;
            health += amount;
            MakeHealVFX();
            PlayHealSFX();
            healFxTimer = 0;
            amount = Mathf.Min(amount, maxHealth - oldHeath);
            onHeal?.Invoke(amount);

        }
        else if(hitWith == GetOpposite())
        {
            var amount = fullPrimaryDamage * factor;
            health -= amount;
            MakeDamageVFX();
            PlayDamageSFX(factor);
            damageFxTimer = 0;
            onDamage?.Invoke(amount);
        }
        else
        {
            var amount = fullPrimaryDamage / 2 * factor;
            health -= amount;
            MakeDamageVFX();
            PlayDamageSFX(factor);
            damageFxTimer = 0;
            onDamage?.Invoke(amount);
        }
        ClampAndCheckHealth();
    }

    private void ClampAndCheckHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            onKill?.Invoke(1);
            MakeDamageVFX();
            ReleaseResource();
        }
    }

    private void DealDamage(Elements hitWith, int amount)
    {
        health -= amount;
        MakeDamageVFX();
        damageFxTimer = 0;
        onDamage?.Invoke(amount);
        ClampAndCheckHealth();
    }

    private void MakeHealVFX()
    {
        if (healVFX == null)
            return;
        if (healFxTimer >= healFxCoolDown)
        {
            var heal = Instantiate(healVFX, transform.position, Quaternion.identity);
            Destroy(heal, 1);
        }
    }

    private void PlayHealSFX()
    {
        if (heal == null)
            return;
        if (healFxTimer >= fxCoolDown)
        {
            AudioSystem.Instance.PlaySound(heal, transform.position, 0.6f);
        }

    }

    private void MakeDamageVFX()
    {
        if (damageVFX == null)
            return;
        if (damageFxTimer >= fxCoolDown)
        {
            var damage = Instantiate(damageVFX, transform.position, Quaternion.identity);
            Destroy(damage, 1);
        }
    }

    private void PlayDamageSFX(int amount)
    {
        if(amount > 1)
        {
            if (secondaryDamage == null)
                return;
            AudioSystem.Instance.PlaySecondarySound(secondaryDamage, transform.position, 0.5f);
        }
        else
        {
            if (primaryDamage == null)
                return;
            if (damageFxTimer >= fxCoolDown)
            {
                AudioSystem.Instance.PlaySound(primaryDamage, transform.position, 0.6f);
            }
        }

    }


    private Elements GetOpposite()
    {
        if (element == Elements.Fire)
            return Elements.Water;
        else if (element == Elements.Water)
            return Elements.Fire;
        else if (element == Elements.Air)
            return Elements.Earth;
        return Elements.Air;
    }

    private void ReleaseResource()
    {
        enemyManager.ReleaseElement(element, this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemySeekController enemyController))
        {
            Vector2 distance = transform.position - collision.transform.position;
            float magnitude = distance.magnitude;
            if (magnitude < OffsetToEnemy)
            {
                //_rigidBody.MovePosition(_rigidBody.position + _separationSpeed * distance.normalized * Time.deltaTime);
                transform.Translate(distance.normalized * _separationSpeed * Time.deltaTime);
            }
        }
    }

}
