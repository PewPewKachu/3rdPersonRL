using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    [SerializeField] GunStats stats;
    [SerializeField] Transform gunMuzzle;

    [Header("Projectile Shooting")]
    [SerializeField] GameObject projectileToShoot;

    float fireCooldown = 0f;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleCoolDowns();
	}

    private void HandleCoolDowns()
    {
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    public void Shoot(RaycastHit rayHit)
    {
        if (CanShoot() == false)
        {
            return;
        }

        switch (stats.shootType)
        {
            case ShootType.RAYCAST:
                int layermask = ~(1 << LayerMask.NameToLayer("Player"));
                
                //Vector3 dir = new Vector3(rayHit.point.x, rayHit.point.y, rayHit.point.z);
                Vector3 dir = rayHit.point - gunMuzzle.position;
                RaycastHit hit;
                if (Physics.Raycast(gunMuzzle.position, dir.normalized, out hit, stats.range, layermask))
                {
                    Debug.DrawRay(gunMuzzle.position, dir.normalized * stats.range, Color.green);
                }

                Debug.Log(hit.transform.name);
                fireCooldown = stats.fireRate;
                break;
            case ShootType.PROJECTILE:
                break;
            default:
                break;
        }
    }

    bool CanShoot()
    {
        if (fireCooldown <= 0)
        {
            return true;
        }

        return false;
    }
}
