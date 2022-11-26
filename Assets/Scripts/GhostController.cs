using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject checker;
    private Animator animator;
    public float velocity;
    public float attackDelay = 3f;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("LookX",Mathf.Round(GetComponent<AIPath>().desiredVelocity.x));
        attackDelay -= Time.deltaTime;

    }
    private void FixedUpdate()
    {
        bool detected = checker.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Shroomp"));
        if (detected && attackDelay<0)
        {
            int x = Random.Range(1, 3);
            if (x == 1)
                animator.SetTrigger("Hand");
            if (x == 2)
                animator.SetTrigger("Finger");
            if (x == 3)
                animator.SetTrigger("Punch");
            attackDelay = 3f;
        }
    }
}
