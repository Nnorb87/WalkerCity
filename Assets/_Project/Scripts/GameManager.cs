using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager Instance { get; private set; }
    public event EventHandler OnGameOver;
    public event EventHandler<OnKillCountChangeEventArgs> OnKillCountChange;
    public class OnKillCountChangeEventArgs : EventArgs {
        public int killCount;
    }

    [SerializeField] private int killCount = 10;
    [SerializeField] private float gameTime = 240f;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        gameTime -= Time.deltaTime;
        if (killCount == 0 || gameTime <= 0f) { 
            OnGameOver?.Invoke(this, EventArgs.Empty);
        }
    }


    public int GetKillCount() { 
        return killCount; 
    }

    public void SetKillCount(int killCount) {
        this.killCount = this.killCount - killCount;
        OnKillCountChange?.Invoke(this, new OnKillCountChangeEventArgs {
            killCount = this.killCount
        });
    }

    public float GetGameTime() {
        return gameTime;
    }

}
