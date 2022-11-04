using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReticleScript : MonoBehaviour
{
    Vector2 pos;
    ShroompController shroomp;
    // Start is called before the first frame update
    void Start()
    {
        shroomp = FindObjectOfType<ShroompController>();

        var offset = testX;

        
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, Input.mousePosition,
            parentCanvas.worldCamera,
            out pos);
        
        pos = parentCanvas.GetComponent<RectTransform>().rect.center;
    }
    private Vector2 mouseDirection,charOrigin;
    public float testX = 10f;
    public Canvas parentCanvas;
    // Update is called once per frame
    void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        //transform.position = parentCanvas.transform.TransformPoint(new Vector3(testX,testX,1f));
        // Sets char origin
        /**
        charOrigin = new Vector2(Camera.main.WorldToScreenPoint(shroomp.GetComponent
            <Rigidbody2D>().transform.position).x, Camera.main.WorldToScreenPoint(shroomp.GetComponent
            <Rigidbody2D>().transform.position).y);
        **/
        charOrigin = pos;
        /**
        //Assigns x and y values based on char position
        if (Input.mousePosition.x > charOrigin.x)
        {
            mouseDirection.x = Input.mousePosition.x - charOrigin.x;
        }
        if (Input.mousePosition.x < charOrigin.x)
        {
            mouseDirection.x = -1 * (charOrigin.x - Input.mousePosition.x);
        }
        if (Input.mousePosition.y > charOrigin.y)
        {
            mouseDirection.y = Input.mousePosition.y - charOrigin.y;
        }
        if (Input.mousePosition.y < charOrigin.y)
        {
            mouseDirection.y = -1 * (charOrigin.y - Input.mousePosition.y);
        }
        **/
        if (movePos.x > charOrigin.x)
        {
            mouseDirection.x = movePos.x - charOrigin.x;
        }
        if (movePos.x < charOrigin.x)
        {
            mouseDirection.x = -1 * (charOrigin.x - movePos.x);
        }
        if (movePos.y > charOrigin.y)
        {
            mouseDirection.y = movePos.y - charOrigin.y;
        }
        if (movePos.y < charOrigin.y)
        {
            mouseDirection.y = -1 * (charOrigin.y - movePos.y);
        }

        //Normalizes the mouse direction vector
        mouseDirection.Normalize();

        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2((mouseDirection.x * shroomp.dashPower) * shroomp.dashTime *testX, (mouseDirection.y * shroomp.dashPower) * shroomp.dashTime *testX);
        //transform.GetComponent<RectTransform>().position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}
