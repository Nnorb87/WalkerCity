using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour{

    [SerializeField] private Transform[] patrolPointsTransformArr;
    private Transform[] patrolPointsVisualArr;

    private void Start() {
        patrolPointsVisualArr = new Transform[patrolPointsTransformArr.Length];

        for (int i = 0; i < patrolPointsTransformArr.Length; i++) {
            if (patrolPointsTransformArr[i] != null && patrolPointsTransformArr[i].childCount > 0) {
                patrolPointsVisualArr[i] = patrolPointsTransformArr[i].GetChild(0);
            } else {
                Debug.LogError("No child object found for patrol point " + i);
            }
        }

        if (DebugController.Instance.ShowPathpoints()) {
            foreach (Transform patrolPointVisual in patrolPointsVisualArr) {
                patrolPointVisual.gameObject.SetActive(true);
            }
        } else {
            foreach (Transform patrolPointVisual in patrolPointsVisualArr) {
                patrolPointVisual.gameObject.SetActive(false);
            }
        }
    }

    public Transform[] GetPatrolPoints() {
        return patrolPointsTransformArr;
    }
}
