using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPoint : MonoBehaviour{

    [SerializeField] private GameObject guardPointVisual;
    private Transform guardPoint;

    private void Start() {

        if (DebugController.Instance.ShowPathpoints()) {
            guardPointVisual.SetActive(true);

        } else {
            guardPointVisual.SetActive(false);
        }
        guardPoint = this.transform;
    }

    public Transform GetGuardPoint() {
        return guardPoint;
    }

}
