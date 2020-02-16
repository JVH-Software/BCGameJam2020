using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int timeToDespawn = 1000;
    public GameObject particleHit;
    public GameObject particleShoot;
    public AudioClip bloodHit;

    Rigidbody2D rbody;
    Vector2 movementVector;
    PackMember shooter;
    Gun gun;
    Pack pack;

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
        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime * gun.projectileSpeed * pack.projectileSpeedMultiplier);
    }

    public void Shoot(Vector2 movementVector, Pack pack, PackMember shooter, Gun gun)
    {
        Instantiate(particleShoot, transform.position, transform.rotation);
        this.movementVector = movementVector;
        this.shooter = shooter;
        this.gun = gun;
        this.pack = pack;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooter.GetComponent<Collider2D>());
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        PackMember pm;
        if(coll.gameObject.tag.Equals("UpperBarriers"))
        {
            Instantiate(particleHit, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(coll.gameObject.TryGetComponent<PackMember>(out pm))
        {
            GameObject particles = Instantiate(particleHit, transform.position, transform.rotation);
            particles.GetComponent<AudioSource>().clip = bloodHit;
            particles.GetComponent<AudioSource>().Play();
            particles.GetComponent<ParticleSystem>().startColor = Color.red;

            // No friendly fire (for now)
            if (!coll.tag.Equals(shooter.tag))
            {
                Instantiate(particleHit, transform.position, transform.rotation);
                coll.gameObject.GetComponent<PackMember>().Hit(gun.damage, movementVector * gun.knockbackStrength);
                Destroy(gameObject);
            }
        }
    }
}