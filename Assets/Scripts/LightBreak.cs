using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBreak : MonoBehaviour {

    GameObject lightOff;

    private void Start() {
        lightOff = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AccessPrefab>().lightOff;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "LightBreak") {
            Debug.Log("break light");
            // Get component from child light, turn it off
            collision.transform.GetComponentInChildren<Light>().enabled = false;

            // Spawn new light in same spot as collision
            Instantiate(lightOff, collision.gameObject.transform.position, collision.gameObject.transform.rotation);

            // Destroy collision obj
            Destroy(collision.gameObject);

        }
    }



}
