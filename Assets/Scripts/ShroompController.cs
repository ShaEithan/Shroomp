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


    bool isGrounded;
    public float checkRadius;
    public Transform groundCheck;
    int dashes;
    bool isDashing;
    public float dashingTimer;
    float jumpDelay;
    Animator animator;
    float dashSpriteDirection;
    Transform ShroompSpriteT;
    Vector2 direction;
    Vector2 lookDirection = new Vector2(1, 0);

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
            rigidbody2d.velocity = new Vector2(direction.x * dashPower, direction.y * dashPower);
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
        }
    }
    void FixedUpdate()
    {
        if (!isDashing)
        {
            rigidbody2d.velocity = new Vector2(direction.x * speed * Time.deltaTime, rigidbody2d.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, LayerMask.GetMask("Blocks"));


        
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
