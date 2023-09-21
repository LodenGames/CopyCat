using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class ProceduralCellSpawner : MonoBehaviour {

    [SerializeField] GameObject[] spawnPoints;

    [SerializeField] GameObject[] spawnCells;

    [SerializeField] string spawnerTag;

    CardCopier cardCopier;

    void Start() {
        spawnPoints = GameObject.FindGameObjectsWithTag(spawnerTag);

        foreach (GameObject spawnPoint in spawnPoints) {
            int randomIndex = Random.Range(0, spawnCells.Length);
            // Pick a random obj from list of objs
            Instantiate(spawnCells[randomIndex], spawnPoint.transform.position, spawnPoint.transform.rotation);
        }

    }
}
