using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenBehaviour : MonoBehaviour {

    public void Initialize()
    {
        EventManager.OnAllLevelGoalsReached += ShowWinScreen;
        gameObject.SetActive(false);
    }

    public void ShowWinScreen()
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
