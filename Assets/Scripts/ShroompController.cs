using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using SpriteGlow;
using System.Collections;

public class ShroompController : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpPower = 10;
    public float dashPower = 50;
    public int dashAmmount = 3;
    public float dashRegenTime = 3f;
    public float dashRegenTimeFast = 0.75f;
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
    public AudioClip swoosh;
    public AudioClip Shield;
    //ParticleStuff
    public ParticleSystem impactEffect;
    public ParticleSystem iceEffect,fireEffect;
    public TrailRenderer dashTrail;

    //CameraStuff
    private CinemachineVirtualCamera mainCamera;

    bool isGrounded;
    bool isTouchingLeft, isTouchingRight, isTouchingUp;

    public float checkRadius;
    public Transform groundCheck, leftCheck, rightCheck, upCheck;

    public int dashes;
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

    // Scene Stuff
    Scene currentScene = SceneManager.GetActiveScene();
    string sceneName;

    [SerializeField]
    private int maxHealth = 5;
    public int currentHealth;
    public float timeInvincible = 5f;

    public bool isInvincible = false;
    public float invincibleTimer = 0f;

    //TextMeshProUGUI textHealth;
    //Transform healthCanvasPos;

    //Circle rectile stuff
    Transform rangeCircle,rectile;
    private float rangeOffset,reticleOffset;
    //random test variable remove later
    public float testX = 1f;

    //Wall grab mode stuff
    private bool isWallGrab = false;
    private bool canWallJump;
    private bool lastSideJumped; // True is Left, False is Right, used to remember last side of wall jumped

    //Status stuff
    StatusEffectController statusHandler;

    //bDash power up stuff
    private bool bDashQueue = false;
    private List<float> bDashTime = new List<float>();
    private List<float> bDashTimeDelay = new List<float>();
    private List<float> tempdashSpriteDirection = new List<float>();
    private List<float> tempPosX = new List<float>();
    private List<float> tempPosY = new List<float>();
    private float timeDif;

    //Heart container stuff
    HealthTracker healhTracker;
    //Heart Empty container stuff
    ContainerTracker containerTracker;
    //Feather tracker stuff
    FeatherTracker featherTracker;
    //Blue fairy object script
    public BlueFairyScript blueFairy;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        ResumeGame();
        animator = GameObject.Find("ShroompSprite").GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        ShroompSpriteT = GameObject.Find("ShroompSprite").GetComponent<Transform>();
        //textHealth = GameObject.Find("charHealthText").GetComponent<TMPro.TextMeshProUGUI>();
        //healthCanvasPos = GameObject.Find("CanvasHealth").GetComponent<Transform>();
        rangeCircle = GameObject.Find("RangeCircle").GetComponent<Transform>();
       // rectile = GameObject.Find("Reticle").GetComponent<Transform>();

        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        currentHealth = maxHealth;
        statusHandler = FindObjectOfType<StatusEffectController>();
        healhTracker = FindObjectOfType<HealthTracker>();
        containerTracker = FindObjectOfType<ContainerTracker>();
        featherTracker = FindObjectOfType<FeatherTracker>();
        dashes = dashAmmount;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    int h = 0;
    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (h == 0)
        {
            healhTracker.createContainers();
            containerTracker.createContainers();
            featherTracker.createContainers();
            h++;
        }
        wallGrabCheck();
        if(Time.timeScale >0)
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
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f) )
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
                audioSource.PlayOneShot(swoosh, 0.25F);
                //Debug.Log("Dash END");
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
            //CancelInvoke("increaseDashes"); //cancel dash increase invokes
            //dashes = dashAmmount;
            //Debug.Log("I am grounded");
            animator.SetBool("Jumping", false);
            //animator.SetBool("Dash", false);
            //ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
            CancelInvoke("increaseDashes");
            if(!isFastRunning)
            InvokeRepeating("increaseDashesFast",0, dashRegenTimeFast);

        }

        //A dash will happen as long as you aren't in the process of a wall jump, or are grounded
        //        if (Input.GetMouseButtonDown(0) && dashes > 0 && !isTouchingLeft && !isTouchingRight && !isGrounded)

        if (Input.GetMouseButtonDown(0) && dashes > 0)
        {
            //bDash power up code triggered on end of dash
            if (statusHandler.bDashUp)
            {
                timeDif = dashingTimer;
                //Debug.Log("BDASH TRIGGERED");
                //Temp storage of mouse location for bdash powerup
                tempPosX.Add(mouseDirection.x);
                tempPosY.Add(mouseDirection.y);
                //Calculate angle trijectory angle for ghost image power up
                if (mouseDirection.x * dashPower > 0)
                    tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((mouseDirection.y *dashPower) / (mouseDirection.x * dashPower))) - 90 * -1);
                else if (mouseDirection.x * dashPower < 0)
                    tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((mouseDirection.y * dashPower) / (mouseDirection.x * dashPower))) - 90 * 1);
                else if (mouseDirection.x * dashPower == 0)
                    tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((mouseDirection.y * dashPower) / (mouseDirection.x * dashPower))) - 90);
                //Calculate angle trijectory angle
                /**
                else if (rigidbody2d.velocity.x < 0)
                {
                    tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90 * 1);
                }
                else if (rigidbody2d.velocity.x == 0)
                {
                    tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((-rigidbody2d.velocity.y) / -rigidbody2d.velocity.x)) - 90);
                }
                **/
                bDashTimeDelay.Add(dashTime);
                /**
                if (dashingTimer > 0)
                {
                    bDashTimeDelay[0] =0;
                }
                **/
                bDashQueue = true;
                //Debug.Log("1st Bdash?");
                //Debug.Log("Count tempDashD: " + tempdashSpriteDirection.Count + " Count tempX: " + tempPosX.Count + " Count tempY: " + tempPosY.Count);
            }
            Debug.Log("Dash attempt");
            animator.SetBool("Dash", true);
            rigidbody2d.velocity = new Vector2(mouseDirection.x * dashPower, mouseDirection.y * dashPower);



            dashes = dashes - 1;
            featherTracker.createContainers();
            //Invoke attempt to increase dashes after a certain amount
            Invoke("increaseDashes", dashRegenTime);
            isDashing = true;
            //Set gravit to 0 for dashing
            rigidbody2d.gravityScale = 0;
            dashingTimer = dashTime;
            //Calls Bomb for powerup stuff
            bomb();
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
                //tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90*-1);
            }
            //Calculate angle trijectory angle
            else if (rigidbody2d.velocity.x < 0)
            {
                dashSpriteDirection = (Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90 * -1;
                //tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90 * 1);
            }
            else if (rigidbody2d.velocity.x == 0)
            {
                dashSpriteDirection = (Mathf.Rad2Deg * Mathf.Atan((rigidbody2d.velocity.y) / rigidbody2d.velocity.x)) - 90;
                //tempdashSpriteDirection.Add((Mathf.Rad2Deg * Mathf.Atan((-rigidbody2d.velocity.y) / -rigidbody2d.velocity.x)) - 90);
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
            /**
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
            **/
        }
        //Used countdown invincibility time to avoid taking damage too much in a couple of frames of collision detection
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        //Used to show health
        /**
        textHealth.text = (currentHealth + " / " + maxHealth);
        healthCanvasPos.position = new Vector2(rigidbody2d.position.x, rigidbody2d.position.y + 0.5f);
        **/
        //Range circle and rectile position code
        rangeOffset = ((dashPower * dashTime) * 0.13f);
        //recticleOffset = ((dashPower * dashTime) * (31f / 450f));


        //rangeCircle.transform.localScale = new Vector2(rangeOffset, rangeOffset);
        //rectile.transform.localPosition = new Vector2((mouseDirection.x * dashPower) *dashTime, (mouseDirection.y * dashPower) * dashTime);
        //Code for rotation the circle
        //rangeCircle.transform.Rotate(0, 0, -8 * Time.deltaTime);


        if (isDead)
            deathScene();
        if (statusHandler.bDashUp && bDashQueue)
            bDash();
        checkParticleStatus();

        //trail render stuff
        if (isDashing)
            dashTrail.emitting = true;
        if (!isDashing)
            dashTrail.emitting = false;

        //collider checker for sides
        if(groundCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks")) || groundCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Platforms")))
        isGrounded = true;
        else isGrounded = false;

        if (leftCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks")))
            isTouchingLeft = true;
        else isTouchingLeft = false;

        if (rightCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks")))
            isTouchingRight = true;
        else isTouchingRight = false;
        if (upCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks")))
            isTouchingUp = true;
        else isTouchingUp = false;

        //Wide power up
        if (statusHandler.wideUp)
            transform.localScale = new Vector3(2, 1, 1);
        if (!statusHandler.wideUp)
            transform.localScale = new Vector3(1, 1, 1);

    }
    void FixedUpdate()
    {
        //wallGrabCheck();
        //Can't use movement controls while dashing or grabbing a wall
        //If setting velocity or adding force isn't working it's prob related to this
        if (!isDashing && grabTimer<0 && !isWallGrab)
        {
            rigidbody2d.velocity = new Vector2(direction.x * speed * Time.deltaTime, rigidbody2d.velocity.y);
        }
        /**
         * Old shroomp overlap checker for blocks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, LayerMask.GetMask("Blocks"));
        isTouchingLeft = Physics2D.OverlapCircle(leftCheck.position, checkRadius, LayerMask.GetMask("Blocks"));
        isTouchingRight = Physics2D.OverlapCircle(rightCheck.position, checkRadius, LayerMask.GetMask("Blocks"));
        isTouchingUp = Physics2D.OverlapCircle(upCheck.position, checkRadius, LayerMask.GetMask("Blocks"));
        **/
        /** New checker,doing checks in normal Update() for more consitency with wall grabs, 
         * can be swapped back here if needed
        isGrounded = groundCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks"));
        isTouchingLeft = leftCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks"));
        isTouchingRight = rightCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks"));
        isTouchingUp = upCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Blocks"));
        **/
        /**
        //Debug info for wall grabs left here if needed later
        Debug.Log("WallGrabDEBUG: " + "IsWallGrab Status: " + isWallGrab
            + " Gravity Scale is: " + rigidbody2d.gravityScale + " canWallJump Status: " + canWallJump
            + " Isgrounded: " + isGrounded + " grabTimer: " + grabTimer
            + " Is touching left?: " + isTouchingLeft + " Is touching right?: " + isTouchingRight);
        **/
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
            if(isTouchingLeft && (grabTimer < 0 || !lastSideJumped) && canWallJump && Input.GetKey(KeyCode.A))
            {
                animator.SetBool("Jumping", false);
                rigidbody2d.gravityScale = 0;
                rigidbody2d.velocity = Vector2.zero;
                isWallGrab = true;
                animator.SetTrigger("GrabLeft");

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
            if (isTouchingRight && (grabTimer < 0 || lastSideJumped) && canWallJump && Input.GetKey(KeyCode.D))
            {
                animator.SetBool("Jumping", false);
                rigidbody2d.gravityScale = 0;
                rigidbody2d.velocity = Vector2.zero;
                isWallGrab = true;

                animator.SetTrigger("GrabRight");
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
        //Fix for stuck on wall, resets to normal
        if(isWallGrab && canWallJump && !isTouchingRight && !isTouchingLeft)
        {
            isWallGrab = false;
            canWallJump = false;
            rigidbody2d.gravityScale = 1;
            animator.SetTrigger("LetGo");
        }
        if((isWallGrab && isGrounded))
        {
            isWallGrab = false;
            canWallJump = false;
            rigidbody2d.gravityScale = 1;
            animator.SetTrigger("LetGo");
        }
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            isWallGrab = false;
            canWallJump = false;
            rigidbody2d.gravityScale = 1;
            animator.SetTrigger("LetGo");
        }
        
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (blueFairy.isActiveAndEnabled && blueFairy.shieldStatus)
            {
                audioSource.PlayOneShot(Shield);
                blueFairy.shieldStatus = false;
                return;
            }
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;



        }
 
        animator.SetTrigger("Hurt");
        audioSource.PlayOneShot(hurt);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healhTracker.createContainers();
  
        if (currentHealth ==0)
        {
            isDead = true;
            deathDelayTime = deathDelaySetter;
            //mainCamera.m_Lens.OrthographicSize = 0.75f;
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
            Cursor.visible = true;
            ResumeGame();
            SceneManager.LoadScene("StartScreen");
        }
        deathDelayTime -= Time.unscaledDeltaTime;
        deathTransitionTime -= Time.unscaledDeltaTime;
        deathLoop = 1;
    }

    public GameObject bombPrefab;
    public Material bDashMaterial;
 
    private void bomb()
    {
        if(statusHandler.bombUp)
        Instantiate(bombPrefab).GetComponent<Transform>().position = new Vector2(rigidbody2d.position.x, rigidbody2d.position.y);
    }
    int token = 0;
    private List<GameObject >tempHolder = new List<GameObject>();
    public Sprite dashSprite;
    private void bDash()
    {

        if (bDashTimeDelay.Count > 0)
        {
            for (var i = 0; i < bDashTimeDelay.Count; i++)
            {
                if (bDashTimeDelay[i] > 0)
                {
                    //Debug.Log(bDelay + "what is this");
                    bDashTimeDelay[i] -= Time.deltaTime;

                }
                if (bDashTimeDelay.Count > 1)
                {
                    //Debug.Log("Delay Set to 0");
                    bDashTimeDelay[0] = -1;
                }
                if (bDashTimeDelay[i] <= 0)
                {
                    //Debug.Log("Token Added");
                    bDashTimeDelay.RemoveAt(0);
                    token++;
                }
            }
        }


        if (token > 0)
        {
            //Create new object
            GameObject bDashObject = new GameObject();
            bDashObject.transform.position = transform.position;
            //Assign components
            //bDashObject.SetActive(false);
            bDashObject.AddComponent(typeof(SpriteRenderer));
            bDashObject.AddComponent(typeof(Rigidbody2D));
            bDashObject.AddComponent(typeof(BoxCollider2D));
            bDashObject.AddComponent<SpriteGlowEffect>();
            bDashObject.tag = "PlayerShadow";
            bDashObject.layer = 7;
            //Assign correct components from original shroomp
            
            bDashObject.GetComponent<BoxCollider2D>().size = transform.gameObject.GetComponent<BoxCollider2D>().size;
            bDashObject.GetComponent<BoxCollider2D>().isTrigger = true;
            bDashObject.transform.GetComponent<Rigidbody2D>().gravityScale = 0;
            bDashObject.gameObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, tempdashSpriteDirection[0]);
            tempdashSpriteDirection.RemoveAt(0);

            bDashObject.GetComponent<SpriteRenderer>().sprite = dashSprite;
            bDashObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            bDashObject.GetComponent<SpriteGlowEffect>().DrawOutside = true;
            bDashObject.GetComponent<SpriteGlowEffect>().GlowColor = new Color(0.1647059f, 0f, 1f, 1f);
            bDashObject.GetComponent<SpriteRenderer>().enabled = false;
            //bDashObject.SetActive(true);
            bDashObject.transform.GetComponent<Rigidbody2D>().position = transform.position;
            
            bDashObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-tempPosX[0] * dashPower, -tempPosY[0] * dashPower);
            bDashObject.GetComponent<SpriteRenderer>().enabled = true;
            tempPosX.RemoveAt(0);
            tempPosY.RemoveAt(0);
            //bDashTime = dashTime;
            if (dashingTimer < dashTime)
            {
                //prob never gets called
                bDashTime.Add(dashTime - dashingTimer);
                //Debug.Log("Dash0Trigger");
            }
            else if (dashingTimer > 0)
            {
                bDashTime.Add(dashTime - timeDif);
                //Debug.Log("Dash>0Trigger");
            }
            //Debug.Log("bDashTime timer added with value: " + (dashTime));
            tempHolder.Add(bDashObject);
            token--;
            //Debug.Log("Token Removed");
        }
        if (bDashTime.Count > 0)
        {
            for (var i = 0; i < bDashTime.Count; i++)
            {
                //Debug.Log(bDashTime[i]);
                if (bDashTime[i] > 0)
                {
                    bDashTime[i] -= Time.deltaTime;

                }
                if (bDashTime[i] == 0 || bDashTime[i] < 0)
                {
                    bDashTime.RemoveAt(i);
                    //Debug.Log("Destroy attempt");
                    GameObject.Destroy(tempHolder[i]);
                    tempHolder.RemoveAt(i);
                }
            }
        }
        //Debug.Log("Bdash delay queue: " + bDashTimeDelay.Count + " Bdashtime queue: " + bDashTime.Count);
        //Debug.Log(Mathf.Clamp(dashingTimer, 0, dashTime) + " Dashing timer");
        /**
        foreach (var item in bDashTime)
        {
            Debug.Log("Items in bdashtime: " + item);
        }
        **/
    }
    private void checkParticleStatus()
    {
        //Turn on section
        if (statusHandler.fireUp)
            fireEffect.Play();
        if (statusHandler.iceUp)
            iceEffect.Play();
        //Turn off section
        if (!statusHandler.fireUp)
        {
            fireEffect.Clear();
            fireEffect.Pause();
        }
        if (!statusHandler.iceUp)
        {
            iceEffect.Clear();
            iceEffect.Pause();
        }
    }
    public GameObject slashObject;
    public AudioClip slashSound;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && isDashing)
        {
            Random.seed = System.DateTime.Now.Millisecond;

            var slash = Instantiate(slashObject);
            slash.SetActive(true);
            //slash.transform.position = collision.transform.position;
            slash.transform.position = collision.ClosestPoint(transform.position);


            var slashAnimC = slash.GetComponent<Animator>();
            audioSource.PlayOneShot(slashSound);
            var rand = Random.Range(1, 5);
            if (rand == 1)
            {
                slash.transform.rotation = Quaternion.Euler(0, 0, dashSpriteDirection+45);
                slashAnimC.SetTrigger("Slash1");
            }
            if (rand == 2)
            {
                slash.transform.rotation = Quaternion.Euler(0, 0, dashSpriteDirection + 45);
                slashAnimC.SetTrigger("Slash2");
            }
            if (rand == 3)
            {
                slash.transform.rotation = Quaternion.Euler(0, 0, dashSpriteDirection+90);
                slashAnimC.SetTrigger("Slash3");
            }
            if (rand == 4)
            {
                slash.transform.rotation = Quaternion.Euler(0, 0, dashSpriteDirection+90);
                slashAnimC.SetTrigger("Slash4");
            }
        }

    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
    }
    void increaseDashes()
    {
        Debug.Log("Slow dash gained!");
        if (dashes < dashAmmount)
        {
            dashes++;
            featherTracker.createContainers();
        }
        if(dashes == dashAmmount)
        {
            isFastRunning = false;
            CancelInvoke("increaseDashes");
        }
    }
    private bool isFastRunning = false;
    void increaseDashesFast()
    {
        Debug.Log("Faster dash gained!");
        isFastRunning = true;
        if (dashes < dashAmmount)
        {
            dashes++;
            featherTracker.createContainers();
        }
        if (dashes == dashAmmount)
        {
            isFastRunning =false;
            CancelInvoke("increaseDashesFast");
        }
    }
    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag + " ISSS THEEE TAGGG");
        if (other.CompareTag("Rain") && !isDashing)
            ChangeHealth(-1);

    }
}
