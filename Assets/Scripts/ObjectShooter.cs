using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectShooter : MonoBehaviour
{
    public GameObject projectile;
    public float rateOfFire;
    private float timer =0f;
    public float speedOfFire;
    private AOETarget aoeTarget;

    // Start is called before the first frame update
    void Start()
    {
        aoeTarget = GetComponent<AOETarget>();
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (aoeTarget.hasTarget && timer >= rateOfFire)
        {
            fireProjectile();
            timer = 0f;
        }
    }
    private void fireProjectile()
    {
        /**
        if(!aoeTarget.hasTarget)
        {
            return;
        }    
        **/
        GameObject p = Instantiate(projectile);
        p.gameObject.transform.position = transform.position;
        p.GetComponent<Rigidbody2D>().AddForce((Vector3.Normalize(aoeTarget.nearestTarget() - transform.position)) * speedOfFire);
        Debug.Log("Normalized projectile angle: " + (Vector3.Normalize(aoeTarget.nearestTarget() - transform.position)) + " towards position" + aoeTarget.nearestTarget());
    }
}
