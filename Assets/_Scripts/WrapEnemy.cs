using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<EnemySeekController>(out var enemySeekController))
        {
            enemySeekController.RespawnEnemy();
        }
    }
}
