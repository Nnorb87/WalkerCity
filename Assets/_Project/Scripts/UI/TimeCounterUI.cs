using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCounterUI : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI timeCounterText;

    private void Update() {
        timeCounterText.text = Mathf.CeilToInt(GameManager.Instance.GetGameTime()).ToString();
    }
}
