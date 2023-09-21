using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBreak : MonoBehaviour {

    GameObject lightOff;
    GameObject key;
    AudioSource audioSource;

    AccessPrefab acessPrefab;


    private void Start() {
        acessPrefab = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AccessPrefab>();
        lightOff = acessPrefab.lightOff;
        key = acessPrefab.key;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision) {
        // see what tag this game object has

        switch (transform.tag) {
            case "Hammer":
                audioSource.volume = 0.3f;
                audioSource.PlayOneShot(acessPrefab.sounds[0]);
                break;
            case "Key":
                audioSource.volume = 0.5f;
                audioSource.PlayOneShot(acessPrefab.sounds[1]);
                break;
            case "Book":
                audioSource.volume = 0.5f;
                audioSource.PlayOneShot(acessPrefab.sounds[2]);
                break;
            default:
                break;
        }

        if (collision.gameObject.tag == "LightBreak") {

            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(acessPrefab.sounds[3]);

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
