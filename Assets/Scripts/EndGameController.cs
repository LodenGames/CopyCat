using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour {

    public void PressedQuit() {
        Application.Quit();
    }

    public void PressedRestart() {
        SceneManager.LoadScene(0);
    }
}
