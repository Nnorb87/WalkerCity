using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour {

    [SerializeField] private EnemyTrigger enemyTrigger;
    [SerializeField] private float playerPositionCheckFrequency= 0.5f;
    [SerializeField] private float attackDistance = 2.5f;
    [SerializeField] private float defaultSearchtime = 30f;

    private NavMeshAgent navMeshAgent;
    private Player player;
    private Coroutine FollowPlayerCoroutine;
    private State state;
    private float distanceFromPlayer;
    private EnemyHpController enemyHpController;
    private State lastState;
    private float searchTimer;
    private Transform guardPoint;
    private Quaternion rightTargetVector;
    private Quaternion leftTargetVector;
    private bool isLookingRight = true;
    private float rotationTimer = 0f;
    private float waitTimeForLookaround = 2f;
    private bool waiting = false;
    private Transform[] patrolPoints;
    private Vector3 playerLostPosition;
    private Vector3 searchTargetPosition;
    private State defaultBehaviour = State.Idle;
    private Coroutine searchCR;
    private Collider enemyCollider;

    public enum State {
        Idle,
        Guard,
        Patrol,
        Follow,
        Attack,
        Search,
        Retreat,
        Dead
    }

    private void Awake() {
        enemyCollider = GetComponent<Collider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHpController = GetComponent<EnemyHpController>();
        state = State.Idle;
    }

    private void Start() {
        enemyTrigger.OnEnemyTriggerEnter += EnemyTrigger_OnEnemyTriggerEnter;
        enemyTrigger.OnEnemyTriggerExit += EnemyTrigger_OnEnemyTriggerExit;
        enemyHpController.OnEnemyDeath += EnemyHpController_OnEnemyDeath;
    }

    private void EnemyHpController_OnEnemyDeath(object sender, System.EventArgs e) {
        if (GameManager.Instance != null) {
            GameManager.Instance.SetKillCount(1);
        }
        state = State.Dead;
    }
    private void EnemyTrigger_OnEnemyTriggerEnter(Player player) {
        FollowPlayerCoroutine = StartCoroutine(IEFollowPlayer());
        state = State.Follow;
        this.player = player;
        
    }

    private void EnemyTrigger_OnEnemyTriggerExit(object sender, System.EventArgs e) {
        StartCoroutine(IESampleSearchPositions());
        searchTimer = defaultSearchtime;
        state = State.Search;
        
    }

    private void Update() {

        switch (state) {
            default:
            case State.Idle:
                break;
            case State.Guard:
                if (guardPoint != null) {

                    if (navMeshAgent.velocity.magnitude < 0.1f && (guardPoint.position - transform.position).magnitude > 1f) {
                        navMeshAgent.SetDestination(guardPoint.position);
                    }
                    GuardRotation();
                }
                break;
            case State.Patrol:
                if (!patrolWait && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                    StartCoroutine(IEPatrol());
                }
                break;
            case State.Follow:
                break;
            case State.Attack:
                RotateTowardPlayer();
                distanceFromPlayer = (player.transform.position - transform.position).magnitude;
                if (distanceFromPlayer > attackDistance) {
                    state = State.Follow;
                }
                break;
            case State.Search:
                if (FollowPlayerCoroutine != null) {
                    StopCoroutine(FollowPlayerCoroutine);
                }
                if (!waitSearch && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                    searchCR = StartCoroutine(IESearch(new Vector3[] { playerLostPosition, searchTargetPosition }));
                }
                searchTimer -= Time.deltaTime;
                //Debug.Log(searchTimer);

                if (searchTimer < 0f) { 
                    searchTimer = defaultSearchtime;
                    StopCoroutine(searchCR);
                    state = defaultBehaviour;
                }
                break;
            case State.Retreat:
                break;
            case State.Dead:
                if (FollowPlayerCoroutine != null) {
                    StopCoroutine(FollowPlayerCoroutine);
                }
                if (searchCR != null) {
                    StopCoroutine(searchCR);
                }
                enemyCollider.enabled = false;
                enemyTrigger.gameObject.SetActive(false);
                navMeshAgent.enabled = false;
                break;
                }
        if (lastState != state) {
            Debug.Log(state);
        }
        lastState = state;
    }


    private IEnumerator IEFollowPlayer() {
        while (true) {
            if (player != null && navMeshAgent.isActiveAndEnabled) {
                navMeshAgent.SetDestination(player.transform.position);
                distanceFromPlayer = (player.transform.position - transform.position).magnitude;
                if (distanceFromPlayer <= attackDistance) {
                    state = State.Attack;
                }
            }
            yield return new WaitForSeconds(playerPositionCheckFrequency);
            }
    }

    public State GetState() {
        return state;
    }
    public void SetGuardPoint(Transform guardPoint) {

        this.guardPoint = guardPoint;
        rightTargetVector =  guardPoint.rotation * Quaternion.Euler(0, 90, 0);
        leftTargetVector = guardPoint.rotation * Quaternion.Euler(0, -90, 0);
    }

    public void SetState(State state) {
        this.state = state;
    }

    private void RotateTowardPlayer() {
        float rotationSpeed = 5f;

    // Calculate the direction from the current object's position to the player's position
    Vector3 directionToPlayer = Player.Instance.transform.position - transform.position;

    // Calculate the rotation to face the player using LookRotation
    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

    // Smoothly rotate towards the target rotation
    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed* Time.deltaTime);
    
    }

    private void GuardRotation() {
        float rotationSpeed = 100f;

        Quaternion targetRotation = isLookingRight ? rightTargetVector : leftTargetVector;

        if (rotationTimer <= 0f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            waiting = false;
        } else {
            rotationTimer -= Time.deltaTime;
        }

        if (Quaternion.Angle(transform.rotation, rightTargetVector) < 1.0f && !waiting) {
            isLookingRight = false;
            rotationTimer = waitTimeForLookaround;
            waiting = true;
        }

        if (Quaternion.Angle(transform.rotation, leftTargetVector) < 1.0f && !waiting) {
            isLookingRight = true;
            rotationTimer = waitTimeForLookaround;
            waiting = true;
        }

    }

    public void SetPatrolPoints(Transform[] patrolPoints) {
        this.patrolPoints = patrolPoints;
    }

    float patrolWaitTime = 1f;
    bool patrolWait = false;
    int patrolIndex = 0;
    Vector3 navMeshSpawnPosition;
    private IEnumerator IEPatrol() {
        patrolWait = true;
        yield return new WaitForSeconds(patrolWaitTime);

        float maxSamplingDistance = 1f;
        if (NavMesh.SamplePosition(patrolPoints[patrolIndex].position, out NavMeshHit navMeshHit, maxSamplingDistance, NavMesh.AllAreas)) {
            navMeshSpawnPosition = navMeshHit.position;
        } else {
            Debug.Log("Cant find the closest position on the navMesh");
        }
        if (navMeshAgent != null) {
            navMeshAgent.SetDestination(navMeshSpawnPosition);
        }

        patrolIndex++;
        if (patrolIndex == patrolPoints.Length) {
            patrolIndex = 0;
        }
        patrolWait = false;
    }

    float searchWaitTime = 1f;
    bool waitSearch = false;
    int searchIndex = 0;
    private IEnumerator IESearch(Vector3[] searchPoints) {
        waitSearch = true;
        yield return new WaitForSeconds(searchWaitTime);
        if (navMeshAgent != null) {
            navMeshAgent.SetDestination(searchPoints[searchIndex]);
        }
        searchIndex++;
        if (searchIndex == searchPoints.Length) {
            searchIndex = 0;
        }
        waitSearch = false;
    }

    float sampleRateTimer = 2f;
    private IEnumerator IESampleSearchPositions() {
        playerLostPosition = Player.Instance.transform.position;
        yield return new WaitForSeconds(sampleRateTimer);
        searchTargetPosition = Player.Instance.transform.position;
    }
    private void OnDrawGizmos() {
        if (DebugController.Instance != null && DebugController.Instance.ShowSearchPoints()) {
            float sphereRadius = 0.2f;
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawSphere(playerLostPosition, sphereRadius);
            Gizmos.DrawSphere(searchTargetPosition, sphereRadius);
        }
    }

    public void SetDefaultBehaviour(State defaultBehaviour) {
        this.defaultBehaviour = defaultBehaviour;
    }

}
