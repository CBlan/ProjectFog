using UnityEngine;
using System.Collections;

public class Motion_Rotate : MonoBehaviour
{

    public float RotateSpeed = 4f;
	public Vector3 axis;

	void Update ()
	{
        transform.Rotate(axis, RotateSpeed * Time.deltaTime);
	}
}