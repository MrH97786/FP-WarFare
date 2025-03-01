using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Target"))
        {
            Debug.Log("Hit " + collision.gameObject.name + " !");
        }
        Destroy(gameObject);
    }
}
