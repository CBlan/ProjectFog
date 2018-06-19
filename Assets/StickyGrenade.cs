using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyGrenade : MonoBehaviour {

    private bool hasCollided = false;
    public float explodeTime = 5;
    public float damage;
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
        if (!hasCollided)
        {
            GameObject emptyObject = new GameObject();
            emptyObject.transform.SetParent(collision.gameObject.transform);
            gameObject.transform.SetParent(emptyObject.transform);
            rB.isKinematic = true;
            hasCollided = true;
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explodeTime);
        Instantiate(explosion, transform.position, Quaternion.identity);
        AddDescendantsWithTag(gameObject.transform);
        damage = damage * (grenadeCount + 1);
        //print(damage);
        Destroy(gameObject);
        yield break;
    }

    private void AddDescendantsWithTag(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == "Grenade")
            {
                grenadeCount++;
            }
            AddDescendantsWithTag(child);
        }
    }
}
