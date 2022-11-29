using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Boss1Timer : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;
    private AudioSource AudioSource;
    public float minutes;
    public float seconds;
    public CinemachineVirtualCamera cm;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        AudioSource = GetComponent<AudioSource>();
        
    }

    private bool endTimer = false;
    void Update()
    {
        if (cm.Follow != null)
        {
            cm.Follow = FindObjectOfType<ShroompController>().transform;
            cm.m_Lens.OrthographicSize = 7;
        }
        if (!endTimer)
        {
            seconds -= Time.deltaTime;
            if (seconds <= 0)
            {
                seconds = 59;
                minutes--;
            }

            if(Mathf.RoundToInt(seconds) >=10)
                text.text = "Survive! " + Mathf.RoundToInt(minutes) + ":" + Mathf.RoundToInt(seconds);
            if(Mathf.RoundToInt(seconds) <= 9)
                text.text = "Survive! " + Mathf.RoundToInt(minutes) + ":" +"0" + Mathf.RoundToInt(seconds);
            if (minutes < 0)
            {
                endTimer = true;
                text.text = "Survive! " + "0" + ":" + "00";
            }
            
        }
        Debug.Log("TIMESCALE IS " + Time.timeScale);
        if (Time.timeScale == 0)
            AudioSource.Pause();
        if (Time.timeScale == 1 && !AudioSource.isPlaying && !endTimer)
            AudioSource.Play();
    }
}
