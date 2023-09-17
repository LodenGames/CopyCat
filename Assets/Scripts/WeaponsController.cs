using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] Transform weaponSpawnPoint;
    [SerializeField] GameObject test;

    void Start() {

    }

    void Update() {
        
    }

    public void ChangeWeapons(int weapon) {
        // Need to reference CardCopier for weapons or split that into a class that holds the weapons?
        Instantiate(test, weaponSpawnPoint.position, weaponSpawnPoint.rotation, weaponSpawnPoint);
    }
}
