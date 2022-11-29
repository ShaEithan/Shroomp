using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayIntro : MonoBehaviour
{
    public VideoPlayer player;
    void Start()
    {
        player.loopPointReached += CheckOver;
    }
    private void Update()
    {
    }
    void CheckOver(UnityEngine.Video.VideoPlayer player)
    {
        SceneManager.LoadScene("StartScreen");
    }
}
