using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxDialog : MonoBehaviour
{
    public string inputText;
    public float textSpeed;
    public GameObject Eobject;
    public GameObject TextBox;
    public GameObject tmpObject;
    public AudioClip soundClip;
    public float min, max;
    private AudioSource audioSource;
    private TMPro.TextMeshProUGUI outputText;
    private bool startText = false;
    private float timer = 0f;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        outputText = tmpObject.GetComponent<TMPro.TextMeshProUGUI>();
        audioSource = Eobject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Eobject.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Shroomp")))
        {
            if(!startText)
            Eobject.GetComponent<SpriteRenderer>().enabled = true;
            if (Input.GetKeyUp(KeyCode.E))
            {
                Eobject.GetComponent<SpriteRenderer>().enabled = false;
                TextBox.SetActive(true);
                startText = true;
            }
        }
        if (!Eobject.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Shroomp")))
        {
            TextBox.SetActive(false);
            Eobject.GetComponent<SpriteRenderer>().enabled = false;
            timer = 0f;
            startText = false;
            i = 0;
            outputText.text = "";
        }

        if(startText)
        {
            timer += Time.deltaTime;
            if(timer > textSpeed/10)
            {
                timer = 0f;
                if(i< inputText.Length)
                    outputText.text += inputText[i];

                float x = Random.Range(min, max);
                audioSource.pitch = x;
                if (inputText[i] !=' ')
                audioSource.PlayOneShot(soundClip);
                if (i == inputText.Length)
                    startText = false;
                i++;
            }

        }

    }
}
