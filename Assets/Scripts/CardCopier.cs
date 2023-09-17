using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCopier : MonoBehaviour
{

    [Header("Spawnable Objects")]
    [SerializeField] List<SpawnableObjects> playingCardsInHand = new List<SpawnableObjects>();
    [SerializeField] List<SpawnableObjects> spawnableObjects = new List<SpawnableObjects>();

    [Header("UI Elements")]
    [SerializeField] Image cursor;
    [SerializeField] Image copyRing;
    [SerializeField] float scanDuration = 0.45f, pressDelayDuration = 0.15f;

    [Header("Spawn Points")]
    [SerializeField] Transform objectSpawnPoint;
    [SerializeField] Transform env;

    WeaponsController weaponsController;
    
    Transform cam;
    float timer;
    bool canScan, switchWeapon;


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
                    if (switchWeapon) {
                        Debug.Log("Switch Weapon!");
                        weaponsController.ChangeWeapons(i);
                        switchWeapon = false;
                    }
                    ResetCopyCursorUI();
                } 
                else if (Input.GetKeyDown((KeyCode)(48 + i))) { 
                    ResetCopyCursorUI();
                    canScan = true;
                } 
                else if (Input.GetKey((KeyCode)(48 + i))) {
                    if (!canScan) { return; }

                    timer += Time.deltaTime;

                    // TODO refactor below... messy

                    if (timer < pressDelayDuration) {
                        switchWeapon = true;
                    } 
                    else {
                        switchWeapon = false;
                        ShowCopyUILoadingCircle();
                    }

                    if (timer >= scanDuration && canScan) {
                        CopyHitObjectToPlayingCard(hit, i);
                        ResetCopyCursorUI();
                        canScan = false;
                    }

                }
            }
        } 
        else {
            cursor.color = Color.black;
            ResetCopyCursorUI();
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); // Debugging
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

        // Find object to spawn with the same tag
        foreach (SpawnableObjects objectToSpawn in spawnableObjects) {
            if (hitTag == objectToSpawn.spawnable.tag) {
                SpawnTheObjectShownOnCardInfrontOfPlayer(cardInHand);
                ChangeCardImageInHand(objectToSpawn.playingCard, playingCardsInHand[cardInHand - 1]);
                Destroy(hit.collider.gameObject); // Destroy hit Object in World
                ReplaceObjectToBeSpawnedFromCardInHand(cardInHand, objectToSpawn);

                return;
            }
        }
    }

    private void ReplaceObjectToBeSpawnedFromCardInHand(int cardInHand, SpawnableObjects objectToSpawn) {
        playingCardsInHand[cardInHand - 1].spawnable = objectToSpawn.spawnable;
    }

    private void SpawnTheObjectShownOnCardInfrontOfPlayer(int cardInHand) {
        if (playingCardsInHand[cardInHand - 1].spawnable.tag == "Empty") { return; }
        
        GameObject temp = Instantiate(playingCardsInHand[cardInHand - 1].spawnable, objectSpawnPoint.position, objectSpawnPoint.rotation, env);

        AddRigidBodyIfNonExists(temp);
    }

    private static void AddRigidBodyIfNonExists(GameObject temp) {
        if (temp.GetComponent<Rigidbody>() == null) {
            temp.AddComponent<Rigidbody>();
        }
    }

    private void ChangeCardImageInHand(GameObject newCard, SpawnableObjects playingCardsInHand) {
        Transform handPosiiton = playingCardsInHand.playingCard.transform;
        GameObject temp = Instantiate(newCard, handPosiiton.position, handPosiiton.rotation, cam);
        Destroy(playingCardsInHand.playingCard);
        playingCardsInHand.playingCard = temp;
    }
}
