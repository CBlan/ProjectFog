using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyGrenade : MonoBehaviour {

    private bool hasCollided = false;
    public float explodeTime = 5;
    public float damage = 20;
    public float radius = 5.0F;
    public float power = 10.0F;
    public GameObject explosion;
    private Rigidbody rB;
    private int grenadeCount;

    private void Start()
    {
        rB = GetComponent<Rigidbody>();
        StartCoroutine(Explode());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (!hasCollided)
            {
                GameObject emptyObject = new GameObject();
                emptyObject.transform.SetParent(collision.gameObject.transform);
                gameObject.transform.SetParent(emptyObject.transform);
                rB.isKinematic = true;
                hasCollided = true;
            }
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explodeTime);
        AddDescendantsWithTag(gameObject.transform);
        damage = damage * (grenadeCount + 1);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rB;
            EnemyHealth hP;

            if (rB = hit.GetComponent<Rigidbody>())
            {
                rB.AddExplosionForce(power, explosionPos, radius, 0.0F);
                if (hP = hit.GetComponent<EnemyHealth>())
                {
                    hP.health -= damage;
                }
            }
        }
        //print(damage);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield break;
    }

    private void AddDescendantsWithTag(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.CompareTag("Grenade"))
            {
                grenadeCount++;
            }
            AddDescendantsWithTag(child);
        }
    }
}
