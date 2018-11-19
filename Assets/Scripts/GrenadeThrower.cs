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
    private bool superGrenadeThrower;
	
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

    public void ActivateSuperThrower()
    {
        StartCoroutine(SuperThrower());
    }

    IEnumerator SuperThrower()
    {
        cooldown = 0.1f;
        superGrenadeThrower = true;
        yield return new WaitForSeconds(10f);
        cooldown = 0.8f;
        superGrenadeThrower = false;
        yield break;
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
        if (superGrenadeThrower)
        {
            throwPower = maxThrowPower;
        }
        thrownGrenade = Instantiate(grenades[currentGrenade], transform.position, Quaternion.identity);
        thrownGrenadeRB = thrownGrenade.GetComponent<Rigidbody>();
        thrownGrenadeRB.AddForce(transform.forward * throwPower, ForceMode.Impulse);
        thrownGrenadeRB.angularVelocity = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500), Random.Range(-500, 500));
        throwPower = 0f;

        yield break;
    }
}
