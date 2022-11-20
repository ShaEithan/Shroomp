using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCheck : MonoBehaviour
{
    ShroompController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.name == "Shroomp")
        {
            player.ChangeHealth(-1);
        }
    }
}
