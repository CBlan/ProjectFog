using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour {

    public GameObject[] grenades;
    public int currentGrenade;
    private GameObject thrownGrenade;
    public AnimationCurve powerCurve;
    public float throwPower;
    public float maxThrowPower;
    public float cooldown;
    private float timer;
    private Rigidbody thrownGrenadeRB;
	
	// Update is called once per frame
	void Update () {
        if (Time.timeSinceLevelLoad > 1f)
        {
            if (Input.GetButtonDown("Fire1") && timer <= 0)
            {
                StartCoroutine(CalculateThrowStrength());
            }
            timer -= Time.deltaTime;
            transform.LookAt(TargetObject.tarObj.hitPoint);
        }
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
            //print(throwPower);
            yield return null;
        }
        timer = cooldown;
        thrownGrenade = Instantiate(grenades[currentGrenade], transform.position, Quaternion.identity);
        thrownGrenadeRB = thrownGrenade.GetComponent<Rigidbody>();
        thrownGrenadeRB.AddForce(transform.forward * throwPower, ForceMode.Impulse);
        thrownGrenadeRB.angularVelocity = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500), Random.Range(-500, 500));
        throwPower = 0f;

        yield break;
    }
}
