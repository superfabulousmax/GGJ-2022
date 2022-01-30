using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour, ICreateElement
{
    private float speed;
    private GameObject damageFX;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private Vector2 _direction;

    public int hitCount = 0;
    public event Action<Projectile, EnemySeekController> onPrimaryHitEnemy;
    public Collider2D GetCollider => _collider;
    public Rigidbody2D GetRigidbody => _rigidBody;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Instantiate(ElementProjectile elementProjectile, Vector2 direction, Transform player, Transform fire)
    {
        this.speed = elementProjectile.Speed;
        this.damageFX = elementProjectile.DamageFX;
        _direction = direction;
        KeepVelocity();
    }

    public void KeepVelocity()
    {
        _rigidBody.velocity = _direction * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent(out EnemySeekController enemy))
        {
            onPrimaryHitEnemy?.Invoke(this, enemy);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
