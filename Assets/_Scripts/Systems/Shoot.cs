using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yaSingleton;

public class Shoot : MonoBehaviour
{
    private AbilitySet currentAbilities;
    [SerializeField]
    private Transform fire;
    private Collider2D _collider;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        GamePlayManager.Instance.changeAbility -= ChangeAbility;
    }

    private void ChangeAbility(AbilitySet abilities)
    {
        Debug.Log("Here");
        currentAbilities = abilities;
    }


    void Start()
    {
        _collider = GetComponent<Collider2D>();
        GamePlayManager.Instance.changeAbility += ChangeAbility;
        currentAbilities = GamePlayManager.Instance.CurrentAbilities;
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
            Instantiate(currentAbilities.primary.Projectile.ProjectilePrefab, transform.position, Quaternion.identity).TryGetComponent<Projectile>(out var projectile);
            Physics2D.IgnoreCollision(_collider, projectile.GetComponent<Collider2D>());
            projectile.Instantiate(currentAbilities.primary.Projectile, fire.right);
        }
    }
}
