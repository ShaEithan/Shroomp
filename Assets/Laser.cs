using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float rayDistance = 100f;
    public LineRenderer lineRenderer;
    public int layer = 6;
    //Transform m_transform;

    private void Awake()
    {
        //m_transform = GetComponent<Transform>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaser();
    }
    void ShootLaser()
    {
        if (Physics2D.Raycast(transform.position, transform.right,Mathf.Infinity,LayerMask.GetMask("Blocks")))
        {
            RaycastHit2D _hit = Physics2D.Raycast(transform.position,transform.right,Mathf.Infinity, LayerMask.GetMask("Blocks"));
            Draw2DRay(transform.position, _hit.point);
        }
        /**
        else
        {
            Draw2DRay(transform.position, transform.right * rayDistance);
        }
        **/
    }
    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
