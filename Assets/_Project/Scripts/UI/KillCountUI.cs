using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCountUI : MonoBehaviour{

    [SerializeField] TextMeshProUGUI killCountText;

    private void Start() {
        killCountText.text = GameManager.Instance.GetKillCount().ToString();
        GameManager.Instance.OnKillCountChange += Gamemanager_OnKillCountChange;
    }

    private void Gamemanager_OnKillCountChange(object sender, GameManager.OnKillCountChangeEventArgs e) {
        killCountText.text = GameManager.Instance.GetKillCount().ToString();
    }
}
