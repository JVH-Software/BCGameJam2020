using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    
    public int numBullets = 3;
    private int shotgunRoF;

    // - Shotgun multipliers!
    private float recoilMultiplier = 1.5f;
    private float damageMultiplier = 2f;
    private float knockbackMultiplier = 1.5f;
    private float projectileMultiplier = 2f;

    private void Awake()
    {
        //- Assumes rate is num bullets per seconds
        // - ex. rateOfFire = 200, 2 bullets per second
        shotgunRoF = rateOfFire  * 2;

        // - Spread depends on number of bullets
        spread *= Mathf.RoundToInt(numBullets * 0.50f);
        recoil *= recoilMultiplier;
        damage *= damageMultiplier;
        knockbackStrength *= knockbackMultiplier;
        projectileSpeed *= projectileMultiplier;
    }

    private void Update()
    {
        fireDelay -= 1;
        if(fireDelay <= 0)
        {
            fireDelay = 0;
        }
    }

    public override void Shoot(Vector3 target, PackMember shooter) 
    {
        if (fireDelay == 0)
        {
            
            for (int i = 0; i < numBullets; i++)
            {
                Vector3 direction = base.computeMovementVector(target, shooter);
                Quaternion rotation = base.computeBulletRotation(direction);
                fireDelay = shotgunRoF;
                GameObject bullet = Instantiate(projectile, transform.position, rotation);
                // - Low despawn time to simulate close range
                bullet.GetComponent<Bullet>().timeToDespawn = 10;
                bullet.GetComponent<Bullet>().Shoot(direction, shooter.pack, shooter, this);

                if (!shooter.pack.upgrades.Contains(Upgrades.NoRecoil))
                    shooter.Knockback(direction * -recoil);
            }
        }
    }
}
