using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMoveV3 : MonoBehaviour
{

    public float speed = 8.0f;
    public float gravity = 15.0f;
    public float maxVelocityChange = 10.0f;
    public float antiBumpFactor = 0.6f;
    public float slopeLimit = 45;
    public float jumpHeight = 2.0f;
    public bool grounded = false;
    private Rigidbody rB;
    private CapsuleCollider capsule;
    public float jetpackThrust = 100;
    public float maxJetpackFuel = 100;
    public float jetFuelRegenRate = 1f;
    public float jetFuelDegenRate = 2f;
    public float jetFuel;
    float regenCooldown;
    float cooldown;

    float inputX;
    float inputY;

    public bool dash;
    public float dashForce = 500f;
    public float dashLength = 0.1f;
    private bool normalJumpDown;
    public float dashCooldown = 3f;
    private bool dashReady;

    bool falling;
    float maxFall;
    public float fallDamageThreshold;

    void Start()
    {
        jetFuel = maxJetpackFuel;
        capsule = GetComponent<CapsuleCollider>();
        rB = GetComponent<Rigidbody>();
        rB.freezeRotation = true;
        rB.useGravity = false;
        dashReady = true;
        Fabric.EventManager.Instance.PostEvent("Player/Footsteps", Fabric.EventAction.PlaySound, gameObject);
        Fabric.EventManager.Instance.PostEvent("Player/Jetpack/UsingLvl1", Fabric.EventAction.PlaySound, gameObject);
        Fabric.EventManager.Instance.PostEvent("Player/Jetpack/UsingLvl1", Fabric.EventAction.PauseSound, gameObject);
        //StartCoroutine(JetFuelRegen());
        //StartCoroutine(JetFuelDepletion());
    }

    private void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        //print(rB.velocity.sqrMagnitude);
        if (inputY != 0 && grounded && rB.velocity.sqrMagnitude > 5 || inputX != 0 && grounded && rB.velocity.sqrMagnitude > 5)
        {
            Fabric.EventManager.Instance.PostEvent("Player/Footsteps", Fabric.EventAction.UnpauseSound, gameObject);
        }
        else
        {
            Fabric.EventManager.Instance.PostEvent("Player/Footsteps", Fabric.EventAction.PauseSound, gameObject);
        }

    }


    void FixedUpdate()
    {
        if (dash)
        {
            return;
        }

        if (Input.GetButtonDown("Run") && !dash && dashReady)
        {
            dash = true;
            dashReady = false;
            rB.velocity = Vector3.zero;
            StartCoroutine(DashTime());
        }

        grounded = IsGrounded();

        if (grounded)
        {
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
            if (Input.GetButtonDown("Jump"))
            {
                rB.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed() + antiBumpFactor, velocity.z);
                //Fabric.EventManager.Instance.PostEvent("Player/Jetpack/StartLvl1", Fabric.EventAction.PlaySound, gameObject);
                //print("here");
            }
        }

        else
        {
            if (Input.GetButton("Jump") && jetFuel > 0)
            {
                Fabric.EventManager.Instance.PostEvent("Player/Jetpack/UsingLvl1", Fabric.EventAction.UnpauseSound, gameObject);

                // Calculate how fast we should be moving
                Vector3 targetVelocity = new Vector3(inputX * 0.8f, 0.8f, inputY * 0.8f);
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= speed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rB.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
                //rB.AddForce(new Vector3(velocityChange.x, 0, velocityChange.z), ForceMode.VelocityChange);
                rB.AddForce(velocityChange, ForceMode.VelocityChange);

                //rB.AddForce(new Vector3(0, jetpackThrust, 0), ForceMode.Acceleration);
                //rB.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed() + antiBumpFactor, velocity.z);

                if (cooldown > 0.1)
                {
                    jetFuel -= jetFuelDegenRate;
                    jetFuel = Mathf.Clamp(jetFuel, 0, maxJetpackFuel);
                    cooldown = 0;
                }
                cooldown += Time.fixedDeltaTime;
                regenCooldown = -2;
            }

            else
            {
                Fabric.EventManager.Instance.PostEvent("Player/Jetpack/UsingLvl1", Fabric.EventAction.PauseSound, gameObject);
                Vector3 targetVelocity = new Vector3(inputX, 0, inputY);
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= speed/1.5f;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rB.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = -antiBumpFactor;
                rB.AddForce(new Vector3(velocityChange.x, 0, velocityChange.z), ForceMode.VelocityChange);
            }

        }

        if (regenCooldown > 0.1)
        {
            jetFuel += jetFuelRegenRate;
            jetFuel = Mathf.Clamp(jetFuel, 0, maxJetpackFuel);
            regenCooldown = 0;
        }
        regenCooldown += Time.fixedDeltaTime;

        //print(rB.velocity.y);
        if (rB.velocity.y > 10)
        {
            rB.velocity = new Vector3(rB.velocity.x, 10, rB.velocity.z);
        }
        if (rB.velocity.y < -20)
        {
            rB.velocity = new Vector3(rB.velocity.x, -20, rB.velocity.z);
        }

        // We apply gravity manually for more tuning control
        rB.AddForce(new Vector3(0, -gravity * rB.mass, 0));

        if (rB.velocity.y < -0.1 && !dash)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }


        if (falling && grounded)
        {
            if (rB.velocity.y < -fallDamageThreshold)
            {
                GameManager.instance.DamagePlayer(20);
                Fabric.EventManager.Instance.PostEvent("Player/Hit", Fabric.EventAction.PlaySound, gameObject);
            }
            falling = false;
        }

        //print(falling +" : "+ grounded + " : " + maxFall);

    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        float rayDistance = (capsule.height / 2) - (capsule.radius - 0.2f);
        if (Physics.SphereCast(transform.position, capsule.radius-0.01f, -Vector3.up, out hit, rayDistance))
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


    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    IEnumerator DashTime()
    {
        //dashTrail.Play();
        //dashCameraEffect.Play();
        rB.AddForce(Camera.main.transform.forward * dashForce, ForceMode.Impulse);
        Fabric.EventManager.Instance.PostEvent("Player/Teleport/Lvl5", Fabric.EventAction.PlaySound, gameObject);
        yield return new WaitForSeconds(dashLength);
        rB.velocity = Vector3.zero;
        dash = false;
        //dashTrail.Stop();
        //dashCameraEffect.Stop();
        yield return new WaitForSecondsRealtime(dashCooldown);
        dashReady = true;
        yield break;
    }
}

