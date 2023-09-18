using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardCopier : MonoBehaviour {

    [Header("Spawnable Objects")]
    public List<SpawnableObjects> playingCardsInHand = new List<SpawnableObjects>();
    public List<SpawnableObjects> spawnableObjects = new List<SpawnableObjects>();

    [Header("UI Elements")]
    [SerializeField] Image cursor;
    [SerializeField] Image copyRing;
    public float scanDuration = 0.45f, pressDelayDuration = 0.15f;

    [Header("Spawn Points")]
    [SerializeField] Transform objectSpawnPoint;
    [SerializeField] Transform env;

    WeaponsController weaponsController;

    Transform cam;
    float timer;
    bool canScan;


    void Start() {
        cam = Camera.main.transform;
        weaponsController = GetComponent<WeaponsController>();
    }

    void Update() {

        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Copyable"); // Resource: https://forum.unity.com/threads/raycast-layermask-parameter.944194/

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f, layerMask)) {
            //Debug.DrawRay(cam.position, cam.forward * hit.distance, Color.yellow); // For Debugging

            cursor.color = Color.red;

            for (int i = 1; i <= 3; i++) {
                if (Input.GetKeyUp((KeyCode)(48 + i))) {
                    ResetCopyCursorUI();
                } else if (Input.GetKeyDown((KeyCode)(48 + i))) {
                    ResetCopyCursorUI();
                    canScan = true;
                } else if (Input.GetKey((KeyCode)(48 + i))) {
                    if (!canScan) { return; }

                    timer += Time.deltaTime;

                    DelayShowingLoadingCircleUntilButtonHeldNotPressed();

                    if (timer >= scanDuration && canScan) {
                        // check if active weapon is not tagged empty
                        // if so d othis
                        // else drop current weapon
                        CopyHitObjectToPlayingCard(hit, i);
                        ResetCopyCursorUI();
                        canScan = false;
                    }

                }
            }
        } else {
            cursor.color = Color.black;
            ResetCopyCursorUI();
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); // Debugging
        }
    }

    private void DelayShowingLoadingCircleUntilButtonHeldNotPressed() {
        if (timer > pressDelayDuration) {
            ShowCopyUILoadingCircle();
        }
    }

    private void ShowCopyUILoadingCircle() {
        copyRing.fillAmount = Mathf.Clamp((timer - pressDelayDuration) / (scanDuration - pressDelayDuration), 0.0f, 1.0f);

    }

    private void ResetCopyCursorUI() {
        timer = 0f;
        copyRing.fillAmount = 0f;
    }

    private void CopyHitObjectToPlayingCard(RaycastHit hit, int cardInHand) {
        string hitTag = hit.collider.tag;

        // when copying see if slot is empty and not = to the active Weapon slot I am currently using

        // Find object to spawn with the same tag
        foreach (SpawnableObjects objectToSpawn in spawnableObjects) {
            if (hitTag != objectToSpawn.spawnable.tag) { continue; }

            if (weaponsController.activeWeapon.spawnable.tag != "Empty"
                && cardInHand == weaponsController.activeWeaponCardNumber) {

                ChangeCardImageInHand(objectToSpawn.playingCard, playingCardsInHand[cardInHand - 1]);
                ReplaceSpawnableDataInCard(cardInHand, objectToSpawn);
                SpawnCardObjectOnCopyNoActiveWeapon(weaponsController.activeWeapon.spawnable, cardInHand);
                weaponsController.ChangeWeapons(cardInHand);
                ChangeCardImageInHand(spawnableObjects[0].playingCard, playingCardsInHand[cardInHand - 1]);
                ReplaceSpawnableDataInCard(cardInHand, spawnableObjects[0]);
                Destroy(hit.collider.gameObject);
            } else {
                SpawnCardObjectOnCopyNoActiveWeapon(playingCardsInHand[cardInHand - 1].spawnable, cardInHand);
                ChangeCardImageInHand(objectToSpawn.playingCard, playingCardsInHand[cardInHand - 1]);
                Destroy(hit.collider.gameObject); // Destroy hit Object in World
                ReplaceSpawnableDataInCard(cardInHand, objectToSpawn);
            }
            return;
        }
    }

    private void SpawnCardObjectOnCopyNoActiveWeapon(GameObject toSpawn, int cardInHand) {
        if (toSpawn.tag == "Empty") { return; }

        GameObject temp = Instantiate(toSpawn, objectSpawnPoint.position, objectSpawnPoint.rotation, env);
        AddRigidBodyIfNonExists(temp);
    }

    private static void AddRigidBodyIfNonExists(GameObject temp) {
        if (temp.GetComponent<Rigidbody>() == null) {
            temp.AddComponent<Rigidbody>();
        }
    }

    public SpawnableObjects GetSpawnableObjects(GameObject objectToMatch) {
        foreach (SpawnableObjects objectToSpawn in spawnableObjects) {
            if (objectToMatch.tag == objectToSpawn.spawnable.tag) {
                return objectToSpawn;
            }
        }
        return new SpawnableObjects(spawnableObjects[0].spawnable, spawnableObjects[0].spawnable);
    }

    public void ReplaceSpawnableDataInCard(int cardInHand, SpawnableObjects objectToSpawn) {
        playingCardsInHand[cardInHand - 1].spawnable = objectToSpawn.spawnable;
    }

    public void ChangeCardImageInHand(GameObject newCard, SpawnableObjects playingCardsInHand) {
        Transform handPosiiton = playingCardsInHand.playingCard.transform;
        GameObject temp = Instantiate(newCard, handPosiiton.position, handPosiiton.rotation, cam);
        Destroy(playingCardsInHand.playingCard);
        playingCardsInHand.playingCard = temp;
    }
}
