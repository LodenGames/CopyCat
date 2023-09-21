using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeWeaponInputs : MonoBehaviour {

    DoorOpener dooropener;
    CardCopier cardCopier;
    WeaponsController weaponsController;
    AudioSource audioSource;
    [SerializeField] GameObject escapeCanvas;
    FPSController fpsController;

    public bool gameIsPaused;

    void Start() {
        gameIsPaused = false;
        fpsController = GetComponent<FPSController>();
        escapeCanvas.SetActive(false);
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
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (!gameIsPaused) {
                // puase game
                //Time.timeScale = 0f;
                escapeCanvas.SetActive(true);
                gameIsPaused = true;
                fpsController.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            } else {
                // resume game
                Time.timeScale = 1.0f;
                escapeCanvas.SetActive(false);
                gameIsPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                fpsController.enabled = true;
            }
        }
    }
}
