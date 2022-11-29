using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle3 : MonoBehaviour
{
    Vector2 pos;
    ShroompController shroomp;
    // Start is called before the first frame update
    void Start()
    {
        shroomp = FindObjectOfType<ShroompController>();

    }
    private Vector2 mouseDirection, charOrigin;
    public float testX = 10f;
    // Update is called once per frame
    void Update()
    {
        Vector2 movePos;
        charOrigin = shroomp.transform.position;
        movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

        transform.position = new Vector2(charOrigin.x+(mouseDirection.x * shroomp.dashPower) * shroomp.dashTime * testX, charOrigin.y + (mouseDirection.y * shroomp.dashPower) * shroomp.dashTime * testX);
    }
}
