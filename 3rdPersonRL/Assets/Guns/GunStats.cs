using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ShootType
{
    RAYCAST,
    PROJECTILE
}

[System.Serializable]
public class GunStats
{
    public float damage;
    public float fireRate;
    public float clipSize;
    public ShootType shootType;
    public float xAccuracy;
    public float yAccuracy;
    public float range;

    [Header("Projectile Stats")]
    public float projectileSpeed;
}
