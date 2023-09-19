using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeWeaponInputs : MonoBehaviour {

    DoorOpener dooropener;
    CardCopier cardCopier;
    WeaponsController weaponsController;

    void Start() {
        dooropener = GetComponent<DoorOpener>();
        weaponsController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WeaponsController>();
        cardCopier = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CardCopier>();
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
                    if (cardCopier.canPickUpPlayingCards && !cardCopier.playingCardsAreEnabled()) {
                        cardCopier.SetPlayingCardsToActive(cardCopier.hitSaved);
                        cardCopier.playingCardsEnabled = true;
                    }
                    Debug.Log("Nothing Equipped / no action");
                    break;
            }
        }
    }
}
