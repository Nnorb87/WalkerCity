using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class SpawnButton : MonoBehaviour{

    [SerializeField] private Renderer buttonRenderer;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnLocation;
    private Vector3 navMeshSpawnPosition;

    private bool triggerStay = false;

    private void Start() {
        buttonRenderer.material.color = Color.blue;
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e) {
        if (triggerStay) {
            float maxSamplingDistance = 1f;
            if (NavMesh.SamplePosition(spawnLocation.position, out NavMeshHit navMeshHit, maxSamplingDistance, NavMesh.AllAreas)) {
                navMeshSpawnPosition = navMeshHit.position;
            } else {
                Debug.Log("Cant find the closest position on the navMesh");
            }
            Instantiate(enemyPrefab, navMeshSpawnPosition, Quaternion.identity) ;
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.TryGetComponent<Player>(out Player player)) {
            if (buttonRenderer != null) {
                buttonRenderer.material.color = Color.green;
                triggerStay = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {  
        if (buttonRenderer != null) {
            buttonRenderer.material.color = Color.blue;
            triggerStay = false;
        } }

}
