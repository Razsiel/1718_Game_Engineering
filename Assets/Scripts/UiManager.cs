using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Extensions;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text LEVEL_GOALS_TITLE;
    public Text LEVEL_GOALS_DESCRIPTION;

    public CanvasGroup CanvasGroup;

    void Awake()
    {
        EventManager.InitializeUi += Initialize;
        EventManager.EnableUserInput += ShowUi;
        EventManager.DisableUserInput += HideUi;
    }

    void Initialize()
    {
//        LEVEL_GOALS_TITLE.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_TITLE");
//        LEVEL_GOALS_DESCRIPTION.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_DEMO1");

    }

    void ShowUi()
    {
        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;
    }

    void HideUi()
    {
        CanvasGroup.alpha = 0f;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;
    }
}
