using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCopier : MonoBehaviour
{
    [SerializeField] Transform objectSpawnPoint, env;

    [SerializeField] List<SpawnableObjects> playingCardsInHand = new List<SpawnableObjects>();
    [SerializeField] List<SpawnableObjects> spawnableObjects = new List<SpawnableObjects>();

    Transform cam;


    string currentObj;

    void Start() {
        cam = Camera.main.transform;
    }

    void Update() {

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f)) {
            Debug.DrawRay(cam.position, cam.forward * hit.distance, Color.yellow);

            for (int i = 1; i <= 3; i++) { 
                if (Input.GetKeyDown((KeyCode)(48 + i))) { // If Key 1..5 Pressed
                    CopyHitObjectToPlayingCard(hit, i);
                }
            }

        } else {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
        
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
