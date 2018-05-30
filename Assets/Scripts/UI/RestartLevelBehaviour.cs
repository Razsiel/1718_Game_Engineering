using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelBehaviour : MonoBehaviour {

    public void Restart() {
        print("Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
