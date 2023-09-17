using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    [SerializeField] SpawnableObjects activeWeapon;

    void Start() {
        cardcopier = GetComponent<CardCopier>();
        pressDelayDuration = cardcopier.pressDelayDuration;
    }

    void Update() {
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

    public void ChangeWeapons(int cardNumber) {

        GameObject newWeapon = Instantiate(cardcopier.playingCardsInHand[cardNumber - 1].spawnable, weaponSpawnPoint.position, weaponSpawnPoint.rotation, weaponSpawnPoint);
        
        foreach (SpawnableObjects obj in cardcopier.spawnableObjects) {
            if (activeWeapon.spawnable.tag == obj.spawnable.tag) {
                cardcopier.ReplaceObjectToBeSpawnedFromCardInHand(cardNumber, obj);
                activeWeapon.playingCard = obj.playingCard;
            }
            
        }

        cardcopier.ChangeCardImageInHand(activeWeapon.playingCard, cardcopier.playingCardsInHand[cardNumber - 1]);

        Destroy(activeWeapon.spawnable);
        activeWeapon.spawnable = newWeapon;

    }
}
