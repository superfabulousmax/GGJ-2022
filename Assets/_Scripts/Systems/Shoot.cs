using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private Ability currentAbility;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var projectilePrefab = currentAbility.Projectile;
            Instantiate(projectilePrefab).TryGetComponent<Projectile>(out var projectile);
        }
    }
}
