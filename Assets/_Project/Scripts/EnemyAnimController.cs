using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour{

    const string ENEMYMOVING = "EnemyMoving";
    const string ENEMYATTACK = "EnemyAttack";
    const string ENEMYDEAD = "EnemyDead";


    [SerializeField] private GameObject EnemyRoot;

    private Rigidbody EnemyRigidBody;
    private Animator EnemyAnimatorComponent;
    private Vector3 lastPosition;
    private Vector3 velocity;
    private EnemyAI EnemyAI;
    private EnemyHpController EnemyHpController;


   
    
    private void Start() {
        EnemyAnimatorComponent = GetComponent<Animator>();
        lastPosition = transform.position;
        EnemyRigidBody = EnemyRoot.GetComponent<Rigidbody>();
        EnemyAI = EnemyRoot.GetComponent<EnemyAI>();
        EnemyHpController = EnemyRoot.GetComponent<EnemyHpController>();
        EnemyHpController.OnEnemyDeath += EnemyHpController_OnEnemyDeath;

    }

    private void EnemyHpController_OnEnemyDeath(object sender, System.EventArgs e) {
        EnemyAnimatorComponent.SetBool(ENEMYDEAD, true);
    }

    private void Update() {
        velocity = CalculateVelocity();
        if (velocity.magnitude > 0) {
            EnemyAnimatorComponent.SetBool(ENEMYMOVING, true);
        } else {
            EnemyAnimatorComponent.SetBool(ENEMYMOVING, false);
        }

        if (EnemyAI.GetState() == EnemyAI.State.Attack) {
            EnemyAnimatorComponent.SetBool(ENEMYATTACK, true);
        } else {
            EnemyAnimatorComponent.SetBool(ENEMYATTACK, false);
        }
    }

    private Vector3 CalculateVelocity() {
        Vector3 velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
        return velocity;
    }

}
