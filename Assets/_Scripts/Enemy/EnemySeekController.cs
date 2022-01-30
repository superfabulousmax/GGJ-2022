using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Rigidbody2D GetRigidBody => _rigidBody;
    public Collider2D GetCollider => _collider;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void SpawnAndSeek(Vector3 spawnPosition, Transform playerTransform)
    {
        transform.position = spawnPosition;
        _playerTarget = playerTransform;
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
