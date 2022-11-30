using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AiPatrol : MonoBehaviour
{
    public float walkSpeed;
    public int maxHealth = 5;
    int currentHealth;
    [HideInInspector]
    public bool mustPatrol;
    private bool mustTurn;
   

    public Rigidbody2D rb;
    public Transform groundCheckPos;
    public LayerMask groundLayer;

    private bool isInvincible = false;
    private float invincibleTime;
    private float dotTime, dotInvulTime;
    private bool dotActive;
    private bool dotIsInvul = false;
    public float dotInvulSetter = 0.5f;
    public int fireWeakness = 1;
    public int iceWeakness = 1;
    public float maxVelocity = 6;
    public Transform groundCheck;
    private StatusEffectController statusHandler;
    Vector4 colorStorage;

    SpriteRenderer frogSprite;
    SpriteRenderer FrogJump;



    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 25f;
    public float pathUpdate = 0.5f;

    public float nextWaypointDistance = 3f;
    public float jumpNodeHeight = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheck = 0.1f;

    public bool followEndable = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    Seeker seeker;
    private Vector2 currentVelocity;

    [Header("not Pathfinding")]
    private bool hitOnce;
    public int bombDamage = 20;
    private float colorChangeDelay;
    public float colorChangeSetter = 0.1f;
    private bool isColorChanging = true;

    [Header("Debug")]
    public bool isGrounded = false;


    ShroompController player;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        mustPatrol = true;
        currentHealth = maxHealth;
        frogSprite = GetComponent<SpriteRenderer>();
        FrogJump = GetComponent<SpriteRenderer>();
        colorStorage = frogSprite.color;
        statusHandler = FindObjectOfType<StatusEffectController>();
        
        InvokeRepeating("UpdatePath", 0f, pathUpdate);
        player = FindObjectOfType<ShroompController>();
        target = FindObjectOfType<ShroompController>().transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (TargetInDistance() && followEndable)
        {

            PathFollow();
        }
        else 
        {
            Patrol();
        }
        dotHandler();
    }

    private void FixedUpdate()
    {

        if (mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }
        //Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheck);
        //isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.1f);
        isGrounded = groundCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks"));
    }

    private void UpdatePath()
    {
        if(followEndable && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collision detected with: " + other);
        if (other.CompareTag("Bomb"))
        {
            ChangeHealth(-1);
        }
    }
    private void PathFollow()
    {
        
        if (path == null)
        {
            
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            
            return;
        }

        // See if colliding with anything
        //Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheck);
        //isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * walkSpeed * Time.deltaTime;

        // Jump
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeight)
            {
                rb.AddForce(Vector2.up * walkSpeed * jumpModifier);
            }
        }

        // Movement
        rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    void Patrol()
    {
        if (mustTurn)
        {
            Flip();
        }
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        //walkSpeed *= -1;
        mustPatrol = true;
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Frog Collided With " + collision.tag);
        //hitOnce = false;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null && !player.isDashing)
            {

                player.ChangeHealth(-1);
            }
            if (player.isDashing && !isInvincible)
            {

                invincibleTime = 0.2f;
                isInvincible = true;
                int damage = 10;
                if (statusHandler.wideUp)
                    damage = damage * 2;
                ChangeHealth(-damage);
                if (currentHealth > 0)
                {
                    dotTime = statusHandler.getDotTime();
                    dotActive = true;
                }
            }
        }
        if (collision.gameObject.CompareTag("PlayerShadow"))
            ChangeHealth(-5);
        if (collision.gameObject.CompareTag("Bomb"))
        {
            ChangeHealth(-bombDamage);
        }

    }
    private bool stopDeadRepeat = false;
    public void ChangeHealth(int amount)
    {
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth == 0)
        {
            if (!stopDeadRepeat)
            {
                FindObjectOfType<RandomCoinSpawner>().spawnRandom(transform);
                Destroy(transform.gameObject);
                stopDeadRepeat = true;
            }
        }
    }

    
    void dotHandler()
    {
        if (dotActive && !dotIsInvul)
        {

            if (statusHandler.fireUp)
            {
                frogSprite.color = new Color(1f, 0f, 0f, 1f); 
                ChangeHealth(-5*fireWeakness);
                dotInvulTime = dotInvulSetter;
                dotIsInvul = true;
                colorChangeDelay = colorChangeSetter;
                isColorChanging = true;
            }


        }
        if(dotActive)
        {
            if (statusHandler.iceUp)
            {
                colorChangeDelay -= Time.deltaTime;
                if (colorChangeDelay < 0)
                frogSprite.color = new Color(0f, 0f, 1f, 1f);
                maxVelocity = Mathf.Clamp(6-iceWeakness, 1, 6);
            }
            dotTime -= Time.deltaTime;
            if (dotTime < 0)
            {
                dotActive = false;
                //Restore max velocity and then color
                maxVelocity = 6f;
                frogSprite.color = colorStorage;
            }
        }
        if(dotIsInvul)
        {
            dotInvulTime -= Time.deltaTime;
            if (dotInvulTime < 0)
                dotIsInvul = false;
        }

    }


}
