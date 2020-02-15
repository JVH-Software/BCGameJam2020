using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float damage = 1f;

    Rigidbody2D rbody;
    Vector2 movementVector;
    GameObject shooter;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime);
    }

    public void Shoot(Vector2 movementVector, GameObject shooter)
    {
        this.movementVector = movementVector;
        this.shooter = shooter;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooter.GetComponent<Collider2D>());
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag.Equals("UpperBarriers"))
        {
            Destroy(gameObject);
        }
        else if(coll.gameObject.tag.Equals("Pack"))
        {
            coll.gameObject.GetComponent<Pack>().Hit(damage);
            Destroy(gameObject);
        }
    }
}