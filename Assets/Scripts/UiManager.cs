using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Lib.Extensions;
using Assets.Scripts.Lib.Helpers;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : TGEMonoBehaviour
{
    public Text LEVEL_GOALS_TITLE;
    public Text LEVEL_GOALS_DESCRIPTION;
    public Text PlayerColour;

    public override void Awake()
    {
        base.Awake();
        EventManager.OnInitializeUi += Initialize;
    }

    void Initialize(GameInfo gameInfo)
    {
        LEVEL_GOALS_TITLE.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_TITLE");
        LEVEL_GOALS_DESCRIPTION.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_DEMO1");
    }
}
