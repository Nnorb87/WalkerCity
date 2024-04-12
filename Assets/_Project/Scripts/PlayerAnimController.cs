using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour{

    private const string VELOCITY_X = "VelocityX";
    private const string VELOCITY_Z = "VelocityZ";

    private Rigidbody rb;
    private Animator animator;
    private float playerMoveSpeed = 1;



    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        rb = Player.Instance.GetComponent<Rigidbody>();
        playerMoveSpeed = Player.Instance.GetMoveSpeed();
    }


    private void Update() {
        // Get the local velocity relative to the player's position
        //Vector3 localVelocity = Player.Instance.transform.InverseTransformDirection(rb.velocity);
        Vector3 localVelocity = Player.Instance.transform.InverseTransformDirection(Player.Instance.GetVelocity());
        // Set animatior values
        animator.SetFloat(VELOCITY_X, localVelocity.x / playerMoveSpeed);
        animator.SetFloat(VELOCITY_Z, localVelocity.z / playerMoveSpeed);
    }
}
