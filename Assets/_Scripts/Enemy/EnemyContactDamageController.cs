using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamageController : MonoBehaviour
{
    [SerializeField] private float _damagePerSecond = 10f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealthController playerHealthController))
        {
            playerHealthController.TakeDamage(_damagePerSecond * Time.deltaTime);
        }
    }
}
