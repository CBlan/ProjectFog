using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionSparkSpawner : MonoBehaviour {

	public GameObject sparks;
	public float sparkThreshold = 5f;
	private GameObject obj;
	private Rigidbody rigidbody;

	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
        obj = Instantiate(sparks, transform.position, Quaternion.identity);
    }
	
	void Update ()
	{
		
	}

    //void OnCollisionEnter (Collision other)
    //{
    //       foreach (ContactPoint contact in other.contacts)
    //       {
    //       	if(other.relativeVelocity.magnitude > sparkThreshold)
    //       	{
    //       		obj = Instantiate (sparks, contact.point, Quaternion.FromToRotation(Vector3.up, contact.normal)) as GameObject;
    //       		obj.GetComponent<Rigidbody>().velocity = rigidbody.velocity;
    //       	}
    //       }
    //}
    void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.contacts[0];
        obj.SetActive(false);
        if (other.relativeVelocity.magnitude > sparkThreshold)
        {
            obj.transform.position = contact.point;
            obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            obj.SetActive(true);
        }

    }
}