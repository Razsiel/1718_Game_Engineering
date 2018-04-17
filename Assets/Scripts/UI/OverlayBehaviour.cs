using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayBehaviour : MonoBehaviour {

	void Awake ()
	{
	    EventManager.MonologueEnded += ShowOverlay;
	}

    void Start()
    {
        HideOverlay();
    }

    void ShowOverlay()
    {
        gameObject.SetActive(true);
    }

    public void HideOverlay()
    {
        gameObject.SetActive(false);
    }

}
