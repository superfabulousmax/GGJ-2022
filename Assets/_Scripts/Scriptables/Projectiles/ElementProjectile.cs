using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Projectiles/Basic Projectile", fileName = "New Projectile")]
public class ElementProjectile : ScriptableObject
{
    [ShowAssetPreview, SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject damageFX;

    public GameObject ProjectilePrefab { get => projectile; }
    public float Speed { get => speed;  }
    public GameObject DamageFX { get => damageFX; }
}
