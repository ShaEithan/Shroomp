using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class BossStarterEnder : MonoBehaviour
{
    public List<GameObject> gameObjectstoDisable;
    public List<GameObject> gameObjectstoEnable;
    public GameObject boss;
    public AudioClip deathClip;
    private int x = 0;
    private bool hasRan = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null && !hasRan && x ==1)
        {
            InvokeRepeating("endSounds", 0, 1);
            Invoke("toEndScreen", 5f);
            hasRan = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (x == 0)
            {
                foreach (GameObject go in gameObjectstoDisable)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in gameObjectstoEnable)
                {
                    go.SetActive(true);
                }
                boss.GetComponent<PlayableDirector>().Play();
                x++;
            }
        }
    }
    void endSounds()
    {
        GetComponent<AudioSource>().PlayOneShot(deathClip);
    }
    void toEndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }
}
