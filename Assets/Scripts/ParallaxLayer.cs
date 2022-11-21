using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    Vector3 newPos;
    public float parallaxFactorX, parallaxFactorY;
    private void Start()
    {
         newPos = transform.localPosition;
    }
    public void Move(float deltaX, float deltaY)
    {

        
        newPos.x -= deltaX * parallaxFactorX;
        newPos.y -= deltaY * parallaxFactorY;
        transform.localPosition = newPos;
    }
}

