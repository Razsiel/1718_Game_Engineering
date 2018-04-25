using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanelBehaviour : MonoBehaviour
{
    private RectTransform _rectTransform;

    Vector3 HidePosition = new Vector3(0.0f, -1404.3f, 0.0f);
    Vector3 ShowPosition = new Vector3(0.0f, -756.3f, 0.0f);

    void Awake ()
	{
	    EventManager.OnInitializeUi += Initialize;
	    EventManager.OnUserInputEnable += ShowBottomPanel;
	    EventManager.OnUserInputDisable += HideBottomPanel;
    }

    void Initialize(GameInfo gameInfo)
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void HideBottomPanel()
    {
        _rectTransform.DOLocalMove(HidePosition, 1f);

    }

    void ShowBottomPanel()
    {
        _rectTransform.DOLocalMove(ShowPosition, 1f);
    }
}
