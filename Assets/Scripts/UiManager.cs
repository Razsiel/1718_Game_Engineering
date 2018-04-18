using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Lib.Extensions;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text LEVEL_GOALS_TITLE;
    public Text LEVEL_GOALS_DESCRIPTION;

    void Awake()
    {
        EventManager.InitializeUi += Initialize;
    }

    void Initialize()
    {
        LEVEL_GOALS_TITLE.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_TITLE");
        LEVEL_GOALS_DESCRIPTION.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_DEMO1");
    }

    public void OnHoverOverElement()
    {
        EventManager.PlaySoundEffect(SFX.ButtonHover);
    }
}
