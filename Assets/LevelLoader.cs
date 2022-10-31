using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update

    public int iLevelToLoad;
    public string sLevelToLoad;

    public bool intToLoadLevel = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.name == "Shroomp")
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (intToLoadLevel) // if true we load this level
        {
            SceneManager.LoadScene(iLevelToLoad);
        }
        else // if not then we go here 
        {
            SceneManager.LoadScene(sLevelToLoad);
        }
    }
}
