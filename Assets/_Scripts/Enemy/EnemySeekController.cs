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
    private const int fullPrimaryDamage = 40;
    public Rigidbody2D GetRigidBody => _rigidBody;
    public Collider2D GetCollider => _collider;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void SpawnAndSeek(Vector3 spawnPosition, Transform playerTransform, Elements element, GameObject damageVFX)
    {
        transform.position = spawnPosition;
        _playerTarget = playerTransform;
        this.element = element;
        this.damageVFX = damageVFX;
        health = 100;
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
        if(collision.gameObject.CompareTag("Fire Projectile"))
        {
            CalculateDamage(Elements.Fire);
        }
        if (collision.gameObject.CompareTag("Secondary Fire Projectile"))
        {
            CalculateDamage(Elements.Fire, 2);
        }

        if (collision.gameObject.CompareTag("Water Projectile"))
        {
            CalculateDamage(Elements.Water);
        }
        if (collision.gameObject.CompareTag("Earth Projectile"))
        {
            CalculateDamage(Elements.Earth);
        }
        if (collision.gameObject.CompareTag("Air Projectile"))
        {
            CalculateDamage(Elements.Air);
        }
    }

    private void CalculateDamage(Elements hitWith, int factor = 1)
    {
        if(element == hitWith)
        {
            health += fullPrimaryDamage * factor;

        }
        else if(hitWith == GetOpposite())
        {
            health -= fullPrimaryDamage * factor;
            MakeDamageVFX();
        }
        else
        {
            health -= fullPrimaryDamage/2 * factor;
            MakeDamageVFX();
        }
        Mathf.Clamp(health, 0, 200);
        if(health <= 0)
        {
            MakeDamageVFX();
            Destroy(gameObject);
        }
    }

    private void MakeDamageVFX()
    {
        if (damageVFX == null)
            return;
        var damage = Instantiate(damageVFX, transform.position, Quaternion.identity);
        Destroy(damage, 1);
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
