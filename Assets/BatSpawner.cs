using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public GameObject bat;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", 15f);
    }

    void Spawn()
    {
        Instantiate(bat,transform.position, Quaternion.identity);
    }
}
