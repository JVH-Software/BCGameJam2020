using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int timeToDespawn = 1000;
    public GameObject particleHit;
    public GameObject particleShoot;

    Rigidbody2D rbody;
    Vector2 movementVector;
    Pack shooter;
    Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToDespawn -= 1;
        if(timeToDespawn <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime * gun.projectileSpeed * shooter.projectileSpeedMultiplier);
    }

    public void Shoot(Vector2 movementVector, Pack shooter, Gun gun)
    {
        Instantiate(particleShoot, transform.position, transform.rotation);
        this.movementVector = movementVector;
        this.shooter = shooter;
        this.gun = gun;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooter.GetComponent<Collider2D>());
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag.Equals("UpperBarriers"))
        {
            Instantiate(particleHit, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(coll.gameObject.tag.Equals("Pack"))
        {
            Instantiate(particleHit, transform.position, transform.rotation);
            coll.gameObject.GetComponent<Pack>().Hit(gun.damage, movementVector * gun.knockbackStrength);
            Destroy(gameObject);
        }
    }
}