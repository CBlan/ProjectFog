using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour {

    public GameObject grenade;
    private GameObject thrownGrenade;
    public AnimationCurve powerCurve;
    float throwPower;
    public float maxThrowPower;
    public float cooldown;
    private float timer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && timer <= 0)
        {
            StartCoroutine(CalculateThrowStrength());
            timer = cooldown;
        }
        timer -= Time.deltaTime;
        transform.LookAt(TargetObject.tarObj.hitPoint);
    }

    IEnumerator CalculateThrowStrength() //increases hitpower overtime.
    {
        float CurveTime = 0f;
        float CurvePosition = 0f;
        throwPower = 0f;
        while (Input.GetButton("Fire1"))
        {
            CurveTime += Time.deltaTime *0.1f;
            CurvePosition = powerCurve.Evaluate(CurveTime);
            throwPower = maxThrowPower * CurvePosition;
            yield return null;
        }
        thrownGrenade = Instantiate(grenade, transform.position, Quaternion.identity);
        thrownGrenade.GetComponent<Rigidbody>().AddForce(transform.forward * throwPower, ForceMode.Impulse);
        yield break;
    }
}
