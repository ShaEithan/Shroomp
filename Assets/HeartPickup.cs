using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HeartPickup : MonoBehaviour
{
    private float delayT = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
    }
    private bool State = false;
    // Update is called once per frame
    void Update()
    {

        if (State)
            Destroy(gameObject);
        delayT -= Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && delayT < 0)
        {
            if (FindObjectOfType<ShroompController>().currentHealth < FindObjectOfType<ShroompController>().maxHealth)
            {
                FindObjectOfType<ShroompController>().currentHealth += 1;
                FindObjectOfType<HealthTracker>().createContainers();
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
