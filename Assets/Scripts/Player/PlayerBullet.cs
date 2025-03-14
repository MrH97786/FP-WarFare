using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int bulletDamage;

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

        if (hitTransform.CompareTag("Enemy"))
        {
            if (hitTransform.gameObject.GetComponent<Enemy>().isDead == false)
            {
                hitTransform.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }
            
            CreateBloodSprayEffect(hitTransform, collision);
        }

        Destroy(gameObject);
    }

    void CreateBulletImpactEffect(Transform objectWeHit, Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffect, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(objectWeHit);
    }

    void CreateBloodSprayEffect(Transform objectWeHit, Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject bloodSpray = Instantiate(GlobalReferences.Instance.bloodSprayEffect, contact.point, Quaternion.LookRotation(contact.normal));
        bloodSpray.transform.SetParent(objectWeHit);
    }
}
