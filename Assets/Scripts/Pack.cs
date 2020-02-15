using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{

    public float health = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;
    public GameObject bulletPrefab;
    public float shootSpeed = 20f;

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

        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime * speed);
    }
     
    protected void Shoot()
    {
        // Compute bullet movement vector
        Vector3 moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        moveDirection.z = 0;
        moveDirection.Normalize();
        moveDirection = moveDirection* shootSpeed;

        // Compute bullet rotation
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Create and shoot bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
        bullet.GetComponent<Bullet>().Shoot(moveDirection);
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
