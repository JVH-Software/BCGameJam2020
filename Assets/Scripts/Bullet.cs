using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Rigidbody2D rbody;
    Vector2 movementVector;

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

    public void Shoot(Vector2 movementVector)
    {
        this.movementVector = movementVector;
    }

}
