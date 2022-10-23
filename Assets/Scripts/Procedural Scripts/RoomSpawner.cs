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
    public bool isChecker = false;
    private bool finishedChecking = false;
    public float waitTime = 4f;
    void Start()
    {
        //Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.01f);
    }
    void Spawn()
    {
        if (!isChecker)
        {
            //Spawn Rooms
            if (spawned == false && finishedChecking)
            {
                if (openingDirection == 1)
                {
                    //Need to spawn a room with Bottom door.
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                }
                else if (openingDirection == 2)
                {
                    //Need to spawn a room with Top door.
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                }
                else if (openingDirection == 3)
                {
                    //Need to spawn a room with Left door.
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                }
                else if (openingDirection == 4)
                {
                    //Need to spawn a room with Right door.
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                }
                spawned = true;
            }
            //Check rooms
            if (spawned == false && !finishedChecking)
            {
 
                    Instantiate(templates.checker, transform.position, templates.checker.transform.rotation,transform);

            }
        }
    }
    private bool tBlocked, bBlocked, rBlocked, lBlocked;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isChecker)
        {
            if (collision.CompareTag("SpawnPoint"))
            {
                if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
                {
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                spawned = true;
            }
        }
        if(isChecker)
        {
            if(collision.CompareTag("RoomCore"))
            {
                if (openingDirection == 1)
                    GetComponentInParent<RoomSpawner>().tBlocked = true;
                else if (openingDirection == 2)
                    GetComponentInParent<RoomSpawner>().bBlocked = true;
                else if (openingDirection == 3)
                    GetComponentInParent<RoomSpawner>().rBlocked = true;
                else if (openingDirection == 4)
                    GetComponentInParent<RoomSpawner>().lBlocked = true;
            }
        }
    }
}
