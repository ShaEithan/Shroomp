using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private StatusEffectController Status;
    private SimpleInventory inventory;
    public bool fireUp = false;
    public bool iceUp = false;
    public bool bombUp = false;
    // Start is called before the first frame update
    void Start()
    {
        Status = FindObjectOfType<StatusEffectController>();
        inventory = FindObjectOfType<SimpleInventory>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Power Up collected");
        sendPowerUpStatus();
        inventory.addItem(transform.GetComponent<SpriteRenderer>());
        Destroy(transform.gameObject);
    }
    void sendPowerUpStatus()
    {
        if (fireUp)
        {
            Status.fireUp = fireUp;
        }
        if (iceUp)
        {
            Status.iceUp = iceUp;
        }
        if (bombUp)
        {
            Status.bombUp = bombUp;
        }
    }
}
