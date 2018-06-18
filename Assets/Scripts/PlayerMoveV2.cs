using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMoveV2 : MonoBehaviour
{

    public float speed = 6f;
    public float airSpeed = 25f;
    public float gravity = 15f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.5f;
    public float slopeLimit = 45;
    private bool anchored = false;
    private Rigidbody rB;
    private CapsuleCollider capsule;
    private Collision hitPoint;
    private Vector3 reflect;
    private bool sliding;
    public float antiBumpFactor = 0.6f;
    private bool dash;
    public float dashForce = 500f;
    public float dashLength = 0.1f;
    private bool normalJumpDown;
    public float dashCooldown = 3f;
    private bool dashReady;
    public float bulletTime = 10f;
    public float bulletTimeCooldown = 5f;
    private bool bulletTimeReady;
    public ParticleSystem dashTrail;
    public ParticleSystem dashCameraEffect;
    public AnimationCurve animCurve;

    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        rB = GetComponent<Rigidbody>();
        rB.freezeRotation = true;
        rB.useGravity = false;
        dashReady = true;
        bulletTimeReady = true;
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (dash)
        {
            return;
        }

        //print(IsOnGround());
        if (IsOnGround())
        {
            
            anchored = false;
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(inputX, 0, inputY);
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rB.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = -antiBumpFactor;
            rB.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButtonDown("Jump"))
            {
                normalJumpDown = true;
                rB.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed() + antiBumpFactor*3, velocity.z);
            }
        }

        if (anchored && !sliding)
        {
            rB.velocity = Vector3.zero;

            if (canJump && Input.GetButtonDown("Jump"))
            {
                normalJumpDown = false;
                sliding = true;
            }

            if (canJump && Input.GetButtonUp("Jump") && !normalJumpDown)
            {
                Vector3 targetVelocity = new Vector3(inputX, 0, inputY);
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= speed;

                rB.velocity = new Vector3(targetVelocity.x, CalculateJumpVerticalSpeed(), targetVelocity.z);
                anchored = false;
            }
        }

        if (sliding)
        {
            rB.AddForce(new Vector3(0, -gravity * rB.mass, 0));

            if (canJump && Input.GetButtonUp("Jump") && !normalJumpDown)
            {
                Vector3 targetVelocity = new Vector3(inputX, 0, inputY);
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= speed;

                rB.velocity = new Vector3(targetVelocity.x, CalculateJumpVerticalSpeed(), targetVelocity.z);
                anchored = false;
                sliding = false;
            }
        }

        //print(!IsOnGround() && !anchored);

        //print (anchored);

        if (!IsOnGround() && !anchored && !sliding)
        {
            Vector3 targetVelocity = new Vector3(inputX, 0, inputY);
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= airSpeed;


            rB.AddForce(new Vector3(targetVelocity.x, -gravity * rB.mass, targetVelocity.z));


        }

        if (Input.GetButtonDown("Run") && !dash && dashReady)
        {
            dash = true;
            dashReady = false;
            rB.velocity = Vector3.zero;
            StartCoroutine(DashTime());
        }
        // We apply gravity manually for more tuning control
        //rB.AddForce(new Vector3(0, -gravity * rB.mass, 0));

        if (Input.GetButtonDown("BulletTime") && bulletTimeReady)
        {
            bulletTimeReady = false;
            StartCoroutine(BulletTimeTimer());
        }



        //anchored = false;
    }

    public bool IsOnGround()
    {
        //RaycastHit hit;
        //float rayDistance = (capsule.height / 2) + (capsule.radius/2);
        //if (Physics.SphereCast(transform.position, capsule.radius, -Vector3.up, out hit, rayDistance))
        //{
        //    if (Vector3.Angle(hit.normal, Vector3.up) < slopeLimit)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        print("here");
        //        return false;
        //    }
        //}

        //else return false;

        RaycastHit hit;
        RaycastHit hit1;
        float rayDistance = (capsule.height / 2) + (capsule.radius);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
        {
            if (Vector3.Angle(hit.normal, Vector3.up) < slopeLimit)
            {
                return true;
            }
            else
            {
                //print("here");
                return false;
            }
        }
        else if (Physics.SphereCast(transform.position, capsule.radius + 0.1f, -Vector3.up, out hit1, rayDistance))
        {
            if (Vector3.Angle(hit1.normal, Vector3.up) < slopeLimit)
            {
                return true;
            }
            else
            {
                //print("here1");
                return false;
            }
        }

        else return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOnGround())
        {
            anchored = true;
            //if (dash)
            //{
            //    Camera.main.GetComponent<Motion_Shake>().Shake(0.1f, 0.5f);
            //}
        }
        //anchored = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        anchored = false;
    }

    //void OnCollisionStay()
    //{
    //    anchored = true;
    //}

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    IEnumerator DashTime()
    {
        dashTrail.Play();
        dashCameraEffect.Play();
        rB.AddForce(Camera.main.transform.forward * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dashLength);
        rB.velocity = Vector3.zero;
        dash = false;
        dashTrail.Stop();
        dashCameraEffect.Stop();
        yield return new WaitForSecondsRealtime(dashCooldown);
        dashReady = true;
        yield break;
    }

    IEnumerator BulletTimeTimer()
    {
        yield return new WaitForSecondsRealtime(bulletTime);
        yield return new WaitForSecondsRealtime(bulletTimeCooldown);
        bulletTimeReady = true;
        yield break;
    }
}
