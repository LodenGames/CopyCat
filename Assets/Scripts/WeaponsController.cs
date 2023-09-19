using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] Transform weaponSpawnPoint;
    CardCopier cardcopier;

    float pressDelayDuration;
    float timer;
    bool switchWeapon;
    public SpawnableObjects activeWeapon;
    public int activeWeaponCardNumber;

    void Start() {
        cardcopier = GetComponent<CardCopier>();
        pressDelayDuration = cardcopier.pressDelayDuration;
    }

    void Update() {
        EquipWeapon();
    }

    public bool WeaponIsActive() {
        return (activeWeapon.spawnable.tag == "Empty") ? false : true;
    }

    public void DestroyActiveWeapon() {
        GameObject newWeapon = Instantiate(cardcopier.spawnableObjects[0].spawnable, weaponSpawnPoint.position, weaponSpawnPoint.rotation, weaponSpawnPoint);
        Destroy(activeWeapon.spawnable);
        activeWeapon.spawnable = newWeapon;
    }

    public void ChangeWeapons(int cardNumber) {

        if (!cardcopier.playingCardsAreEnabled()) { return; }

        bool activeWeaponIsEmpty = (activeWeapon.spawnable.tag == "Empty") ? true : false;

        GameObject newWeapon = Instantiate(cardcopier.playingCardsInHand[cardNumber - 1].spawnable, weaponSpawnPoint.position, weaponSpawnPoint.rotation, weaponSpawnPoint);

        // change layermask of new weapon
        int activeWeaponLayer = LayerMask.NameToLayer("ActiveWeapon");
        newWeapon.layer = activeWeaponLayer;

        foreach (Transform child in newWeapon.transform) {
            foreach (Transform grandchildren in child) {
                grandchildren.gameObject.layer = activeWeaponLayer;
            }
            child.gameObject.layer = activeWeaponLayer;
        }

        cardcopier.ChangeCardImageInHand(cardcopier.spawnableObjects[0].playingCard, cardcopier.playingCardsInHand[cardNumber - 1]);

        foreach (SpawnableObjects obj in cardcopier.spawnableObjects) {
            // change the new card position to active card...
            if (activeWeapon.spawnable.tag == obj.spawnable.tag) {

                // put active weapon into card
                if (!activeWeaponIsEmpty) {
                    cardcopier.ChangeCardImageInHand(obj.playingCard, cardcopier.playingCardsInHand[activeWeaponCardNumber - 1]);
                }
                cardcopier.ReplaceSpawnableDataInCard(cardNumber, cardcopier.spawnableObjects[0]);

                if (!activeWeaponIsEmpty) {
                    cardcopier.ReplaceSpawnableDataInCard(activeWeaponCardNumber, obj);
                }
                activeWeaponCardNumber = cardNumber;
                Destroy(activeWeapon.spawnable);
                activeWeapon.spawnable = newWeapon;
                return;

            }

        }

    }

    private void EquipWeapon() {
        for (int i = 1; i <= 3; i++) {
            if (Input.GetKeyUp((KeyCode)(48 + i))) {
                if (switchWeapon) {
                    ChangeWeapons(i);
                    switchWeapon = false;
                    timer = 0f;
                }
            } else if (Input.GetKeyDown((KeyCode)(48 + i))) {
                timer = 0f;
            } else if (Input.GetKey((KeyCode)(48 + i))) {

                timer += Time.deltaTime;

                if (timer < pressDelayDuration) {
                    switchWeapon = true;
                } else {
                    switchWeapon = false;
                }

            }
        }
    }

    
}
