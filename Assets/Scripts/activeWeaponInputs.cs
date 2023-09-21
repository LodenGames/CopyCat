using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeWeaponInputs : MonoBehaviour {

    DoorOpener dooropener;
    CardCopier cardCopier;
    WeaponsController weaponsController;
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        dooropener = GetComponent<DoorOpener>();
        weaponsController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WeaponsController>();
        cardCopier = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CardCopier>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            switch (weaponsController.activeWeapon.spawnable.tag) {
                case "Hammer":
                    if (dooropener.canMoveDoor && dooropener.vent) {
                        dooropener.EndGame();
                    }
                    break;
                case "Key":
                    if (dooropener.canMoveDoor && !dooropener.vent) {
                        // Play audio
                        audioSource.PlayOneShot(audioSource.clip);
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
