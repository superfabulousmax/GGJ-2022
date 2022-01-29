using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    private float speed;
    private GameObject damageFX;
    private Rigidbody2D _rb;

    public void Instantiate(ElementProjectile elementProjectile, Vector2 direction)
    {
        this.speed = elementProjectile.Speed;
        this.damageFX = elementProjectile.DamageFX;
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = direction * speed;
        Debug.Log(direction);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
