using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanelBehaviour : MonoBehaviour
{
    private RectTransform _rectTransform;
//    Vector3 HidePosition = new Vector3(-1920.0f, -1079.7f, 0.0f);
    Vector3 HidePosition = new Vector3(0.0f, -1404.3f, 0.0f);
    Vector3 ShowPosition = new Vector3(0.0f, -756.3f, 0.0f);

    void Awake ()
	{
	    EventManager.InitializeUi += Initialize;
	    EventManager.EnableUserInput += ShowBottomPanel;
	    EventManager.DisableUserInput += HideBottomPanel;
    }

    void Initialize()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void HideBottomPanel()
    {
        _rectTransform.DOLocalMove(HidePosition, 1f);
        print("bot " + _rectTransform.localPosition);

    }

    void ShowBottomPanel()
    {
        _rectTransform.DOLocalMove(ShowPosition, 0.5f);

        print("bot " + _rectTransform.localPosition);

    }
}
