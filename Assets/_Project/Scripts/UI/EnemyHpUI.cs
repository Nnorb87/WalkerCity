using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour{

    [SerializeField] EnemyHpController EnemyHpController;
    [SerializeField] private Image barImage;
    private float maxHP;


    private void Start() {
        maxHP = EnemyHpController.GetHealth();
        barImage.fillAmount = maxHP;
        Show();
        
        EnemyHpController.OnEnemyHealthChange += EnemyHpController_OnEnemyHealthChange;
        EnemyHpController.OnEnemyDeath += EnemyHpController_OnEnemyDeath;
    }

    private void EnemyHpController_OnEnemyDeath(object sender, System.EventArgs e) {
        Hide();
    }

    private void EnemyHpController_OnEnemyHealthChange(object sender, System.EventArgs e) {
        barImage.fillAmount = EnemyHpController.GetHealth() /maxHP;
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

}
