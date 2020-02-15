using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{

    public float health = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;
    public GameObject bulletPrefab;
    public float shootSpeedMultiplier = 1f;
    public SimpleHealthBar healthBar;
    public Transform respawnPoint;
    public int respawnTime = 500;

    private int respawnDelay = 0;
    private bool dead = false;

    protected UpgradeList upgrades;

    Rigidbody2D rbody;
    Animator anim;
    protected Vector2 movementVector = Vector2.zero;

    public void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        upgrades = new UpgradeList(this);
    }

    public void FixedUpdate()
    {
        PerformMovement();
    }

    private void Update()
    {
        respawnDelay -= 1;
        if (respawnDelay < 0)
        {
            respawnDelay = 0;
            if(dead)
            {
                dead = false;
                ModifyHealth(maxHealth);
                transform.position = respawnPoint.position;
            }
        }
    }

    private void PerformMovement()
    {

        if (dead)
            return;

        /* Sample code for future animation
    
        if (movementVector != Vector2.zero)
        {
            anim.SetBool("Iswalking", true);
            anim.SetFloat("Input_x", movementVector.x);
            anim.SetFloat("Input_y", movementVector.y);
        }
        else
        {
            anim.SetBool("Iswalking", false);
        }
        */

        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime * speed);
    }
     
    protected void Shoot(Vector3 target)
    {
        if (dead)
            return;

        // Compute bullet movement vector
        Vector3 moveDirection = (target - transform.position);
        moveDirection.z = 0;
        moveDirection.Normalize();

        // Compute bullet rotation
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Create and shoot bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
        bullet.GetComponent<Bullet>().Shoot(moveDirection, this);
    }

    public void Hit(float damage, Vector2 knockback)
    {
        ModifyHealth(-damage);
        rbody.velocity = rbody.velocity + (rbody.position + knockback);
    }

    private void ModifyHealth(float amount)
    {
        health += amount;
        if(health <= 0)
        {
            Death();
        }
        else if(health > maxHealth)
        {
            health = maxHealth;
        }

        if(healthBar != null) {
            healthBar.UpdateBar(health, maxHealth);
        }
    }

    private void Death()
    {
        dead = true;
        respawnDelay = respawnTime;
    }
}
