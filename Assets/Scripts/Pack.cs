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

    private void PerformMovement()
    {
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
        Debug.Log(string.Format("{0} died.", name));
    }
}
