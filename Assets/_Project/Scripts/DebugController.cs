using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour{

    public static DebugController Instance { get; private set; }

    [SerializeField] private bool showPathpoints;
    [SerializeField] private bool showTriggers;
    [SerializeField] private bool showSearchPoints;

    private void Awake() {
        Instance = this;
    }

    public bool ShowPathpoints() {
        return showPathpoints; 
    }

    public bool ShowTriggers() {
        return showTriggers;
    }

    public bool ShowSearchPoints() {
        return showSearchPoints;
    }
}
