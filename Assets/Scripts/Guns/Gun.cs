﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject projectile;
    public int rateOfFire = 100;
    public float projectileSpeed = 10f;
    public float damage = 1f;
    public float knockbackStrength = 5f;
    public float recoil = 1f;
    public float spread = 0.2f;

    protected int fireDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay -= 1;
        if (fireDelay <= 0)
        {
            fireDelay = 0;
        }
    }

    public virtual void Shoot(Vector3 target, PackMember shooter)
    {
        if (fireDelay == 0)
        {
            // Compute bullet movement vector
            Vector3 direction = (target - transform.position);
            direction.z = 0;
            direction.Normalize();
            if (!shooter.pack.upgrades.Contains(Upgrades.PerfectAim))
            {
                direction.x = direction.x + Random.Range(-spread, spread);
                direction.y = direction.y + Random.Range(-spread, spread);
            }

            // Reset rate of fire delay
            fireDelay = (int)(rateOfFire/shooter.pack.fireRateMultiplier);

            // Compute bullet rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Create and shoot bullet
            GameObject bullet = Instantiate(projectile, transform.position, rotation);
            bullet.GetComponent<Bullet>().Shoot(direction, shooter.pack, shooter, this);

            //Recoil
            if(!shooter.pack.upgrades.Contains(Upgrades.NoRecoil))
                shooter.Knockback(direction * -recoil);
        }
    }
}
