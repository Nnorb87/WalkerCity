using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour{

    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected float fireRate = 0.2f;
    [SerializeField] protected int weaponDamagemin = 5;
    [SerializeField] protected int weaponDamagemax = 10;
    [SerializeField] protected float critChance = 0.1f;

    protected Transform parent;
    protected Transform spawnPoint;
    protected bool shootingPressed = false;
    protected float timer = 0f;
    protected int ammo;
    protected int weaponDamage = 10;
    protected int critMultiplier = 2;



    protected virtual void Start() {
        GameInput.Instance.OnShootPerformed += GameInput_OnShootPerformed;
        GameInput.Instance.OnShootCanceled += GameInput_OnShootCanceled;
        parent = Player.Instance.weaponSlot;
        spawnPoint = Player.Instance.gunPoint;
    }

    protected virtual void Update() {
        Shoot();
    }


    protected void GameInput_OnShootCanceled(object sender, System.EventArgs e) {
        shootingPressed = false;
    }

    protected void GameInput_OnShootPerformed(object sender, System.EventArgs e) {
        shootingPressed = true;
    }

    protected void Shoot() {
        if (shootingPressed) {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                Instantiate(weaponSO.weaponBulletPrefab, spawnPoint.position, spawnPoint.rotation);
                timer = fireRate;

                Ray ray = new Ray(spawnPoint.position, spawnPoint.forward);
                float raycastDistance = 100f;
                float rayDebugDuration = 1f;
                LayerMask layermaskForRaycast = LayerMask.GetMask("Enemy");

                if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance,layermaskForRaycast)) {
                    // ray did hit someting
                    Debug.DrawRay(spawnPoint.position, spawnPoint.forward * raycastDistance, Color.green, rayDebugDuration);

                    if (hitInfo.collider.gameObject.TryGetComponent<EnemyHpController>(out EnemyHpController hpController)) {
                        weaponDamage = Random.Range(weaponDamagemin, weaponDamagemax);
                        if (Random.Range(1,100) <= critChance * 100) {
                            weaponDamage = weaponDamage * critMultiplier;
                            hpController.SetCritDamage(true);
                        } else {
                            hpController.SetCritDamage(false);
                        }
                        hpController.SetDamage(weaponDamage);
                        hpController.Damage(weaponDamage);
                    }
                }
                else {
                    // ray did not hit anything
                    Debug.DrawRay(spawnPoint.position, spawnPoint.forward * raycastDistance, Color.red,rayDebugDuration);
                }
            }
        }
    }


    public virtual int GetAmmo() {
        return ammo;
    }

    public virtual void SetAmmo(int ammo) {
        this.ammo = ammo;
    }

    public void SetSpawnPoint(Transform spawnPoint) {
        this.spawnPoint = spawnPoint;
    }

    public WeaponSO GetWeaponSO() {
        return weaponSO;
    }
}
