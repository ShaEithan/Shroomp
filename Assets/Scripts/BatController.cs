using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public float speed;
    public int maxHealth = 5;
    int currentHealth;
    public float detectRadius = 2f;
    public float movePower = 7f;
    bool isDetecting, alerted;

    Vector2 charPos,batPos,moveDirection;
    private Transform mainChar;

    public float maxVelocity = 6;

    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        mainChar = GameObject.Find("ShroompSprite").transform;
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
        moveDirection.Normalize();
    }
    private void FixedUpdate()
    {
        isDetecting = Physics2D.OverlapCircle(rigidbody2D.position, detectRadius, LayerMask.GetMask("Shroomp"));
        if (isDetecting) alerted = true;
        if (alerted)
        {
            rigidbody2D.AddForce(new Vector2(-1 * movePower * moveDirection.x , -1 * movePower * moveDirection.y ));
            if(rigidbody2D.velocity.magnitude > maxVelocity)
            {
                rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxVelocity;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        ShroompController player = collision.gameObject.GetComponent<ShroompController>();

        if(player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
