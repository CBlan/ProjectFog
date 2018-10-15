using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Turret_AI : MonoBehaviour
{
    public List<GameObject> enemies;

    public Transform target;
    public Transform gun;
    public Transform muzzle;
    public float turretDegreesPerSecond = 45.0f;
    public float gunDegreesPerSecond = 45.0f;

    public float maxGunAngle = 45.0f;

    public float attackRange;
    public float bulletInterval;
    public float fireingCone;
    public float damage;

    public GameObject particle;

    private float targetHP;

    private Quaternion qTurret;
    private Quaternion qGun;
    private Quaternion qGunStart;
    private Transform trans;

    private float curDist = 100000;
    private float cooldown;

    void Start()
    {
        trans = transform;
        qGunStart = gun.transform.localRotation;
    }

    void Update()
    {
        if (GameManager.instance != null)
        {
            enemies = GameManager.instance.enemies;
        }
        //check if there are enemies
        if (enemies.Count > 0)
        {
            //go through each enemey
            foreach (var enemy in enemies)
            {
                //check if enemy is in line of sight
                if (CanSeeTarget(enemy.transform))
                {
                    //check distance of seen enemy
                    //print("Checking enemy: " + enemy.transform.name);
                    var distance = Vector3.Distance(enemy.transform.position, transform.position);

                    //if distance if this enemy is less than the distance of the current targets distance swap targets
                    //print("distance " + distance + " for " + enemy.transform.name);
                    //print("Current distance of targeted enemy: " + curDist);
                    if (distance < curDist)
                    {
                        curDist = distance;
                        target = enemy.transform;
                    }

                    if (target == enemy.transform)
                    {
                        curDist = distance;
                        //print("My closest selected enemy so far is: " + target.transform.name);
                    }

                }

            }
            //Reset curdist outside of loop so it chooses only from avaliible targets next iteration
            curDist = 100000;
        }

        if (target == null)
            return;

        if (!CanSeeTarget(target))
            return;


        //rotate to target
        Debug.DrawRay(gun.position, gun.forward * 20.0f);
        float distanceToPlane = Vector3.Dot(trans.up, target.position - trans.position);
        Vector3 planePoint = target.position - trans.up * distanceToPlane;

        qTurret = Quaternion.LookRotation(planePoint - trans.position, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTurret, turretDegreesPerSecond * Time.deltaTime);

        Vector3 v3 = new Vector3(0.0f, distanceToPlane, (planePoint - transform.position).magnitude);
        qGun = Quaternion.LookRotation(v3);

        if (Quaternion.Angle(qGunStart, qGun) <= maxGunAngle)
            gun.localRotation = Quaternion.RotateTowards(gun.localRotation, qGun, gunDegreesPerSecond * Time.deltaTime);
        else
            Debug.Log("Target beyond gun range");
        //rotate to target end

        //If target is within fireing cone then fire
        var forward = muzzle.TransformDirection(Vector3.forward);
        var targetDir = target.transform.position - transform.position;

        if (Vector3.Angle(forward, targetDir) < fireingCone)
        {
            if (Time.time >= cooldown)
            {
                //Fire(turretBullet);
                Instantiate(particle, muzzle.transform);
                //target.GetComponent<Enemy_HP>().HP -= damage;

                //print("Fireing at " + target.transform.name);
                cooldown = bulletInterval + Time.time;
            }

        }

        //print("Ange to target: " + Vector3.Angle(forward, targetDir) + " Fireing cone:" + fireingCone);
    }

    //Check to see if target is in line of sight
    private bool CanSeeTarget(Transform target)
    {
        //print(target);
        //removes layer 8 and 9 (bullet layers) and player from linecast with a layermask so that bullets do not interfere with if it can see the target
        int layerMask1 = 1 << 8;
        int layerMask2 = 1 << 9;
        int layerMask3 = 1 << 10;
        int layerMask4 = 1 << 13;
        int layerMask = layerMask1 | layerMask2 | layerMask3 | layerMask4;
        layerMask = ~layerMask;

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > attackRange)
                return false;
            RaycastHit hit;
            if (Physics.Linecast(muzzle.position, target.transform.position, out hit, layerMask))
            {
                //print((hit.transform == target.transform) + " || " + (hit1.transform == target.transform));
                return (hit.transform == target);
            }

            return false;
        }
        else return false;

    }
}