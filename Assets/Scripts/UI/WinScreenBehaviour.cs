using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenBehaviour : MonoBehaviour {

    void Awake()
    {
        EventManager.AllLevelGoalsReached += ShowWinScreen;
    }

	void Start () {
		gameObject.SetActive(false);
	}

    void ShowWinScreen()
    {
        gameObject.SetActive(true);
    }

    public void OnClickContinue()
    {
        // Restart level? To level Select?
        print("Level Completed!");
        gameObject.SetActive(false);
    }
}
