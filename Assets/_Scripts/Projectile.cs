using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour, ICreateElement
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
