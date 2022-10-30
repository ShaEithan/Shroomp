using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    public GameObject blockT, blockB, blockL, blockR;

    public GameObject checker;

    public List<GameObject> rooms;
    public List<GameObject> unsortedRooms, TRooms, BRooms, LRooms, RRooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;
    AstarPath pathFinder;

    void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
            spawnedBoss = true;
            pathFinder.Scan();
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
    private void Start()
    {
        foreach (var room in unsortedRooms)
        {
            if (room.name.Contains('T'))
                TRooms.Add(room);
            if (room.name.Contains('B'))
                BRooms.Add(room);
            if (room.name.Contains('L'))
                LRooms.Add(room);
            if (room.name.Contains('R'))
                RRooms.Add(room);

        }
        Debug.Log("T room counts" + TRooms.Count);
        Debug.Log("B room counts" + BRooms.Count);
        Debug.Log("L room counts" + LRooms.Count);
        Debug.Log("R room counts" + RRooms.Count);
        pathFinder = FindObjectOfType<AstarPath>();
    }
}
