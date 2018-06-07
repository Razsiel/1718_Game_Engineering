//using System.Collections;
//using System.Collections.Generic;
//using Assets.Scripts;
//using DG.Tweening;
//using UnityEngine;
//
//public class RightPanelBehaviour : MonoBehaviour {
//
//    private RectTransform _rectTransform;
//
//    Vector3 HidePosition = new Vector3(2304.0f, 324.1f, 0.0f);
//    Vector3 ShowPosition = new Vector3(1536.0f, 324.0f, 0.0f);
//
//    void Awake()
//    {
//        EventManager.OnInitializeUi += Initialize;
//        EventManager.OnUserInputEnable += ShowRightPanel;
//        EventManager.OnUserInputDisable += HideRightPanel;
//    }
//
//    void Initialize(GameInfo gameInfo)
//    {
//        _rectTransform = GetComponent<RectTransform>();
//    }
//
//    void HideRightPanel()
//    {
//        _rectTransform.DOLocalMove(HidePosition, 1f);
//    }
//
//    void ShowRightPanel()
//    {
//        _rectTransform.DOLocalMove(ShowPosition, 1f);
//    }
//}
