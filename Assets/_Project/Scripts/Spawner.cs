using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour{

    [SerializeField] private GameObject guardPointGO;
    [SerializeField] private GameObject patrolPointsGO;
    [SerializeField] private GameObject spawnerVisual;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private bool guard;
    [SerializeField] private bool patrol;
    private Vector3 navMeshSpawnPosition;

    private Transform guardPoint;
    private Transform[] patrolPoints;

    private void Start() {
        guardPoint = guardPointGO.transform;

        if (patrolPointsGO.TryGetComponent<PatrolPoints>(out PatrolPoints patrolPoints)){
            this.patrolPoints = patrolPoints.GetPatrolPoints();
        }

        // show visual representation of patrol/guard points
        if (DebugController.Instance.ShowPathpoints()) {
            spawnerVisual.gameObject.SetActive(true);
        } else {
            spawnerVisual.gameObject.SetActive(false);
        }

        // spawn Enemy
        float maxSamplingDistance = 1f;
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit navMeshHit, maxSamplingDistance, NavMesh.AllAreas)) {
            navMeshSpawnPosition = navMeshHit.position;
        } else {
            Debug.Log("Cant find the closest position on the navMesh");
        }
        GameObject spawnedEnemy = Instantiate(enemyPrefab, navMeshSpawnPosition,Quaternion.identity);

        // inject path data
        spawnedEnemy.GetComponent<EnemyAI>().SetPatrolPoints(this.patrolPoints);
        spawnedEnemy.GetComponent<EnemyAI>().SetGuardPoint(this.guardPoint);
        //Debug.Log(this.guardPoint);

        // set state for spawned Enemy
        if (patrol) {
            spawnedEnemy.GetComponent<EnemyAI>().SetState(EnemyAI.State.Patrol);
            spawnedEnemy.GetComponent<EnemyAI>().SetDefaultBehaviour(EnemyAI.State.Patrol);
        } else if (guard) {
            spawnedEnemy.GetComponent<EnemyAI>().SetState(EnemyAI.State.Guard);
            spawnedEnemy.GetComponent<EnemyAI>().SetDefaultBehaviour(EnemyAI.State.Guard);
        } 
    }

}
