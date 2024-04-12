using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour{

    public static Player Instance { get; private set; } 

    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector3 rotationAngle = new Vector3(0,100,0);
    [SerializeField] public Transform weaponSlot;
    [SerializeField] public Transform gunPoint;
    [SerializeField] public Transform weaponDropPoint;

    private Vector3 lastPosition;
    private Vector3 velocity;
    private Rigidbody rb;
    private Vector3 movementVector;
    private Vector3 rotationVector;

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        lastPosition = transform.position;
    }

    private void FixedUpdate() {
        velocity = CalculateVelocity();
        movementVector = GameInput.Instance.GetMovementVector();
        rotationVector = GameInput.Instance.GetRotationVector();
        HandleMovement(movementVector, moveSpeed);
        HandleRotation(movementVector, rotationVector, rotationSpeed);
    }
    private void HandleMovement(Vector3 movementVector, float moveSpeed) {
        rb.MovePosition(transform.position + movementVector * moveSpeed * Time.deltaTime);
    }

    private void HandleRotation (Vector3 movementVector, Vector3 rotationVector, float rotationSpeed) {

        if (rotationVector != Vector3.zero) {
            Quaternion targetRotation = CalculateRotationAngle(rotationVector);
            // Rotate the player smoothly towards the target rotation using MoveRotation
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));

        } else if (movementVector != Vector3.zero) {
            Quaternion targetRotation = CalculateRotationAngle(movementVector);
            // Rotate the player smoothly towards the target rotation using MoveRotation
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }
     
    private Quaternion CalculateRotationAngle(Vector3 target) {
        // Calculate the target angle based on the input
        float targetAngle = Mathf.Atan2(target.x, target.z) * Mathf.Rad2Deg;
        // Create the target rotation based on the target angle
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        return targetRotation;
    }

    private Vector3 CalculateVelocity() {
        Vector3 velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
        return velocity;
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public Vector3 GetVelocity() {
        return velocity;
    }

    public bool HasWeapon() {
        if (weaponSlot.childCount == 0) {
            return false;
        } else {
            return true;
        }
    }

    public WeaponSO GetWeaponSO() {
        if (weaponSlot.childCount != 0) {
            WeaponSO GunWeaponSO = weaponSlot.GetChild(0).GetComponent<Weapon>().GetWeaponSO();
            return GunWeaponSO;
        } else { return null; }
    }

    public void DestroyWeapon() { 
    foreach(Transform child in weaponSlot) {
            if (child != null) {
                Destroy(child.gameObject);
            }
        }
    }

}
