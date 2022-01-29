using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour, ICreateElement
{
    private float speed;
    private GameObject damageFX;
    private Rigidbody2D _rb;

    public void Instantiate(ElementProjectile elementProjectile, Vector2 direction, Transform player, Transform fire)
    {
        this.speed = elementProjectile.Speed;
        this.damageFX = elementProjectile.DamageFX;
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //if(col.gameObject.TryGetComponent<ICreateElement>(out var element))
        //{
        //    return;
        //}
        if(col.gameObject.TryGetComponent<EnemyController>(out var enemyController))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
