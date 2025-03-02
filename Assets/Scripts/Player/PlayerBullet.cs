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
            CreateBulletImpactEffect(hitTransform, collision);
        }
        
        if (hitTransform.CompareTag("Wall") || hitTransform.CompareTag("Floor"))
        {
            Debug.Log("Hit a wall or floor!");
            CreateBulletImpactEffect(hitTransform, collision);
        }

        Destroy(gameObject);
    }

    void CreateBulletImpactEffect(Transform objectWeHit, Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffect, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(objectWeHit);
    }
}
