using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackMember : MonoBehaviour
{

    public float health = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;
    public Pack pack;

    private int respawnDelay = 0;
    private bool dead = false;
    private Gun gun;

    protected UpgradeList upgrades;

    Rigidbody2D rbody;
    Animator anim;
    protected Vector2 movementVector = Vector2.zero;

    public void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }

    public void FixedUpdate()
    {
        PerformMovement();
    }

    public void Update()
    {
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

        rbody.velocity = (rbody.velocity + movementVector * Time.deltaTime * speed * pack.speedMultiplier);
    }

    public void Shoot(Vector3 target, Pack pack)
    {
        if (dead)
            return;

        gun.Shoot(target, pack, this);

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
        if (health <= 0)
        {
            Death();
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }

        pack.UpdateHealth();
    }

    private void Death()
    {
        dead = true;
    }
}
