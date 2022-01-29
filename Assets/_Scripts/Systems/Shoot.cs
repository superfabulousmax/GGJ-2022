using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private Ability currentAbility;
    [SerializeField]
    private Transform fire;
    private Collider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        //fire = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - transform.position);
            var lookDirection = mousePos;
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            fire.rotation = Quaternion.Euler(0, 0, lookAngle);
            direction.Normalize();
            Instantiate(currentAbility.Projectile.ProjectilePrefab, transform.position, Quaternion.identity).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(_collider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(currentAbility.Projectile, fire.right);
        }
    }
}
