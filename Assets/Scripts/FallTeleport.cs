using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTeleport : MonoBehaviour
{
    public float targetHeight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x,
                targetHeight, collision.gameObject.transform.position.z);
        }
    }
}
