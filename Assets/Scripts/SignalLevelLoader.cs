using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalLevelLoader : MonoBehaviour
{
    public int forceLevelNumber = 0;
    public bool forceStatus = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    bool isLoading = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.name == "Shroomp" && !isLoading)
        {
            if (!forceStatus)
            {
                isLoading = true;
                FindObjectOfType<LevelLoader2>().LoadNextLevel();
            }
            if(forceStatus)
            {
                isLoading = true;
                FindObjectOfType<LevelLoader2>().LoadNextLevel(forceLevelNumber);
            }
        }
    }
}
