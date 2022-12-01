using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallRespawn : MonoBehaviour
{
    public Transform spawnPoint; // make a empty gameobject and assign it
    // to the script in the inspector
    public GameObject player;
    public float minHeightForDeath;

    Scene currentScene = SceneManager.GetActiveScene();
    string sceneName;
    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (player == null)
            player = GameObject.Find("Shroomp");

        if (sceneName == "Level2")
        {
            if (player.transform.position.y < minHeightForDeath)
            {
                player.transform.position = spawnPoint.position;
            }
        }
    }
}
