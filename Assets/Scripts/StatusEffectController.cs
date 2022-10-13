using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    //Used as global time DOT lasts
    public float DOT = 2f;
    //Keep track of power ups activated
    public bool fireUp = true;
    public bool iceUp = true;
    public bool bombUp;
    public bool bDashUp;

    // Start is called before the first frame update
    void Start()
    {
        //bombUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(fireUp || iceUp || bombUp||bDashUp))
        {
            DOT = 0f;
        }
        else { DOT = 10f; }
        //Debug.Log("Fire is: " + fireUp + " Ice is: " + iceUp + " Bomb is: " + bombUp);
    }

    public float getDotTime()
    {
        return (DOT);
    }
    public bool getFireStatus()
    {
        return fireUp;
    }
    public bool getIceStatus()
    {
        return iceUp;
    }
}
