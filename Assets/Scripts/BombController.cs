using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private float timeUntilExplode;
    public float timeUntilExplodeSetter = 4f;
    public float timeUntilDestroySetter =2f;
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
        timeUntilExplode -= Time.deltaTime;
        timeUntilDestroy -= Time.deltaTime;
        if (timeUntilExplode < 0 && !hasExploded)
        {
            Debug.Log("Time until explode is " + timeUntilExplode);
            animator.SetTrigger("Explode");
            boxCollider.enabled = true;
            timeUntilDestroy = timeUntilDestroySetter;
            hasExploded = true;
        }
        if (timeUntilDestroy < 0 && hasExploded)
        {
            Debug.Log("Bomb Destroyed");
            Destroy(transform.gameObject);
        }
    }
    private void Awake()
    {
        timeUntilExplode = timeUntilExplodeSetter;
        hasExploded = false;
        timeUntilDestroy = 0;
    }

}
