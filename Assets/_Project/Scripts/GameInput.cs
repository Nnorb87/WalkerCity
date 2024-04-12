using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInput : MonoBehaviour{

    public static GameInput Instance { get; private set; }

    public event EventHandler OnShootPerformed;
    public event EventHandler OnShootCanceled;
    public event EventHandler OnInteract;
    public event EventHandler OnPausePerformed;

    private PlayerInputActions playerInputActions;
    private Vector2 movementVector;
    private Vector2 rotationVector;


    private void Awake() {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += Shoot_performed;
        playerInputActions.Player.Shoot.canceled += Shoot_canceled;
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputActions.Player.Shoot.performed -= Shoot_performed;
        playerInputActions.Player.Shoot.canceled -= Shoot_canceled;
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();

    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPausePerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteract?.Invoke(this,EventArgs.Empty);
    }

    private void Shoot_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnShootCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnShootPerformed?.Invoke(this, new EventArgs());

    }

    public Vector3 GetMovementVector() {
        movementVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return new Vector3(movementVector.x, 0f, movementVector.y);
    }

    public Vector3 GetRotationVector() {
        rotationVector = playerInputActions.Player.Rotation.ReadValue<Vector2>();
        return new Vector3(rotationVector.x, 0f, rotationVector.y);
    }


}
