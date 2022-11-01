using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreScript : MonoBehaviour
{
    TextMeshProUGUI text;
    public int score=0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ("Score: " + score);
    }
}
