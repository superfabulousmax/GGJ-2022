using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour, ICreateElement
{
    private float speed;
    private GameObject damageFX;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

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
        _rigidBody.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //if(col.gameObject.TryGetComponent<ICreateElement>(out var element))
        //{
        //    return;
        //}
        if(col.gameObject.TryGetComponent<EnemySeekController>(out var enemyController))
        {
            _rigidBody.velocity = Vector2.zero;
            _collider.enabled = false;
            var sprite = transform.Find("Sprite");
            if (sprite != null)
            {
                sprite.gameObject.SetActive(false);
            }

            Destroy(gameObject, 0.5f);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
