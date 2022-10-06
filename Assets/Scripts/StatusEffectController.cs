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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
