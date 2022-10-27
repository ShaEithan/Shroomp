using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    //1 Need Bottom door
    //2 Need Top door
    //3 Need Left door
    //4 Need Right door
    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;
    private bool deadEnd = true;
    public float waitTime = 4f;
    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }
    void Spawn()
    {

        //Spawn Rooms
        if (spawned == false && deadEnd)
        {
            if (openingDirection == 1)
            {
                //Need to spawn a room with Bottom door.
                rand = Random.Range(0, templates.BRooms.Count);
                Instantiate(templates.BRooms[rand], transform.position, templates.BRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                //Need to spawn a room with Top door.
                rand = Random.Range(0, templates.TRooms.Count);
                Instantiate(templates.TRooms[rand], transform.position, templates.TRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                //Need to spawn a room with Left door.
                rand = Random.Range(0, templates.LRooms.Count);
                Instantiate(templates.LRooms[rand], transform.position, templates.LRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                //Need to spawn a room with Right door.
                rand = Random.Range(0, templates.RRooms.Count);
                Instantiate(templates.RRooms[rand], transform.position, templates.RRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomCore"))
        {
            Debug.Log("Room core name is: " + collision.transform.root.name);
            if (openingDirection == 1 && collision.transform.root.name.Contains('B'))
                deadEnd = false;
            if (openingDirection == 2 && collision.transform.root.name.Contains('T'))
                deadEnd = false;
            if (openingDirection == 3 && collision.transform.root.name.Contains('L'))
                deadEnd = false;
            if (openingDirection == 4 && collision.transform.root.name.Contains('R'))
                deadEnd = false;
            Debug.Log("Room core spawned = " + deadEnd);
            if (deadEnd)
            {
                if (openingDirection == 1)
                    Instantiate(templates.blockT, transform.parent.position, Quaternion.identity);
                if (openingDirection == 2)
                    Instantiate(templates.blockB, transform.parent.position, Quaternion.identity);
                if (openingDirection == 3)
                    Instantiate(templates.blockR, transform.parent.position, Quaternion.identity);
                if (openingDirection == 4)
                    Instantiate(templates.blockL, transform.parent.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false && deadEnd)
            {
                if (spawned == false)
                {
                    /**
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    **/
                    if (openingDirection == 1)
                        Instantiate(templates.blockT, transform.parent.position, Quaternion.identity);
                    if (openingDirection == 2)
                        Instantiate(templates.blockB, transform.parent.position, Quaternion.identity);
                    if (openingDirection == 3)
                        Instantiate(templates.blockR, transform.parent.position, Quaternion.identity);
                    if (openingDirection == 4)
                        Instantiate(templates.blockL, transform.parent.position, Quaternion.identity);
                    //Destroy(gameObject);
                }
            spawned = true;
        }
            
        //Destroy(gameObject);
    }
}
