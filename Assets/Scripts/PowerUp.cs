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
    public bool bDashUp = false;
    public bool wideUp = false;
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Power Up collected");
        sendPowerUpStatus();
        //inventory.addItem(transform.GetComponent<SpriteRenderer>());
        sendToInv();
        Destroy(transform.gameObject);
    }
    private void sendToInv()
    {
        if (fireUp)
            inventory.addItem("fireUp");
        if (iceUp)
            inventory.addItem("iceUp");
        if (bombUp)
            inventory.addItem("bombUp");
        if (bDashUp)
            inventory.addItem("bDashUp");
        if (wideUp)
            inventory.addItem("wideUp");
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
        if (bDashUp)
        {
            Status.bDashUp = bDashUp;
        }
        if(wideUp)
        {
            Status.wideUp = wideUp;
        }
    }
}
