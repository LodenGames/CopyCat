using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCopier : MonoBehaviour
{
    [SerializeField] GameObject playingCard;
    [SerializeField] Transform cam, objectSpawnPoint, env;

    [SerializeField] List<GameObject> playingCardsInHand = new List<GameObject>();
    [SerializeField] List<SpawnableObjects> spawnableObjects = new List<SpawnableObjects>();

    string currentObj;

    void Start() {
        
    }

    void Update() {

        Debug.Log(spawnableObjects);

        RaycastHit hit;
        
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f)) {
            Debug.DrawRay(cam.position, cam.forward * hit.distance, Color.yellow);

            //
            // Click 1 - 5 to assign playingCards
            if (Input.GetKeyDown(KeyCode.Alpha1)) {

                string hitTag = hit.collider.tag;
                GameObject temp;

                foreach (SpawnableObjects tempObj in spawnableObjects) {
                    if (tempObj.spawnable.tag == hitTag) {
                        temp = Instantiate(tempObj.spawnable, objectSpawnPoint.position, objectSpawnPoint.rotation, env);
                        ReplaceCard(tempObj.playingCard);
                        Destroy(hit.collider.gameObject);
                        currentObj = hitTag;
                        break;
                    }
                }

                //if (hit.collider.tag == "Hammer") {
                    
                //Debug.Log("current Object is: " + playingCard);
                //ReplaceCard(hammerPlayingCard);
                //Destroy(hit.collider.gameObject);
                //currentObj = "Hammer";
                //// Spawn object in the card if not empty ontop of card?
                //} else if (hit.collider.tag == "King") {
                //    Debug.Log("current Object is: " + playingCard);
                //    //GameObject temp; 
                //    if (currentObj == "Hammer") {
                //        temp = Instantiate(hammerObj, objectSpawnPoint.position, objectSpawnPoint.rotation, env);
                //    } else {
                //        temp = Instantiate(kingObj, objectSpawnPoint.position, objectSpawnPoint.rotation, env);
                //    }
                //    // if temp does not have an rb component add one

                //    ReplaceCard(kingPlayingCard);
                //    Destroy(hit.collider.gameObject);
                //    currentObj = "King";
                //    // Spawn object in playingCard
                //} 
            }

            
        } else {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        // replace with hammer playng card
        
    }



    void ReplaceCard(GameObject newCard) {
        GameObject temp = Instantiate(newCard, playingCard.transform.position, playingCard.transform.rotation, cam);
        Destroy(playingCard);
        playingCard = temp;
    }
}
