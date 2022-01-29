using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour, ICreateElement
{
    private Transform player;
    private Transform fire;
    public void Instantiate(ElementProjectile elementProjectile, Vector2 direction, Transform player, Transform fire)
    {
        this.player = player;
        this.fire = fire;

    }

    void Start()
    {
        
    }
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePos - player.position);
        var lookDirection = mousePos;
        var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        fire.rotation = Quaternion.Euler(0, 0, lookAngle);
        transform.rotation = fire.rotation;
    }
}
