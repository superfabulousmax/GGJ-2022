using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _spriteGO;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    private float _moveSpeed = 2.5f;
    private Vector2 _moveAxis = Vector2.zero;
    private Vector2 _lookPosition = Vector2.zero;

    private const float MaxDiagonalUpAngle = 150f;
    private const float MaxDiagonalDownAngle = 30f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = _spriteGO.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _moveAxis.x = Input.GetAxisRaw("Horizontal");
        _moveAxis.y = Input.GetAxisRaw("Vertical");
        _lookPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = _lookPosition - _rigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if (angle < 0 && angle >= -90f)
        {
            angle = Mathf.Max(angle, -MaxDiagonalDownAngle);
            FlipSpriteRendererY(false);
        }
        else if (angle < -90f && angle >= -180f)
        {
            angle = Mathf.Min(angle, -MaxDiagonalUpAngle);
            FlipSpriteRendererY(true);
        }
        else if (angle >= 0 && angle < 90f)
        {
            angle = Mathf.Min(angle, MaxDiagonalDownAngle);
            FlipSpriteRendererY(false);
        }
        else if (angle >= 90f && angle < 180f)
        {
            angle = Mathf.Max(angle, MaxDiagonalUpAngle);
            FlipSpriteRendererY(true);
        }

        _rigidbody.rotation = angle;
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _moveSpeed * _moveAxis.normalized * Time.fixedDeltaTime);
    }

    private void FlipSpriteRendererY(bool flag)
    {
        if (_spriteRenderer.flipY != flag)
        {
            _spriteRenderer.flipY = flag;
        }
    }
}
