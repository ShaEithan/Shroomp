using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class ShroompController : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpPower = 10;
    public float dashPower = 50;
    public int dashAmmount = 3;
    //Used to calculate the jump that happens against the wall
    public float wallGrabJumpPower = 4f;
    //Used to set time for wall jumps, too low and it gets you stuck on the wall
    public float wallGrabTimeSetter = 1f;
    //Ammount of air time before wall jump ends and collision check can occur again
    private float grabTimer = -1f; 
    //int isJumping = 0;
    Rigidbody2D rigidbody2d;
    
    //Audio Stuff
    AudioSource audioSource;
    public AudioClip hurt;
    public AudioClip dead;
    public ParticleSystem impactEffect;

    //CameraStuff
    private CinemachineVirtualCamera mainCamera;

    bool isGrounded;
    bool isTouchingLeft, isTouchingRight, isTouchingUp;

    public float checkRadius;
    public Transform groundCheck, leftCheck, rightCheck, upCheck;

    int dashes;
    public bool isDashing;
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
    private int currentHealth;
    private float timeInvincible = 2.0f;

    bool isInvincible;
    float invincibleTimer;

    TextMeshProUGUI textHealth;
    Transform healthCanvasPos;

    //Circle rectile stuff
    Transform rangeCircle,rectile;
    private float rangeOffset,reticleOffset;
    //random test variable remove later
    public float testX = 1f;

    //Wall grab mode stuff
    private bool isWallGrab = false;
    private bool canWallJump;
    private bool lastSideJumped; // True is Left, False is Right, used to remember last side of wall jumped
    // Start is called before the first frame update
    void Start()
    {
        ResumeGame();
        animator = GameObject.Find("ShroompSprite").GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        ShroompSpriteT = GameObject.Find("ShroompSprite").GetComponent<Transform>();
        textHealth = GameObject.Find("charHealthText").GetComponent<TMPro.TextMeshProUGUI>();
        healthCanvasPos = GameObject.Find("CanvasHealth").GetComponent<Transform>();
        rangeCircle = GameObject.Find("RangeCircle").GetComponent<Transform>();
        rectile = GameObject.Find("Reticle").GetComponent<Transform>();

        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        wallGrabCheck();
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
        if (Time.timeScale == 1)
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
        //delay after jumping before another ground jump can occur, used to avoid jump going crazy
        if (jumpDelay > 0)
        {
            jumpDelay -= Time.deltaTime;

        }
        else if (isGrounded == true)
        {
            dashes = dashAmmount;
            Debug.Log("I am grounded");
            animator.SetBool("Jumping", false);
            animator.SetBool("Dash", false);
            ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);

        }

        //A dash will happen as long as you aren't in the process of a wall jump, or are grounded
        if (Input.GetKeyDown("space") && dashes > 0 && !isTouchingLeft && !isTouchingRight && !isGrounded)
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
        //A jump can happen as long as you are in the ground, and aren't grabbing a wall
        else if (Input.GetKeyDown("space") && isGrounded == true && !isWallGrab)
        {
            Debug.Log("Normal jump attempt");
            animator.SetBool("Jumping", true);
            rigidbody2d.velocity = Vector2.up * jumpPower;
            //dashes = dashAmmount;
            jumpDelay = .05f;
        }
        //Trajectory code when dashing for rotation of sprite using principles of unit circle to calculate angle
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
                //rigidbody2d.velocity = impactVelocity;
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = leftCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(0, 90, 90);
                impactEffect.Play();

                //Re adds gravity scale when dash ends by impact
                //rigidbody2d.gravityScale = 1;
            }
            else if (isTouchingRight)
            {
                //rigidbody2d.velocity = impactVelocity;
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = rightCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(0, -90, -90);
                impactEffect.Play();

                //Re adds gravity scale when dash ends by impact
                //rigidbody2d.gravityScale = 1;
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
        //Used countdown invincibility time to avoid taking damage too much in a couple of frames of collision detection
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        //Used to show health
        textHealth.text = (currentHealth + " / " + maxHealth);
        healthCanvasPos.position = new Vector2(rigidbody2d.position.x, rigidbody2d.position.y + 0.5f);

        //Range circle and rectile position code
        rangeOffset = ((dashPower * dashTime) * 0.13f);
        //recticleOffset = ((dashPower * dashTime) * (31f / 450f));


        rangeCircle.transform.localScale = new Vector2(rangeOffset, rangeOffset);
        rectile.transform.localPosition = new Vector2((mouseDirection.x * dashPower) *dashTime, (mouseDirection.y * dashPower) * dashTime);
        //Code for rotation the circle
        rangeCircle.transform.Rotate(0, 0, -8 * Time.deltaTime);


        if (isDead)
            deathScene();
    }
    void FixedUpdate()
    {
        //Can't use movement controls while dashing or grabbing a wall
        //If setting velocity or adding force isn't working it's prob related to this
        if (!isDashing && grabTimer<0 && !isWallGrab)
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
    void wallGrabCheck()
    {
        if (grabTimer > 0)
            grabTimer -= Time.deltaTime;

        if (grabTimer < 0)
            canWallJump = true;
        //Debug.Log("Global" + grabTimer);
        if (!isGrounded && grabTimer < 0)
        {
            // Left Side code wall jumping
            // Checks you are touching a wall, uses grabTimer to avoid multiple checks on side of wall
            // That end up getting you stuck, similar premise for normal jump delay
            // !lastSideJumped bypasses grabTimer as it means you came from opposite side wall so its ok to proceed
            // With no collide issues
            // Could probably be simplified as canWalljump = grabTimer <0 
            if(isTouchingLeft && (grabTimer < 0 || !lastSideJumped) && canWallJump )
            {
                animator.SetBool("Jumping", false);
                rigidbody2d.gravityScale = 0;
                rigidbody2d.velocity = Vector2.zero;
                isWallGrab = true;


            }
            
            if (isTouchingLeft && Input.GetKeyDown("space") && isWallGrab && grabTimer < 0)
            {
                rigidbody2d.gravityScale = 1;
                rigidbody2d.AddForce(new Vector2(wallGrabJumpPower, wallGrabJumpPower * 0.6f));
                isWallGrab = false;
                canWallJump = false;
                grabTimer = wallGrabTimeSetter;
                lastSideJumped = true;

                //Optional dust on bounce off
                animator.SetBool("Jumping", true);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = leftCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(0, 90, 90);
                impactEffect.Play();
            }
            //Right side wall jumping code
            if (isTouchingRight && (grabTimer < 0 || lastSideJumped) && canWallJump)
            {
                animator.SetBool("Jumping", false);
                rigidbody2d.gravityScale = 0;
                rigidbody2d.velocity = Vector2.zero;
                isWallGrab = true;


            }

            if (isTouchingRight && Input.GetKeyDown("space") && isWallGrab && grabTimer < 0)
            {
                rigidbody2d.gravityScale = 1;
                rigidbody2d.AddForce(new Vector2(-1*wallGrabJumpPower, wallGrabJumpPower * 0.6f));
                isWallGrab = false;
                canWallJump = false;
                grabTimer = wallGrabTimeSetter;
                lastSideJumped = false;

                //Optional dust bounce off
                animator.SetBool("Jumping", true);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = rightCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(0, -90, -90);
                impactEffect.Play();
            }
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
        animator.SetTrigger("Hurt");
        audioSource.PlayOneShot(hurt);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth ==0)
        {
            isDead = true;
            deathDelayTime = deathDelaySetter;
            mainCamera.m_Lens.OrthographicSize = 0.75f;
        }
        
    }
    public float deathDelaySetter = 3f;
    private float deathDelayTime;
    private bool isDead = false;
    private int deathLoop = 0;
    private bool deathplayed = false;
    private float deathTransitionTime;
    void deathScene()
    {
        if (deathLoop == 0)
        {
            //GameObject.Find("DeathBackground").GetComponent<Transform>().gameObject.SetActive(true);
           PauseGame();
            animator.speed = 0;
        }
        if(deathDelayTime<0 && !deathplayed)
        {
            animator.speed = 1;
            animator.SetTrigger("Dead");
            audioSource.PlayOneShot(dead);
            deathplayed = true;
            deathTransitionTime = 3f;
        }
        if(deathTransitionTime <0 && deathplayed)
        {
            SceneManager.LoadScene("StartScreen");
        }
        deathDelayTime -= Time.unscaledDeltaTime;
        deathTransitionTime -= Time.unscaledDeltaTime;
        deathLoop = 1;
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
