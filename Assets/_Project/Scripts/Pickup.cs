using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pickup : MonoBehaviour {

    protected PickupTrigger pickupTrigger;
    protected bool isPlayerInTrigger = false;
    protected Collider objectColliderInTrigger;

    protected virtual void Awake() {
        // get reference for pickupTrigger
        foreach (Transform transform in transform) {
            if (transform.gameObject.TryGetComponent<PickupTrigger>(out PickupTrigger pickupTrigger)) {
                this.pickupTrigger = pickupTrigger;
            }
        }
      
    }
    protected virtual void Start() {
        // listen for the trigger
        pickupTrigger.OnPickupTriggerEnter += PickupTrigger_OnPickupTriggerEnter;
        pickupTrigger.OnPickupTriggerExit += PickupTrigger_OnPickupTriggerExit;
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e) {
        // check if the player pressing interact
        if (objectColliderInTrigger != null) {
            if (objectColliderInTrigger.gameObject.TryGetComponent<Player>(out Player player)) {
                // execute custom actions
                if (isPlayerInTrigger) {
                    PickupAction();
                    Cleanup();
                    isPlayerInTrigger = false;
                }
            }
                  
        }
    }

    private void PickupTrigger_OnPickupTriggerExit(object sender, PickupTrigger.OnPickupTriggerEventArgs e) {
        objectColliderInTrigger = null;
        isPlayerInTrigger = false;
    }

    protected virtual void PickupTrigger_OnPickupTriggerEnter(object sender, PickupTrigger.OnPickupTriggerEventArgs e) {
        objectColliderInTrigger = e.collider;
        isPlayerInTrigger = true;
   
    }

    protected virtual void PickupAction() {
        // custom pickup behaviour
    }

    protected virtual void Cleanup() {
        // Unsubscribe and Destroy  
        pickupTrigger.OnPickupTriggerEnter -= PickupTrigger_OnPickupTriggerEnter;
        if (gameObject != null) {
            Destroy(gameObject);
        }
    }
}
