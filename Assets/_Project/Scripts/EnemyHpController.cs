using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpController : MonoBehaviour{

    public event EventHandler OnEnemyHealthChange;
    public event EventHandler OnEnemyDeath;

    [SerializeField] int enemyHealth = 100;
    private int incomingDamage;
    private bool critDamage = false;
    private bool dead = false;

    public void Damage(int damage) {

        if (enemyHealth > 0 ) {

            if (enemyHealth > damage) {
                enemyHealth -= damage;
            } else {
                enemyHealth = 0;
            }
        }
        
        OnEnemyHealthChange?.Invoke(this, EventArgs.Empty);

        if (enemyHealth <= 0 && !dead) {
            EnemyDeath();
            dead = true;
        }
    }

    private void EnemyDeath() {
        StartCoroutine(IEEnemyDeath());
    }

    private float dyingTime = 10f;
    private IEnumerator IEEnemyDeath() {
        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(dyingTime);
        Destroy(gameObject);
    }

    public int GetHealth() { 
        return enemyHealth; 
    }

    public int GetDamage() { 
        return incomingDamage; 
    }
    public void SetDamage(int damage) {
        incomingDamage = damage;
    }
    public bool GetCritDamage() { 
        return critDamage; 
    }
    public void SetCritDamage(bool critDamage) {
        this.critDamage = critDamage;
    }
}
