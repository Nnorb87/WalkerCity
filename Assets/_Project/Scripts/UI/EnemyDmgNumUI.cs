using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDmgNumUI : MonoBehaviour{

    [SerializeField] private EnemyHpController hpController;
    [SerializeField] private TextMeshProUGUI normalDamageNumbers;
    [SerializeField] private TextMeshProUGUI criticalDamageNumbers;

    private float showDamageTimer = 0.2f;

    private void Awake() {
        normalDamageNumbers.gameObject.SetActive(false);
        criticalDamageNumbers.gameObject.SetActive(false);
    }

    private void Start() {
        hpController.OnEnemyHealthChange += HpController_OnEnemyHealthChange;
        hpController.OnEnemyDeath += HpController_OnEnemyDeath;
    }

    private void HpController_OnEnemyDeath(object sender, System.EventArgs e) {
        hpController.OnEnemyHealthChange -= HpController_OnEnemyHealthChange;
        hpController.OnEnemyDeath -= HpController_OnEnemyDeath;
        Destroy(gameObject);
    }

    private void HpController_OnEnemyHealthChange(object sender, System.EventArgs e) {
        if (hpController.GetCritDamage()) {
            StartCoroutine(FlashCriticalDamageNumber());
        } else {
            StartCoroutine(FlashNormalDamageNumber());
        }

    }

    private IEnumerator FlashNormalDamageNumber() {

        TextMeshProUGUI dmgGui = Instantiate(normalDamageNumbers,this.transform);       
        dmgGui.text = hpController.GetDamage().ToString();
        dmgGui.gameObject.SetActive(true);
        hpController.SetCritDamage(false);
        yield return new WaitForSeconds(showDamageTimer);
        Destroy(dmgGui);
    }

    private IEnumerator FlashCriticalDamageNumber() {
        TextMeshProUGUI dmgGui = Instantiate(criticalDamageNumbers, this.transform);
        dmgGui.text = hpController.GetDamage().ToString();
        dmgGui.gameObject.SetActive(true);
        yield return new WaitForSeconds(showDamageTimer);
        Destroy (dmgGui);
    }
}

