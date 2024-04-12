using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour{

    private Renderer meshRenderer;

    public event TriggerDelegate OnEnemyTriggerEnter;
    public delegate void TriggerDelegate(Player player);

    public event EventHandler OnEnemyTriggerExit;

    private void Start() {
        meshRenderer = GetComponent<Renderer>();
        if (DebugController.Instance.ShowTriggers()) {
            meshRenderer.enabled = true;
        } else {
            meshRenderer.enabled = false;
        }
    }


    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.TryGetComponent<Player>(out Player player)){
            OnEnemyTriggerEnter?.Invoke(player);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.TryGetComponent<Player>(out Player player)) {
            OnEnemyTriggerExit?.Invoke(this, EventArgs.Empty);
        }
        
    }

}
