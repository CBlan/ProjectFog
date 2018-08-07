using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour {

    public GameObject[] muzzles;
    public GameObject bullet;
    public float bulletSpeed = 500;
    private GameObject shot;

    public void Shoot()
    {
        foreach (GameObject muzzle in muzzles)
        {
            muzzle.transform.LookAt(GameManager.instance.player.transform);
            shot = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
            shot.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        }
    }

}
