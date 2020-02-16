using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    
    public int numBullets = 3;
    private bool canShoot = false;
    private float shotgunRoF;

    // - Shotgun multipliers!
    private int recoilMultiplier = 3;
    private int damageMultiplier = 2;
    private int knockbackMultiplier = 2;
    private int projectileMultiplier = 2;

    private void Awake()
    {
        //- Assumes rate is num bullets per seconds
        // - ex. rateOfFire = 200, 2 bullets per second
        shotgunRoF = (float)rateOfFire / 100;

        // - Spread depends on number of bullets
        spread *= Mathf.RoundToInt(numBullets * 0.50f);
        recoil *= recoilMultiplier;
        damage *= damageMultiplier;
        knockbackStrength *= knockbackMultiplier;
        projectileSpeed *= projectileMultiplier;
        StartCoroutine("FirerateWatcher");
    }

    public override void Shoot(Vector3 target, PackMember shooter) 
    {
        if (canShoot)
        {
            canShoot = false;
            for (int i = 0; i < numBullets; i++)
            {
                Vector3 direction = base.computeMovementVector(target, shooter);
                Quaternion rotation = base.computeBulletRotation(direction);
                
                GameObject bullet = Instantiate(projectile, transform.position, rotation);
                // - Low despawn time to simulate close range
                bullet.GetComponent<Bullet>().timeToDespawn = 10;
                bullet.GetComponent<Bullet>().Shoot(direction, shooter.pack, shooter, this);

                if (!shooter.pack.upgrades.Contains(Upgrades.NoRecoil))
                    shooter.Knockback(direction * -recoil);
            }
        }
    }

    // - Similar stradegy with fireDelay and Update, just only using booleans and coroutines instead.
    IEnumerator FirerateWatcher()
    {
        while (true)
        {
            canShoot = true;
            yield return new WaitForSeconds(1 / shotgunRoF);
        }
        
    }
}
