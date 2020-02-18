using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Gun
{

    public new int rateOfFire = 50;
    public int rateOfBurst = 10;
    public int burstAmount = 9;

    public new float projectileSpeed = 10f;
    public new float damage = 1f;
    public new float knockbackStrength = 0.5f;
    public new float recoil = 0.5f;
    public new float spread = 0.05f;
    private int burstStatus = 9;

    public override void Shoot(Vector3 target, PackMember shooter) {
        if (fireDelay == 0) {
            // Compute bullet movement vector
            Vector3 direction = (target - transform.position);
            direction.z = 0;
            direction.Normalize();
            direction.x = direction.x + Random.Range(-spread, spread);
            direction.y = direction.y + Random.Range(-spread, spread);

            // Reset rate of fire delay
            if (burstStatus == 0) {
                burstStatus = burstAmount;
                fireDelay = rateOfFire;
            } else {
                burstStatus --;
                if (burstStatus <= 0) burstStatus = 0;
                fireDelay = rateOfBurst;           
            }          

            // Compute bullet rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Create and shoot bullet
            GameObject bullet = Instantiate(projectile, transform.position, rotation);
            bullet.GetComponent<Bullet>().Shoot(direction, shooter.pack, shooter, this);
            shooter.Knockback(direction * -recoil);
        }
    }
}
