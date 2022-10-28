using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretScript : MonoBehaviour
{
    // Start is called before the first frame update\

    public float Range;
    public Transform Target;
    bool Detected = false;

    Vector2 Direction;

    public GameObject AlarmLight;
    public GameObject Bullet;
    public float FireRate;
    float nextShot = 0;
    public Transform ShootPoint;
    public float Force;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 TargetPos = Target.position;
        Direction = TargetPos - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);


        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.tag == "Player")
            {
                if(Detected == false)
                {
                    Detected = true;
                    AlarmLight.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
        else
        {
            if(Detected == true)
            {
                Detected = false;
                AlarmLight.GetComponent<SpriteRenderer>().color = Color.green;

            }
        }
        if (Detected)
        {
            if(Time.time > nextShot)
            {
                nextShot = Time.time + 1 / FireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * Force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
