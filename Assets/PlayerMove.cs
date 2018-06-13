using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMove : MonoBehaviour {

    private Rigidbody rB;

    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;

    public bool enableRunning = true;

    // If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
    private bool limitDiagonalSpeed = true;

    // Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
    public float antiBumpFactor = .75f;

    private float speed;
    private Vector3 moveDirection;

    // Use this for initialization
    void Start () {
        rB = GetComponent<Rigidbody>();
        speed = walkSpeed;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;

        speed = Input.GetButton("Fire3") ? runSpeed : walkSpeed;

        moveDirection = new Vector3(inputX * inputModifyFactor, 0, inputY * inputModifyFactor);
        moveDirection = transform.TransformDirection(moveDirection);

        rB.velocity = moveDirection * speed * Time.deltaTime;
    }

}
