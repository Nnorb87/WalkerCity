using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour {

    public event EventHandler<OnPickupTriggerEventArgs> OnPickupTriggerEnter;
    public event EventHandler<OnPickupTriggerEventArgs> OnPickupTriggerExit;

    public class OnPickupTriggerEventArgs : EventArgs {
       public Collider collider;
    }


    private void OnTriggerEnter(Collider collider) {
        OnPickupTriggerEnter?.Invoke(this, new OnPickupTriggerEventArgs {
            collider = collider
        });
    }

    private void OnTriggerExit(Collider collider) {
        OnPickupTriggerExit?.Invoke(this, new OnPickupTriggerEventArgs {
            collider = collider
        });
    }
}
