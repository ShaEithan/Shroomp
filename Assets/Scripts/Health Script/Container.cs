using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Container : MonoBehaviour
{
    public float count;
    public Sprite fullHeart, halfHeart;
    // Start is called before the first frame update
    void Start()
    {
        if(count == 1)
            transform.GetComponent<UnityEngine.UI.Image>().sprite = halfHeart;
        if (count == 2)
            transform.GetComponent<UnityEngine.UI.Image>().sprite = fullHeart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
