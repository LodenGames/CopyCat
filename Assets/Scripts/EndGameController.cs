using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour {

    public void PressedQuit() {
        Debug.Log("Quit pressed");
        Application.Quit();
    }

    public void PressedRestart() {
        SceneManager.LoadScene(0);
    }
}
