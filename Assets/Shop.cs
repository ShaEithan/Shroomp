using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject eKey, shopItem;
    public int price = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            eKey.SetActive(true);
            if(Input.GetKey(KeyCode.E) && FindObjectOfType<ScoreScript>().score >=price)
            {
                FindObjectOfType<ScoreScript>().score -= price;
                GameObject item = Instantiate(shopItem);
                item.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eKey.SetActive(false);
        }
    }
}
