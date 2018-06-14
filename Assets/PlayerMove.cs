using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMove : MonoBehaviour
{

    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public float slopeLimit = 45;
    private bool grounded = false;
    private Rigidbody rB;
    private CapsuleCollider capsule;
    private Collision hitPoint;
    private Vector3 reflect;
    private float reflectSpeed = 1.1f;
    public float antiBumpFactor = 0.75f;



    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        rB = GetComponent<Rigidbody>();
        rB.freezeRotation = true;
        rB.useGravity = false;
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (IsGrounded())
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(inputX, -antiBumpFactor, inputY);
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rB.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rB.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButtonDown("Jump"))
            {
                rB.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            }
        }

        // We apply gravity manually for more tuning control
        rB.AddForce(new Vector3(0, -gravity * rB.mass, 0));

        //grounded = false;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        float rayDistance = (capsule.height / 2)+ 0.2f;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
        {
            if (Vector3.Angle(hit.normal, Vector3.up) < slopeLimit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else return false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        hitPoint = collision;
        //Debug.DrawRay(hitPoint.contacts[0].point, Vector3.Reflect(-hitPoint.relativeVelocity, hitPoint.contacts[0].normal), Color.green, 1000f);
        //if (!IsGrounded())
        //{

        //        rB.AddForce(Vector3.Reflect(-hitPoint.relativeVelocity, hitPoint.contacts[0].normal) * reflectSpeed, ForceMode.VelocityChange);

        //        //reflect = Vector3.Reflect(rB.velocity, hitPoint.normal);
        //        Debug.DrawRay(hitPoint.contacts[0].point, Vector3.Reflect(-hitPoint.relativeVelocity, hitPoint.contacts[0].normal), Color.red, 1000f);
        //        //rB.AddForce(reflect, ForceMode.Impulse);
        //        ////rB.velocity = new Vector3(rB.velocity.x, CalculateJumpVerticalSpeed(), rB.velocity.z);

        //}

    }

    void OnCollisionStay()
    {
        if (!IsGrounded())
        {
            //rB.velocity = Vector3.zero;
            if (canJump && Input.GetButtonDown("Jump"))
            {
                rB.AddForce(Vector3.Reflect(-hitPoint.relativeVelocity, hitPoint.contacts[0].normal) * reflectSpeed, ForceMode.VelocityChange);
            }
            //reflect = Vector3.Reflect(rB.velocity, hitPoint.normal);
            Debug.DrawRay(hitPoint.contacts[0].point, Vector3.Reflect(-hitPoint.relativeVelocity, hitPoint.contacts[0].normal), Color.red, 1000f);
            //rB.AddForce(reflect, ForceMode.Impulse);
            ////rB.velocity = new Vector3(rB.velocity.x, CalculateJumpVerticalSpeed(), rB.velocity.z);

        }
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}
