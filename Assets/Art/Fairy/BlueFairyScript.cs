using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlueFairyScript : MonoBehaviour
{
    public float shieldRegenTime = 3f;
    public bool shieldStatus = false;
    private bool isInvoking = false;
    private Light2D light2d;
    // Start is called before the first frame update
    void Start()
    {
        light2d = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!shieldStatus && !isInvoking)
        {
            isInvoking = true;
            Invoke("refreshShield", shieldRegenTime);
        }
        if (shieldStatus)
            light2d.enabled = true;
        if(!shieldStatus)
            light2d.enabled = false;
    }
    private void refreshShield()
    {
        CancelInvoke("refreshShield");
        shieldStatus = true;
        isInvoking = false;
    }
}
