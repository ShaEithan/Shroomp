using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField]
    private bool spawned = false;
    [SerializeField]
    private bool deadEnd = false;
    private bool needDeadend = false;
    public float waitTime = 10f;
    private float triggerDelay = 0f;
    void Start()
    {
        //Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        //Check for room core and fill walls as needed


        //Invoke("Spawn", 1f);
        spawnDelay += Random.Range(0.2f, 0.4f);
    }
    private float spawnDelay = 0.03f;
    private float destroyDelay = 6f;
    private void Update()
    {

        triggerDelay += Time.deltaTime;
        spawnDelay -= Time.deltaTime;
        destroyDelay -= Time.deltaTime;
        if (spawnDelay < 0)
            Spawn();
        if (destroyDelay < 0)
            Destroy(gameObject);
        //// If spawn fails dead end created here
        //if (needDeadend && !hasRoom)
        //{
        //    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!NEED DEAD END: " + openingDirection);
        //    if (openingDirection == 1)
        //        Instantiate(templates.blockT, transform.position - new Vector3(0, 14, 0), Quaternion.identity, transform.root);
        //    if (openingDirection == 2)
        //        Instantiate(templates.blockB, transform.position + new Vector3(0, 14, 0), Quaternion.identity, transform.root);
        //    if (openingDirection == 3)
        //        Instantiate(templates.blockR, transform.position - new Vector3(14, 0, 0), Quaternion.identity, transform.root);
        //    if (openingDirection == 4)
        //        Instantiate(templates.blockL, transform.position + new Vector3(14, 0, 0), Quaternion.identity, transform.root);
        //    Destroy(gameObject);
        //}
    }

    private void FixedUpdate()
    {
        /**
        //Debug.Log(transform.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")));
        if (transform.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")))
        {
            hasRoom = true;
            //fixedUpdateRan = true;
            //Debug.Log(triggerDelay);
            //spawned = true;
        }
        **/
    }
    public GameObject spawnedRoom = null;
    void Spawn()
    {
        //Spawn Rooms
        if (spawned == false && !hasRoom && !deadEnd)
        {

            if (openingDirection == 1)
            {

                //Need to spawn a room with Bottom door.
                if (spawnedRoom == null)
                {
                    rand = Random.Range(0, templates.BRooms.Count);
                    spawnedRoom = Instantiate(templates.BRooms[rand], transform.position, templates.BRooms[rand].transform.rotation);
                }
                if (spawnedRoom.transform.GetChild(0).GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")))
                {
                    Destroy(spawnedRoom);
                    spawnedRoom = null;
                    //needDeadend = true;
                    Debug.Log("---------------------------ROOM DELETED RETRYING ROOM IS NOW = " + spawnedRoom);
                }
            }
            else if (openingDirection == 2)
            {
                //Need to spawn a room with Top door.
                if (spawnedRoom == null)
                {
                    rand = Random.Range(0, templates.TRooms.Count);
                    spawnedRoom = Instantiate(templates.TRooms[rand], transform.position, templates.TRooms[rand].transform.rotation);
                }
                if (spawnedRoom.transform.GetChild(0).GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")))
                {
                    Destroy(spawnedRoom);
                    spawnedRoom = null;
                    //needDeadend = true;
                    Debug.Log("---------------------------ROOM DELETED RETRYING ROOM IS NOW = " + spawnedRoom);
                }

            }
            else if (openingDirection == 3)
            {
                //Need to spawn a room with Left door.
                if (spawnedRoom == null)
                {
                    rand = Random.Range(0, templates.LRooms.Count);
                    spawnedRoom = Instantiate(templates.LRooms[rand], transform.position, templates.LRooms[rand].transform.rotation);
                }
                if (spawnedRoom.transform.GetChild(0).GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")))
                {
                    Destroy(spawnedRoom);
                    spawnedRoom = null;
                    //needDeadend = true;
                    Debug.Log("---------------------------ROOM DELETED RETRYING ROOM IS NOW = " + spawnedRoom);

                }
            }
            else if (openingDirection == 4)
            {
                //Need to spawn a room with Right door.
                if (spawnedRoom == null)
                {
                    rand = Random.Range(0, templates.RRooms.Count);
                    spawnedRoom = Instantiate(templates.RRooms[rand], transform.position, templates.RRooms[rand].transform.rotation);
                }
                if (spawnedRoom.transform.GetChild(0).GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("RoomCore")))
                {
                    Destroy(spawnedRoom);
                    spawnedRoom = null;
                    //needDeadend = true;
                    Debug.Log("---------------------------ROOM DELETED RETRYING ROOM IS NOW = " + spawnedRoom);
                }
            }
            //spawned = true;
            //Destroy(gameObject);
        }
    }
    public bool hasRoom = false;
    void OnTriggerStay2D(Collider2D collision)
    {
        // Checks if room is real
        if (collision.CompareTag("RoomCore"))
        {
            if (collision.GetComponentInParent<AddRoom>().realRoom)
                hasRoom = true;
        }
        //Debug.Log(triggerDelay > 1);
        if (triggerDelay > 1)
        {

            //Debug.Log("Collision: " + collision);

            if (hasRoom && collision.CompareTag("RoomCore"))
            {
                if (collision.GetComponentInParent<AddRoom>().realRoom)
                {

                    spawned = true;
                    //Debug.Log("Room core name is: " + collision.transform.root.name);
                    //Debug.Log("Opening direction is: " + openingDirection);
                    if (openingDirection == 1 && !collision.transform.root.name.Contains('B'))
                        deadEnd = true;
                    if (openingDirection == 2 && !collision.transform.root.name.Contains('T'))
                        deadEnd = true;
                    if (openingDirection == 3 && !collision.transform.root.name.Contains('L'))
                        deadEnd = true;
                    if (openingDirection == 4 && !collision.transform.root.name.Contains('R'))
                        deadEnd = true;
                    //Debug.Log("deadEnd = " + deadEnd);
                    if (!deadEnd)
                        Debug.Log("No deadendHere " + collision);
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
                        Destroy(gameObject);
                    }
                }
                //Destroy(gameObject);
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
