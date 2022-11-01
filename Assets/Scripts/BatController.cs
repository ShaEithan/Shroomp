using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BatController : MonoBehaviour
{
    //Status handle script
    private StatusEffectController statusHandler;
    private float dotTime,dotInvulTime;
    private bool dotActive;
    private bool dotIsInvul = false;
    public float dotInvulSetter = 0.5f;

    public float speed;
    public int maxHealth = 100;
    int currentHealth;
    public float detectRadius = 2f;
    public float movePower = 7f;
    bool isDetecting, alerted;
    private float invincibleTime;
    private bool isInvincible = false;

    Vector2 charPos,batPos,moveDirection;
    private Transform mainChar;

    public float maxVelocity = 6;

    Rigidbody2D rigidbody2D;

    TextMeshProUGUI healthText;

    public int fireWeakness = 1;
    public int iceWeakness = 1;

    SpriteRenderer batSprite;
    Animator animator;
    Vector4 colorStorage;
    ShroompController player;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        mainChar = GameObject.Find("ShroompSprite").transform;
        healthText = transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        currentHealth = maxHealth;
        batSprite = GetComponent<SpriteRenderer>();
        colorStorage = batSprite.color;
        statusHandler = FindObjectOfType<StatusEffectController>();
        animator = transform.GetComponent<Animator>();
        player = FindObjectOfType<ShroompController>();
    }

    // Update is called once per frame
    void Update()
    {
        charPos = mainChar.position;
        batPos = transform.position;
        if (batPos.x > charPos.x)
        {
            moveDirection.x = batPos.x - charPos.x;
        }
        if (batPos.x < charPos.x)
        {
            moveDirection.x = -1 * (charPos.x - batPos.x);
        }
        if (batPos.y > charPos.y)
        {
            moveDirection.y = batPos.y - charPos.y;
        }
        if (batPos.y < charPos.y)
        {
            moveDirection.y = -1 * (charPos.y - batPos.y);
        }
        animator.SetFloat("LookX", moveDirection.x);
        moveDirection.Normalize();

        //Handles invincibility time when hit by player
        if (invincibleTime > 0)
        { 
            invincibleTime -= Time.deltaTime;
            if(invincibleTime <0)
            isInvincible = false;
        }

        healthText.text = (currentHealth + " / " + maxHealth);
        dotHandler();
        if (isdead)
            Destroy(transform.gameObject);
    }
    private void FixedUpdate()
    {
        isDetecting = Physics2D.OverlapCircle(rigidbody2D.position, detectRadius, LayerMask.GetMask("Shroomp"));
        if (isDetecting) alerted = true;
        if (alerted)
        {
            rigidbody2D.AddForce(new Vector2(-1 * movePower * moveDirection.x , -1 * movePower * moveDirection.y ));
            //if force being added is making bat too fast lowers it back down to maxVelocity, set as 6 for now
            if(rigidbody2D.velocity.magnitude > maxVelocity)
            {
                rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxVelocity;
            }
        }
    }
    private bool hitOnce;
    public int bombDamage =20;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bat Collided With " + collision.tag);
        //hitOnce = false;
        if (collision.gameObject.CompareTag("Player"))
        {
            //ShroompController player = collision.gameObject.GetComponent<ShroompController>();
            if (player != null && !player.isDashing)
            {
                player.ChangeHealth(-1);
            }
            if (player.isDashing && !isInvincible)
            {

                invincibleTime = 0.2f;
                isInvincible = true;
                ChangeHealth(-1);
                animator.SetTrigger("Damage");
                if (currentHealth > 0)
                {
                    dotTime = statusHandler.getDotTime();
                    dotActive = true;

                }
            }
        }
        if (collision.gameObject.CompareTag("PlayerShadow"))
        {
            ChangeHealth(-5);
            animator.SetTrigger("Damage");
        }
        if (collision.gameObject.CompareTag("Bomb"))
        {
            ChangeHealth(-bombDamage);
            animator.SetTrigger("Damage");
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

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth == 0)
        {
            animator.SetTrigger("Dead");
        }
    }
    [SerializeField]
    private bool isdead = false;

    //Used to stop color overrieds from ice, sets a delay before a power up can alter color
    private float colorChangeDelay;
    public float colorChangeSetter = 0.1f;
    private bool isColorChanging = true;
    void dotHandler()
    {
        if (dotActive && !dotIsInvul)
        {

            if (statusHandler.fireUp)
            {
                batSprite.color = new Color(1f, 0f, 0f, 1f); 
                ChangeHealth(-5*fireWeakness);
                dotInvulTime = dotInvulSetter;
                dotIsInvul = true;
                colorChangeDelay = colorChangeSetter;
                isColorChanging = true;
                animator.SetTrigger("Damage");
            }


        }
        if(dotActive)
        {
            if (statusHandler.iceUp)
            {
                colorChangeDelay -= Time.deltaTime;
                if (colorChangeDelay < 0)
                batSprite.color = new Color(0f, 0f, 1f, 1f);
                maxVelocity = Mathf.Clamp(6-iceWeakness, 1, 6);
            }
            dotTime -= Time.deltaTime;
            if (dotTime < 0)
            {
                dotActive = false;
                //Restore max velocity and then color
                maxVelocity = 6f;
                batSprite.color = colorStorage;
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
