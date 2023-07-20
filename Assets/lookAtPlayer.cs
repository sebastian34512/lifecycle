using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f; // Ignoriere die Y-Komponente

            // Richte das Game Object in Richtung des Spielers aus
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
