using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyGrenade : MonoBehaviour
{

    private bool hasCollided = false;
    public float explodeTime = 5;
    public float damage = 20;
    public float radius = 5.0F;
    public float power = 10.0F;
    public GameObject explosion;
    private Rigidbody rB;
    private int grenadeCount;
    private bool mineActive = false;
    private Coroutine explodeRoutine = null;
    private List<GameObject> thingsHit;

    private void Start()
    {
        rB = GetComponent<Rigidbody>();
        //explodeRoutine = StartCoroutine(Explode());
        Destroy(gameObject, 20);
    }

    private void OnCollisionEnter(Collision collision)
    {

        //if (!collision.gameObject.CompareTag("Ranged") || !collision.gameObject.CompareTag("Melee"))
        //{
        //StopCoroutine(explodeRoutine);
        mineActive = true;
        explodeRoutine = StartCoroutine(Explode());
        //}

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

    private void OnTriggerStay(Collider other)
    {
        if (mineActive)
        {
            if (other.gameObject.CompareTag("Ranged") || other.gameObject.CompareTag("Melee"))
            {
                StopCoroutine(explodeRoutine);
                explodeTime = 0;
                StartCoroutine(Explode());
            }
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explodeTime);
        AddDescendantsWithTag(gameObject.transform);
        damage = damage * (grenadeCount + 1);
        Vector3 explosionPos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius, Physics.AllLayers);
        thingsHit = new List<GameObject>();

        foreach (Collider hit in colliders)
        {
            //if(!thingsHit.Contains(hit.gameObject))
            //{
            //    print("adding " + hit.gameObject.name);
            //    thingsHit.Add(hit.gameObject);
            //}
            Rigidbody rB;
            EnemyHealth hP;

            if (!thingsHit.Contains(hit.gameObject.transform.root.gameObject))
            {
                if (rB = hit.gameObject.GetComponent<Rigidbody>())
                {
                    print(hit.gameObject);
                    rB.AddExplosionForce(power, explosionPos, radius, 0.0F);
                    thingsHit.Add(hit.gameObject.transform.root.gameObject);
                    if (hP = hit.gameObject.GetComponent<EnemyHealth>())
                    {
                        hP.health -= damage;
                        //print("here2");
                    }
                }
            }
        }

        //foreach (GameObject i in thingsHit)
        //{
        //    Rigidbody rB;
        //    EnemyHealth hP;

        //    if (rB = i.GetComponent<Rigidbody>())
        //    {
        //        print(i);
        //        rB.AddExplosionForce(power, explosionPos, radius, 0.0F);
        //        thingsHit.Add(i);
        //        if (hP = i.GetComponent<EnemyHealth>())
        //        {
        //            hP.health -= damage;
        //            //print("here2");
        //        }
        //    }
        //}
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
