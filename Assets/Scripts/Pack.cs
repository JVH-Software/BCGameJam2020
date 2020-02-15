using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{

    public float health = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;
    public float projectileSpeedMultiplier = 1f;
    public SimpleHealthBar healthBar;
    public Transform respawnPoint;
    public int respawnTime = 500;

    private int respawnDelay = 0;
    private bool dead = false;
    public Gun gun;

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

    public void Update()
    {
        respawnDelay--;
        Debug.Log(respawnDelay);
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

        rbody.velocity = (rbody.velocity + movementVector * Time.deltaTime * speed);
    }
     
    protected void Shoot(Vector3 target)
    {
        if (dead)
            return;

        gun.Shoot(target, this);

    }

    public void Hit(float damage, Vector2 knockback)
    {
        ModifyHealth(-damage);
        Knockback(knockback);
    }

    public void Knockback(Vector2 knockback)
    {
        rbody.velocity = (rbody.velocity + knockback);
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
