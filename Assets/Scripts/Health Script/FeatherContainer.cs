using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FeatherContainer : MonoBehaviour
{
    public float count;
    public Sprite feather;
    // Start is called before the first frame update
    void Start()
    {
        //transform.GetComponent<UnityEngine.UI.Image>().sprite = feather;
        if (count == 1)
            transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        if (count == 2)
            transform.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
