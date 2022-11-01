using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurnBlock : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<ShroompController>().isDashing && FindObjectOfType<StatusEffectController>().fireUp)
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
