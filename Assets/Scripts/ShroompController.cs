using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroompController : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpPower = 10;
    public float dashPower = 50;
    //int isJumping = 0;
    Rigidbody2D rigidbody2d;

    public ParticleSystem impactEffect;

    bool isGrounded;
    bool isTouchingLeft, isTouchingRight, isTouchingUp;

    public float checkRadius;
    public Transform groundCheck, leftCheck, rightCheck, upCheck;

    int dashes;
    bool isDashing;
    public float dashingTimer;
    float jumpDelay;
    Animator animator;
    float dashSpriteDirection;
    Transform ShroompSpriteT;
    Vector2 direction;
    Vector2 mouseDirection;
    Vector2 lookDirection = new Vector2(1, 0);
    Vector2 charOrigin;

    private float lastImageXpos;


    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("ShroompSprite").GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        ShroompSpriteT = GameObject.Find("ShroompSprite").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Sets char origin
        charOrigin = new Vector2(Camera.main.WorldToScreenPoint(rigidbody2d.position).x, Camera.main.WorldToScreenPoint(rigidbody2d.position).y);
         //Assigns x and y values based on char position
        if(Input.mousePosition.x > charOrigin.x)
        {
            mouseDirection.x = Input.mousePosition.x - charOrigin.x;
        }
        if(Input.mousePosition.x < charOrigin.x)
        {
            mouseDirection.x = -1 * (charOrigin.x - Input.mousePosition.x);
        }
        if(Input.mousePosition.y > charOrigin.y)
        {
            mouseDirection.y = Input.mousePosition.y - charOrigin.y;
        }
        if (Input.mousePosition.y < charOrigin.y)
        {
            mouseDirection.y = -1 *(charOrigin.y - Input.mousePosition.y);
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
       if(isDashing)
        {
            dashingTimer -= Time.deltaTime;
            
            if (dashingTimer <0)
            {
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
            }
            dashingEffect();
        }
       if(jumpDelay>0)
        {
            jumpDelay -= Time.deltaTime;
            
        }
       else if(isGrounded == true)
        {
            dashes = 0;
            Debug.Log("I am grounded");
            animator.SetBool("Jumping", false);
            animator.SetBool("Dash", false);
            ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);

        }

       if(Input.GetKeyDown("space") && dashes > 0)
        {
            Debug.Log("Dash attempt");
            animator.SetBool("Dash", true);
            rigidbody2d.velocity = new Vector2(mouseDirection.x * dashPower, mouseDirection.y * dashPower);
            dashes = 0;
            isDashing = true;
            dashingTimer = 1f;
        }
       
       else if(Input.GetKeyDown("space") && isGrounded ==true)
        {
            Debug.Log("Normal jump attempt");
            animator.SetBool("Jumping", true);
            rigidbody2d.velocity = Vector2.up * jumpPower;
            dashes = 1;
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
            if(isTouchingUp)
            {
                rigidbody2d.velocity = impactVelocity;
                isDashing = false;
                animator.SetBool("Dash", false);
                animator.SetBool("Jumping", false);
                ShroompSpriteT.rotation = Quaternion.Euler(0, 0, 0);
                impactEffect.transform.position = upCheck.transform.position;
                impactEffect.transform.rotation = Quaternion.Euler(90, 90, 0);
                impactEffect.Play();
            }
            else if(isTouchingLeft)
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
            }
            else if(isTouchingRight)
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
            }
            else if(isGrounded)
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
            }
        }
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
}
