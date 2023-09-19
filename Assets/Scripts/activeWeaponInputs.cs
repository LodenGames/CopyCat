using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeWeaponInputs : MonoBehaviour {

    DoorOpener dooropener;
    WeaponsController weaponsController;

    void Start() {
        dooropener = GetComponent<DoorOpener>();
        weaponsController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WeaponsController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            switch (weaponsController.activeWeapon.spawnable.tag) {
                case "Key":
                    if (dooropener.canMoveDoor) {
                        dooropener.MoveDoor();
                    }
                    break;
                default:
                    Debug.Log("No active weapon");
                    break;
            }
        }
    }
}
