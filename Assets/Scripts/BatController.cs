using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public float speed;
    public int maxHealth = 5;
    int currentHealth;
    public float detectRadius = 2f;
    bool isDetecting;

    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        isDetecting = Physics2D.OverlapCircle(rigidbody2D.position, detectRadius, LayerMask.GetMask("Shroomp"));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
