using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    public bool realRoom = false;
    private RoomTemplates templates;
    void Start()
    {
        Invoke("makeReal", 0.1f);
        if(!(transform.name == "Edge"))
        Invoke("addRoom", 0.2f);
    }
    void addRoom()
    {
        if (!transform.root.name.Contains('x'))
        {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            templates.rooms.Add(this.gameObject);
        }
    }
    void makeReal()
    {
        realRoom = true;
    }
}
