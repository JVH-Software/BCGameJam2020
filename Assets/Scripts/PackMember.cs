using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PackMember : MonoBehaviour
{
    public float speed = 1f;
    internal Pack pack;
    public float agroRange = 5;

    private Vector2 target;

    public bool dead = false;
    private Gun gun;

    protected UpgradeList upgrades;

    Rigidbody2D rbody;
    Animator anim;
    protected Vector2 movementVector = Vector2.zero;
    public Vector2 _staticTarget;

    protected List<Vector2> points = new List<Vector2>() { new Vector2(-3f, 5f) };

    public void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gun = GetComponent<Gun>();
        pack = transform.parent.GetComponent<Pack>();
    }

    public void FixedUpdate()
    {
        if (Mathf.Abs(Vector2.Distance(this.transform.position, _staticTarget)) > 0.6) {
            PerformMovement();
            
        }
    }


    public void SetTarget(float x, float y) {
        _staticTarget = new Vector2(x, y);
    }
    
    public void Update()
    {
        Move(_staticTarget);
        Vector3 direction;
        if (gameObject.tag == "Player")
            direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        else
            direction = (pack.target.transform.position - transform.position);
        direction.z = 0;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //angle - 90 accounts for sprite rotation
        this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
 
    }


    private void PerformMovement()
    {

        if (dead)
            return;
    
        if (movementVector != Vector2.zero)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        rbody.velocity = (rbody.velocity + movementVector * Time.deltaTime * speed * pack.speedMultiplier);
    }

    public void Shoot(Vector3 target, Pack pack)
    {
        if (dead)
            return;

        gun.Shoot(target, this);

    }

    public void Hit(float damage, Vector2 knockback)
    {
        pack.ModifyHealth(-damage, this);
        Knockback(knockback);
    }

    public void Knockback(Vector2 knockback)
    {
        rbody.velocity = (rbody.velocity + knockback);
    }

    public void MemberDeath()
    {
        this.gameObject.SetActive(false);
        dead = true;
    }

    public void MemberRessurect() {
        this.gameObject.SetActive(true);
        dead = false;
    }

    // Relative to current position
    public void MoveDir(float x, float y) {
        movementVector = new Vector2(x, y);
    }
    public void MoveDir(Vector2 v) {
        movementVector = new Vector2(v.x, v.y);
    }

    public void Move(Vector2 pos)
    {
        Move(pos.x, pos.y);
    }
    public void Move(float x, float y)
    {

        // Movement
        List<Node> path;

        Pathfinding.FindPath(
                new Vector2Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f)), // startpos
                new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y)), // target
                pack.gameManager.ground,
                out path);

        // stop endless stream of error messages
        if (path.Count <= 0) return;

        Vector2 targetNodePos = new Vector2(path[0].Position.x + 0.5f,
                                            path[0].Position.y + 0.5f);
        Vector2 moveDirection = targetNodePos - new Vector2(transform.position.x, transform.position.y);
        moveDirection.Normalize();
        movementVector = moveDirection;
        var requiredDist = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(x, y));
        if (requiredDist < 1) movementVector*= requiredDist;

    }
}
