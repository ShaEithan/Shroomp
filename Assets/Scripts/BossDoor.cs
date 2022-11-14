using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public GameObject Eobject;
    private bool touchingDoor = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (touchingDoor && Input.GetKeyUp(KeyCode.E))
            FindObjectOfType<LevelLoader2>().LoadNextLevel();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Eobject.SetActive(true);
            touchingDoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Eobject.SetActive(false);
            touchingDoor = false;
        }
    }
}
