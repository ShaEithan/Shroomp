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
    void Start()
    {
        mustPatrol = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }
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

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        ShroompController player = collision.gameObject.GetComponent<ShroompController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }


}
