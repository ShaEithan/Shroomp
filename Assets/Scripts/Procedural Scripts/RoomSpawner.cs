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
    public float waitTime = 10f;
    private float triggerDelay = 0f;
    void Start()
    {
        //Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        //Check for room core and fill walls as needed


        Invoke("Spawn", 0.2f);
    }
    private bool fixedUpdateRan = false;
    private void FixedUpdate()
    {
            //Debug.Log(transform.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")));
            if (transform.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")))
            {
                hasRoom = true;
            //fixedUpdateRan = true;
            //Debug.Log(triggerDelay);
            spawned = true;
            }
        triggerDelay += Time.deltaTime;
    }
    void Spawn()
    {

        //Spawn Rooms
        if (spawned == false && deadEnd && !hasRoom)
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
            Destroy(gameObject);
        }
    }
    private bool hasRoom = false;
    void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(triggerDelay > 1);
        if (triggerDelay > 0.1)
        {

                Debug.Log("Collision: " + collision);

            if (hasRoom && collision.CompareTag("RoomCore"))
            {
                spawned = true;
                Debug.Log("Room core name is: " + collision.transform.root.name);
                Debug.Log("Opening direction is: " + openingDirection);
                if (openingDirection == 1 && collision.transform.root.name.Contains('B'))
                    deadEnd = false;
                if (openingDirection == 2 && collision.transform.root.name.Contains('T'))
                    deadEnd = false;
                if (openingDirection == 3 && collision.transform.root.name.Contains('L'))
                    deadEnd = false;
                if (openingDirection == 4 && collision.transform.root.name.Contains('R'))
                    deadEnd = false;
                Debug.Log("deadEnd = " + deadEnd);
                if (!deadEnd)
                    Debug.Log("Roomcore collided with is: " + collision);
                //Normal door handler
                if (deadEnd)
                {
                    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!Dead end created for: " + openingDirection);
                    if (openingDirection == 1)
                        Instantiate(templates.blockT, transform.position - new Vector3(0, 14, 0), Quaternion.identity, transform.root);
                    if (openingDirection == 2)
                        Instantiate(templates.blockB, transform.position + new Vector3(0, 14, 0), Quaternion.identity, transform.root);
                    if (openingDirection == 3)
                        Instantiate(templates.blockR, transform.position - new Vector3(14, 0, 0), Quaternion.identity, transform.root);
                    if (openingDirection == 4)
                        Instantiate(templates.blockL, transform.position + new Vector3(14, 0, 0), Quaternion.identity, transform.root);

                }

                Destroy(gameObject);
            }

            if (collision.CompareTag("SpawnPoint") && !hasRoom && !spawned)
            {
                /**
                if (collision.GetInstanceID() < transform.GetInstanceID())
                {
                    Destroy(gameObject);
                }
                **/
                if (openingDirection == 1)
                    Instantiate(templates.blockT, transform.position - new Vector3(0, 14, 0), Quaternion.identity, transform.root);
                if (openingDirection == 2)
                    Instantiate(templates.blockB, transform.position + new Vector3(0, 14, 0), Quaternion.identity, transform.root);
                if (openingDirection == 3)
                    Instantiate(templates.blockR, transform.position - new Vector3(14, 0, 0), Quaternion.identity, transform.root);
                if (openingDirection == 4)
                    Instantiate(templates.blockL, transform.position + new Vector3(14, 0, 0), Quaternion.identity, transform.root);
                //Destroy(gameObject);
                spawned = true;
            }

            /**
            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false && !hasRoom)
            {
                if (spawned == false)
                {
                    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!Dead end created for: " + openingDirection);
                    
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    
                    if (openingDirection == 1)
                        Instantiate(templates.blockT, transform.position - new Vector3(0, 14, 0), Quaternion.identity, transform.root);
                    if (openingDirection == 2)
                        Instantiate(templates.blockB, transform.position + new Vector3(0, 14, 0), Quaternion.identity, transform.root);
                    if (openingDirection == 3)
                        Instantiate(templates.blockR, transform.position - new Vector3(14, 0, 0), Quaternion.identity, transform.root);
                    if (openingDirection == 4)
                        Instantiate(templates.blockL, transform.position + new Vector3(14, 0, 0), Quaternion.identity, transform.root);
                    Destroy(gameObject);
                }
                spawned = true;
                hasRoom = true;
            }
            **/
            //Destroy(gameObject);
        }
    }
}
