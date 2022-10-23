using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] tBlocked, bBlocked, rBlocked, lBlocked;
    public GameObject[] t_b_rBlocked, b_r_lBlocked, r_l_tBlocked, t_b_lBlocked;
    public GameObject[] t_bBlocked, t_rBlocked, t_lBlocked, b_rBlocked, b_lBlocked, r_lBlocked;
    public GameObject closedRoom;

    public GameObject checker;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;

    void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
            spawnedBoss = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
