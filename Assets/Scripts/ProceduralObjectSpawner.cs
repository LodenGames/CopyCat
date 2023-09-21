using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectSpawner : MonoBehaviour {

    [SerializeField] GameObject[] spawnPoints;

    [SerializeField] string spawnerTag;

    [SerializeField] float randomPercentage;

    [SerializeField] Transform backupHammerSpawn;

    CardCopier cardCopier;

    void Start() {
        spawnPoints = GameObject.FindGameObjectsWithTag(spawnerTag);

        cardCopier = GetComponent<CardCopier>();

        foreach (GameObject spawnPoint in spawnPoints) {
            int randSpawn = Random.Range(0, 100);
            if (randSpawn > randomPercentage) { continue; }
            Instantiate(cardCopier.GetRandomSpawnable(), spawnPoint.transform.position, spawnPoint.transform.rotation);
            
        }

        if (GameObject.FindGameObjectsWithTag("Hammer").Length == 0) {
            Instantiate(cardCopier.spawnableObjects[1].spawnable, backupHammerSpawn.position, backupHammerSpawn.rotation);
        }

    }
}
