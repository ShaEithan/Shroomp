using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AOETarget : MonoBehaviour
{
    [SerializeField]
    private GameObject target =null;
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
            if (Vector3.Distance(transform.position, target.transform.position) > GetComponent<CircleCollider2D>().radius)
            {
                target = null;
                hasTarget = false;
            }
        }
        if(target == null)
        {
            //target = null;
            hasTarget = false;
        }
        GetComponent<AOETarget>().enabled = true;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        
        if(collision.CompareTag(targetTag))
        {
            
            if (!hasTarget)
            {
                Debug.Log("COLLISION WITH SPECIAL" + collision + " tag is " + collision.tag + " name " + collision.name);
                target = collision.gameObject;
                Debug.Log("SPECIAL TARGET IS NOW: " + target);
                hasTarget = true;
            }
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
