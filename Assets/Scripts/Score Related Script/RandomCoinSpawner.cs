using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCoinSpawner : MonoBehaviour
{
    public GameObject bronze, gold, diamond;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnRandom(Transform input)
    {
        var rand =Random.Range(0, 100);
        if (rand >= 0 && rand <= 70)
        {
            Instantiate(bronze).transform.position = input.position;
        }
        if (rand > 70 && rand <= 90)
        {
            Instantiate(gold).transform.position = input.position;
        }
        if (rand > 90 && rand <= 100)
        {
            Instantiate(diamond).transform.position = input.position;
        }
    }
}
