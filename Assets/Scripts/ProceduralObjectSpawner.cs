using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectSpawner : MonoBehaviour {

    [SerializeField] GameObject[] spawnPoints;

    [SerializeField] float randomPercentage;

    CardCopier cardCopier;

    void Start() {
        cardCopier = GetComponent<CardCopier>();
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (GameObject spawnPoint in spawnPoints) {
            int randSpawn = Random.Range(0, 100);
            if (randSpawn > randomPercentage) { continue; }
            Instantiate(cardCopier.GetRandomSpawnable(), spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
