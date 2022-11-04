using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CoinScript : MonoBehaviour
{
    public AudioClip coinSound;
    private AudioSource audioSource;
    public int coinValue = 1;
    private float delayT = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private bool soundState = false;
    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
            soundState = true;
        if(soundState && !audioSource.isPlaying)
            Destroy(gameObject);
        delayT -= Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && delayT <0)
        {
            FindObjectOfType<ScoreScript>().score += coinValue;
            audioSource.PlayOneShot(coinSound);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Light2D>().enabled = false;

        }
    }
}
