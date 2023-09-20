using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBreak : MonoBehaviour {

    GameObject lightOff;
    GameObject key;

    private void Start() {
        lightOff = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AccessPrefab>().lightOff;
        key = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AccessPrefab>().key;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "LightBreak") {
            Debug.Log("Collision with light");
            if (collision.gameObject.GetComponent<LightState>().broken == true) { return; }
            Debug.Log("Spawn light");
            // Get component from child light, turn it off
            collision.transform.GetComponentInChildren<Light>().enabled = false;

            // Spawn new light in same spot as collision
            Instantiate(lightOff, collision.gameObject.transform.position, collision.gameObject.transform.rotation);

            // Spawn key with rb (below light)
            Vector3 offset = new Vector3(0, -1f, 0f);
            GameObject keyTemp = Instantiate(key, collision.gameObject.transform.position + offset, collision.gameObject.transform.rotation);

            if (keyTemp.GetComponent<Rigidbody>() == null) {
                keyTemp.AddComponent<Rigidbody>();
            }
            // Destroy collision obj
            Destroy(collision.gameObject);

        }
    }



}
