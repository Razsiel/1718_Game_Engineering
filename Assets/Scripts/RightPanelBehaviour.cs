using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RightPanelBehaviour : MonoBehaviour {

    private RectTransform _rectTransform;
    

    void Awake()
    {
        EventManager.InitializeUi += Initialize;
        EventManager.EnableUserInput += ShowRightPanel;
        EventManager.DisableUserInput += HideRightPanel;
    }

    void Initialize()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void HideRightPanel()
    {
        Vector3 HidePosition = new Vector3(2688.0f, 1079.3f, 0.0f);
        _rectTransform.DOLocalMove(HidePosition, 1f);
    }

    void ShowRightPanel()
    {
        Vector3 ShowPosition = new Vector3(1920.0f, 1079.3f, 0.0f);
        _rectTransform.DOLocalMove(ShowPosition, 0.5f);
    }
}
