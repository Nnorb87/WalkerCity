using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup_Weapon : Pickup {

    [SerializeField] private WeaponSO weaponSO;
    private Transform parent;
    private Transform spawnPoint;
    private WeaponSO playerWeaponSO;

    protected override void Start() {
        base.Start();
        parent = Player.Instance.weaponSlot;
        spawnPoint = Player.Instance.gunPoint;
    }
    protected override void PickupAction() {

        if (Player.Instance.HasWeapon()) {
            playerWeaponSO = Player.Instance.GetWeaponSO();
            Player.Instance.DestroyWeapon();
            SpawnPlayerWeapon();
            SpawnPickup();
        } else {
            Player.Instance.DestroyWeapon();
            SpawnPlayerWeapon();
        }
    }

    private void SpawnPlayerWeapon() {
        GameObject spawnedObject = Instantiate(weaponSO.weaponPrefab, parent);
        spawnedObject.TryGetComponent<Weapon>(out Weapon weaponComponent);
        weaponComponent.SetSpawnPoint(spawnPoint);
    }

    private void SpawnPickup() {
        GameObject spawnedObject = Instantiate(playerWeaponSO.weaponPickup);
        spawnedObject.transform.position = Player.Instance.weaponDropPoint.position;
    }
}
