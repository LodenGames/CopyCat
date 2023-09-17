using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObjects {

    // SpawnableObj
    // Object Img
    // LATER ~~~~~~~~~~
    // Size or distance to spawn from player
    public GameObject spawnable, playingCard;

    public SpawnableObjects(GameObject spawnable, GameObject playingCard) {
        this.spawnable = spawnable;
        this.playingCard = playingCard;
    }

}
