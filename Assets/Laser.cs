using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    public float rayDistance = 100f;
    public LineRenderer lineRenderer;
    public float rotationSpeed = 100f;
    public bool shoot = true;
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
        if (shoot)
        {
            
            ShootLaser();
            transform.localEulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        }
        if (!shoot)
        {
            lineRenderer.enabled = false;
            transform.localEulerAngles = Vector3.zero;
            
        }
    }
    void ShootLaser()
    {
        if(Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, LayerMask.GetMask("Shroomp")))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, LayerMask.GetMask("Shroomp"));
            if(hit.collider.isTrigger)
            {
                if(!FindObjectOfType<ShroompController>().isInvincible && !FindObjectOfType<ShroompController>().isDashing)
                FindObjectOfType<ShroompController>().ChangeHealth(-1);
            }
        }
        if (Physics2D.Raycast(transform.position, transform.up,Mathf.Infinity,LayerMask.GetMask("Blocks")))
        {
            RaycastHit2D _hit = Physics2D.Raycast(transform.position,transform.up,Mathf.Infinity, LayerMask.GetMask("Blocks"));
            Draw2DRay(transform.position, _hit.point);
            lineRenderer.enabled = true;
        }
        
        else
        {
            //Draw2DRay(transform.position, transform.up * rayDistance);
            lineRenderer.enabled = false;
        }
        
    }
    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
