using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenBehaviour : MonoBehaviour {

    void Awake()
    {
    }

	void Start () {
		gameObject.SetActive(false);
	}

    public void Initialize()
    {
        EventManager.OnAllLevelGoalsReached += ShowWinScreen;
    }

    public void ShowWinScreen()
    {
        print("asdsadasdsa");
        gameObject.SetActive(true);
    }

    public void OnClickContinue()
    {
        // Restart level? To level Select?
        print("Level Completed!");
        gameObject.SetActive(false);
    }
}
