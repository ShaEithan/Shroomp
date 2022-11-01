using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private float timeUntilExplode;
    public float timeUntilExplodeSetter = 4f;
    public float timeUntilDestroySetter;
    public bool startParticles = false;
    private float timeUntilDestroy;
    private bool hasExploded = false;
    Animator animator;
    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        boxCollider = transform.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilDestroy = timeUntilDestroySetter;

        if (timeUntilDestroy < 0)
        {
            Debug.Log("Bomb Destroyed");
            Destroy(transform.gameObject);
        }
        if (startParticles && !particleHasPlayed)
            playParticle();
    }
    private void Awake()
    {
        timeUntilExplode = timeUntilExplodeSetter;
        hasExploded = false;
        timeUntilDestroy = 0;
    }
    private bool particleHasPlayed = false;
    public void playParticle()
    {
        transform.GetChild(0).GetComponent<ParticleSystem>().Clear();
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        particleHasPlayed = true;
    }

}
