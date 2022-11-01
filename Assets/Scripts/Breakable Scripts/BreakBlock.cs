using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    public bool destroy;
    public bool startParticles = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
            destroyThis();

        if (startParticles && !particleHasPlayed)
            playParticle();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<ShroompController>().isDashing)
            {
                transform.GetComponent<Animator>().SetTrigger("Break");
            }
        }
        
    }
    void destroyThis()
    {
        Destroy(transform.gameObject);
    }
    private bool particleHasPlayed = false;
    public void playParticle()
    {
        transform.GetComponent<ParticleSystem>().Clear();
        transform.GetComponent<ParticleSystem>().Play();
        particleHasPlayed = true;
    }
}
