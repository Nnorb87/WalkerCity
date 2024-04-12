using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject gameOverUI;


    private void Awake() {
        Hide();
    }

    private void Start() {
        

        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

        mainMenuButton.onClick.AddListener(() => {

            Loader.Load(Loader.Scene.MainMenuScene);

        });

        Hide();
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e) {
        Show();
        mainMenuButton.Select();
        Time.timeScale = 0f;
    }

    private void Show() {
        gameOverUI.SetActive(true);
    }
    private void Hide() {
        gameOverUI.SetActive(false);

    }
}
