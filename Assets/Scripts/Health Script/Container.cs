using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public float count;
    public Sprite fullHeart, halfHeart;
    // Start is called before the first frame update
    void Start()
    {
        if(count == 1)
            transform.GetComponent<SpriteRenderer>().sprite = halfHeart;
        if (count == 2)
            transform.GetComponent<SpriteRenderer>().sprite = fullHeart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
