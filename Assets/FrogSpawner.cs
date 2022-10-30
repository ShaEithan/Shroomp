using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    public GameObject frog;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", 15f);
    }

    void Spawn()
    {
        Instantiate(frog,transform.position, Quaternion.identity);
    }
}
