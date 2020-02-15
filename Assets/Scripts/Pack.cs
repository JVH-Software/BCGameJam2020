using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{

    public float health = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;

    Rigidbody2D rbody;
    Animator anim;
    protected Vector2 movementVector = Vector2.zero;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        PerformMovement();
    }

    private void PerformMovement()
    {
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

        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime * speed);
    }
     
    private void Shoot()
    {
        Debug.Log(string.Format("{0} shot its gun.", name));
    }

    public void Hit(float damage)
    {
        ModifyHealth(-damage);
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
    }

    private void Death()
    {
        Debug.Log(string.Format("{0} died.", name));
    }
}
