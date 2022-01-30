using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
public class EnemySeekController : MonoBehaviour
{
    private float _moveSpeed = 1.75f;
    private float _separationSpeed = 1f;
    private Transform _playerTarget;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private const float OffsetToPlayer = 0.65f;
    private const float OffsetToEnemy = 1f;
    private int health;
    private Elements element;
    private GameObject damageVFX;
    private GameObject healVFX;
    private AudioClip primaryDamage;
    private AudioClip secondaryDamage;
    private AudioClip heal;
    private EnemyManager enemyManager;
    private const int fullPrimaryDamage = 40;

    private AudioSource _audioSource;
    public Rigidbody2D GetRigidBody => _rigidBody;
    public Collider2D GetCollider => _collider;

    public event Action<int> onKill;
    public event Action<int> onHeal;
    public event Action<int> onDamage;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
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
        onKill = enemyManager.OnKill;
        onHeal = enemyManager.OnHeal;
        onDamage = enemyManager.OnDamage;
    }

    void FixedUpdate()
    {
        if (_playerTarget)
        {
            Vector2 distance = _playerTarget.position - transform.position;
            Vector2 direction = distance.normalized;

            if (distance.magnitude >= OffsetToPlayer)
            {
                _rigidBody.MovePosition(_rigidBody.position + _moveSpeed * direction * Time.fixedDeltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.CompareTag("Fire Projectile"))
        //{
        //    CalculateDamage(Elements.Fire);
        //}

        //if (collision.gameObject.CompareTag("Water Projectile"))
        //{
        //    CalculateDamage(Elements.Water);
        //}

        //if (collision.gameObject.CompareTag("Earth Projectile"))
        //{
        //    CalculateDamage(Elements.Earth);
        //}

        //if (collision.gameObject.CompareTag("Air Projectile"))
        //{
        //    CalculateDamage(Elements.Air);
        //}
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Secondary Fire Projectile"))
        {
            CalculateDamage(Elements.Fire, 2);
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
            health += amount;
            MakeHealVFX();
            PlayHealSFX();
            onHeal?.Invoke(amount);

        }
        else if(hitWith == GetOpposite())
        {
            var amount = fullPrimaryDamage * factor;
            health -= amount;
            MakeDamageVFX();
            PlayDamageSFX(factor);
            onDamage?.Invoke(amount);
        }
        else
        {
            var amount = fullPrimaryDamage / 2 * factor;
            health -= amount;
            MakeDamageVFX();
            PlayDamageSFX(factor);
            onDamage?.Invoke(amount);
        }
        Mathf.Clamp(health, 0, 200);
        if(health <= 0)
        {
            onKill?.Invoke(1);
            MakeDamageVFX();
            ReleaseResource();
        }
    }


    private void MakeDamageVFX()
    {
        if (damageVFX == null)
            return;
        var damage = Instantiate(damageVFX, transform.position, Quaternion.identity);
        Destroy(damage, 1);
    }

    private void MakeHealVFX()
    {
        if (healVFX == null)
            return;
        var heal = Instantiate(healVFX, transform.position, Quaternion.identity);
        Destroy(heal, 1);
    }

    private void PlayDamageSFX(int amount)
    {
        if(amount > 1)
        {
            if (secondaryDamage == null)
                return;
            _audioSource?.PlayOneShot(secondaryDamage);
        }
        else
        {
            if (primaryDamage == null)
                return;
            _audioSource?.PlayOneShot(primaryDamage);
        }

    }

    private void PlayHealSFX()
    {
        if (heal == null)
            return;
        _audioSource?.PlayOneShot(heal);
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
