using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour{

    [SerializeField] float bulletSpeed = 1f;
    private float timer = 0f;
    private float killTimer = 3f;

    private void Update() {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        DestroyProjectile();
    }

    private void DestroyProjectile() {
        timer += Time.deltaTime;
        if (timer > killTimer) {
            Destroy(gameObject);
        }

    }

}
