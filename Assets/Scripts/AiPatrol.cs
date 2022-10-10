using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private StatusEffectController statusHandler;
    Vector4 colorStorage;

    SpriteRenderer frogSprite;
    void Start()
    {
        mustPatrol = true;
        currentHealth = maxHealth;
        frogSprite = GetComponent<SpriteRenderer>();
        colorStorage = frogSprite.color;
        statusHandler = FindObjectOfType<StatusEffectController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("changing health new health " + currentHealth);
        if (mustPatrol)
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
        walkSpeed *= -1;
        mustPatrol = true;
    }

    private bool hitOnce;
    public int bombDamage = 20;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Frog Collided With " + collision.tag);
        //hitOnce = false;
        if (collision.gameObject.CompareTag("Player"))
        {
            ShroompController player = collision.gameObject.GetComponent<ShroompController>();
            if (player != null && !player.isDashing)
            {
                player.ChangeHealth(-1);
            }
            if (player.isDashing && !isInvincible)
            {

                invincibleTime = 0.2f;
                isInvincible = true;
                ChangeHealth(-1);
                if (currentHealth > 0)
                {
                    dotTime = statusHandler.getDotTime();
                    dotActive = true;
                }
            }
        }
        if (collision.gameObject.CompareTag("Bomb"))
        {
            ChangeHealth(-bombDamage);
        }

    }

    public void ChangeHealth(int amount)
    {
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth == 0)
            Destroy(transform.gameObject);
    }

    private float colorChangeDelay;
    public float colorChangeSetter = 0.1f;
    private bool isColorChanging = true;

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
