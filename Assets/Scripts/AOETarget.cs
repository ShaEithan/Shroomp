using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AOETarget : MonoBehaviour
{
    public GameObject target;
    public string targetTag;
    public bool hasTarget = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            hasTarget = true;
            if (Vector3.Distance(transform.position, target.transform.position) > 5)
            {
                target = null;
                hasTarget = false;
            }
        }
        if(target == null)
        {
            target = null;
            hasTarget = false;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag(targetTag))
        {
            if (target == null)
                target = collision.gameObject;
            else if (Vector3.Distance(transform.position, collision.transform.position) < Vector3.Distance(transform.position, target.transform.position))
            {
                target = collision.gameObject;
            }
                
        }
    }
    public Vector3 nearestTarget()
    {
        return target.transform.position;
    }
}
