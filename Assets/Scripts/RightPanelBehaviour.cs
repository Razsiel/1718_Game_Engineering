using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RightPanelBehaviour : MonoBehaviour {

    private RectTransform _rectTransform;

    Vector3 HidePosition = new Vector3(2304.0f, 324.1f, 0.0f);
    Vector3 ShowPosition = new Vector3(1536.0f, 324.0f, 0.0f);

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
        _rectTransform.DOLocalMove(HidePosition, 1f);
    }

    void ShowRightPanel()
    {
        _rectTransform.DOLocalMove(ShowPosition, 1f);
    }
}
