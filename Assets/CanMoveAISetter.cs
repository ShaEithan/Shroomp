using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveAISetter : MonoBehaviour
{
    public AOETarget target;
    public AIPath AIPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target.hasTarget)
            AIPath.canMove = true;

        if (!target.hasTarget)
            AIPath.canMove = false;
    }
}
