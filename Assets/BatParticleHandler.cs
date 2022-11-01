using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatParticleHandler : MonoBehaviour
{
    private BatController parentBat;
    // Start is called before the first frame update
    void Start()
    {
        parentBat = GetComponentInParent<BatController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collision detected with: " + other);
        if (other.CompareTag("Bomb"))
        {
            parentBat.ChangeHealth(-1);
        }
    }
}
