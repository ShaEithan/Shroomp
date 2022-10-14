using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneOnTimer : MonoBehaviour
{
    public float changeTime;

    // Update is called once per frame
    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
            SceneManager.LoadScene("Level1");
    }
}
