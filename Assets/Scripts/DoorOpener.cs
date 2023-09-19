using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorOpener : MonoBehaviour {
    
    public bool canMoveDoor;

    public GameObject doorObjToMove;

    float moveAmount = 1.2f;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "NearDoor"){
            if (!other.GetComponent<DoorState>().doorHasBeenMoved) {
                canMoveDoor = true;
                doorObjToMove = other.gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "NearDoor") {
            if (!other.GetComponent<DoorState>().doorHasBeenMoved) {
                canMoveDoor = false;
            }
        }
    }

    public void MoveDoor() {
        doorObjToMove.transform.position = new Vector3(doorObjToMove.transform.position.x + moveAmount, doorObjToMove.transform.position.y, doorObjToMove.transform.position.z);
        doorObjToMove.GetComponent<DoorState>().doorHasBeenMoved = true;
        canMoveDoor = false;
    }

}
