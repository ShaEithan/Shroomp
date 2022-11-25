using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform shotPosition;
    public GameObject projectile;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Instantiate(projectile, shotPosition.position, shotPosition.rotation);
        }
    }
}
