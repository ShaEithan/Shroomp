using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalLevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
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
            FindObjectOfType<LevelLoader2>().LoadNextLevel();
        }
    }
}
