using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanelBehaviour : MonoBehaviour
{
    private RectTransform _rectTransform;
    Vector3 HidePosition = new Vector3(-1920.0f, -1079.7f, 0.0f);

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
    }

    void ShowBottomPanel()
    {
        Vector3 pos = HidePosition;
        pos.y += 648f;
        _rectTransform.DOLocalMove(pos, 0.5f);
    }
}
