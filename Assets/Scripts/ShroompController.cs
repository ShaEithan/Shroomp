using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShroompController : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpPower = 10;
    public float dashPower = 50;
    public int dashAmmount = 3;
    //int isJumping = 0;
    Rigidbody2D rigidbody2d;

    public ParticleSystem impactEffect;

    bool isGrounded;
    bool isTouchingLeft, isTouchingRight, isTouchingUp;

    public float checkRadius;
    public Transform groundCheck, leftCheck, rightCheck, upCheck;

    int dashes;
    bool isDashing;
    private float dashingTimer;
    public float dashTime = 0.05f;
    float jumpDelay;
    Animator animator;
    float dashSpriteDirection;
    Transform ShroompSpriteT;
    Vector2 direction;
    Vector2 mouseDirection;
    Vector2 lookDirection = new Vector2(1, 0);
    Vector2 charOrigin;

    private float lastImageXpos;

    [SerializeField]
    private int maxHealth = 5;
    private int currentHealth =5;
    private float timeInvincible = 2.0f;

    bool isInvincible;
    float invincibleTimer;

    TextMeshProUGUI textHealth;
    Transform healthCanvasPos;

    //Circle rectile stuff
    Transform rangeCircle,rectile;
    private float rangeOffset,reticleOffset;

    public float testX = 1f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("ShroompSprite").GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        ShroompSpriteT = GameObject.Find("ShroompSprite").GetComponent<Transform>();
        textHealth = GameObject.Find("charHealthText").GetComponent<TMPro.TextMeshProUGUI>();
        healthCanvasPos = GameObject.Find("CanvasHealth").GetComponent<Transform>();
        rangeCircle = GameObject.Find("RangeCircle").GetComponent<Transform>();
        rectile = GameObject.Find("Reticle").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Sets char origin
        charOrigin = new Vector2(Camera.main.WorldToScreenPoint(rigidbody2d.position).x, Camera.main.WorldToScreenPoint(rigidbody2d.position).y);
        //Assigns x and y values based on char position
        if (Input.mousePosition.x > charOrigin.x)
        {
            mouseDirection.x = Input.mousePosition.x - charOrigin.x;
        }
        if (Input.mousePosition.x < charOrigin.x)
        {
            mouseDirection.x = -1 * (charOrigin.x - Input.mousePosition.x);
        }
        if (Input.mousePosition.y > charOrigin.y)
        {
            mouseDirection.y = Input.mousePosition.y - charOrigin.y;
        }
        if (Input.mousePosition.y < charOrigin.y)
        {
            mouseDirection.y = -1 * (charOrigin.y - Input.mousePosition.y);
        }
        //Normalizes the mouse direction vector
        mouseDirection.Normalize();

        Vector2 move = new Vector2(direction.x, 0);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", 0);
        animator.SetFloat("Speed", move.magnitude);
        if (isDashing)
        {
            dashingTimer -= Time.deltaTime;

            if (dashingTimer < 0)
            {
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);

                //Re adds gravity scale when dash ends by impact
                rigidbody2d.gravityScale = 1;

                //Makes velocity zero when dash ends
                rigidbody2d.velocity = new Vector2(0, 0);
            }
            dashingEffect();
        }
        if (jumpDelay > 0)
        {
            jumpDelay -= Time.deltaTime;

        }
        else if (isGrounded == true)
        {
            dashes = 0;
            Debug.Log("I am grounded");
            animator.SetBool("Jumping", false);
            animator.SetBool("Dash", false);
            ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);

        }

        if (Input.GetKeyDown("space") && dashes > 0)
        {
            Debug.Log("Dash attempt");
            animator.SetBool("Dash", true);
            rigidbody2d.velocity = new Vector2(mouseDirection.x * dashPower, mouseDirection.y * dashPower);
            dashes = dashes - 1;
            isDashing = true;
            //Set gravit to 0 for dashing
            rigidbody2d.gravityScale = 0;
            dashingTimer = dashTime;
        }

        else if (Input.GetKeyDown("space") && isGrounded == true)
        {
            Debug.Log("Normal jump attempt");
            animator.SetBool("Jumping", true);
            rigidbody2d.velocity = Vector2.up * jumpPower;
            dashes = dashAmmount;
            jumpDelay = .05f;
        }
        //Trajectory code when dashing for rotation of sprite
        if (isDashing)
        {

            //Calculate angle trijectory angle
            if (rigidbody2d.velocity.x > 0)
            {
                dashSpriteDirection = (Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90;
            }
            //Calculate angle trijectory angle
            else if (rigidbody2d.velocity.x < 0)
            {
                dashSpriteDirection = (Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90 * -1;
            }
            else if (rigidbody2d.velocity.x == 0)
            {
                dashSpriteDirection = (Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90;
            }
            //ShroompSpriteT.rotation = Quaternion.Slerp(ShroompSpriteT.rotation, Quaternion.Euler(0, 0, dashSpriteDirection), 1);
            ShroompSpriteT.rotation = Quaternion.Euler(0, 0, dashSpriteDirection);

            //Code for impacts and particle effcts
            Vector2 impactVelocity = new Vector2(0, 0);
            if (isTouchingUp)
            {
                rigidbody2d.velocity = impactVelocity;
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = upCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(90, 90, 0);
                impactEffect.Play();
                //Re adds gravity scale when dash ends by impact
                rigidbody2d.gravityScale = 1;
            }
            else if (isTouchingLeft)
            {
                rigidbody2d.velocity = impactVelocity;
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = leftCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(0, 90, 90);
                impactEffect.Play();

                //Re adds gravity scale when dash ends by impact
                rigidbody2d.gravityScale = 1;
            }
            else if (isTouchingRight)
            {
                rigidbody2d.velocity = impactVelocity;
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = rightCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(0, -90, -90);
                impactEffect.Play();

                //Re adds gravity scale when dash ends by impact
                rigidbody2d.gravityScale = 1;
            }
            else if (isGrounded)
            {
                rigidbody2d.velocity = impactVelocity;
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = groundCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(-90, 90, 0);
                impactEffect.Play();

                //Re adds gravity scale when dash ends by impact
                rigidbody2d.gravityScale = 1;
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        textHealth.text = (currentHealth + " / " + maxHealth);
        healthCanvasPos.position = new Vector2(rigidbody2d.position.x, rigidbody2d.position.y + 0.5f);

        //Range circle and rectile position code
        rangeOffset = ((dashPower * dashTime) * 0.13f);
        //recticleOffset = ((dashPower * dashTime) * (31f / 450f));


        rangeCircle.transform.localScale = new Vector2(rangeOffset, rangeOffset);
        rectile.transform.localPosition = new Vector2((mouseDirection.x * dashPower) *dashTime, (mouseDirection.y * dashPower) * dashTime);
        //Code for rotation the circle
        rangeCircle.transform.Rotate(0, 0, -8 * Time.deltaTime);
    }
    void FixedUpdate()
    {
        if (!isDashing)
        {
            rigidbody2d.velocity = new Vector2(direction.x * speed * Time.deltaTime, rigidbody2d.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, LayerMask.GetMask("Blocks"));
        isTouchingLeft = Physics2D.OverlapCircle(leftCheck.position, checkRadius, LayerMask.GetMask("Blocks"));
        isTouchingRight = Physics2D.OverlapCircle(rightCheck.position, checkRadius, LayerMask.GetMask("Blocks"));
        isTouchingUp = Physics2D.OverlapCircle(upCheck.position, checkRadius, LayerMask.GetMask("Blocks"));



    }
    void dashingEffect()
    {
        AfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;

        if(Mathf.Abs(transform.position.x - lastImageXpos) > 1)
        {
            AfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
