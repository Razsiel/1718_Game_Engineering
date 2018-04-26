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
        EventManager.OnPlayerColourSet += SetPlayerColourText;
    }

    void Initialize(GameInfo gameInfo)
    {
        LEVEL_GOALS_TITLE.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_TITLE");
        LEVEL_GOALS_DESCRIPTION.text = LanguageManager.Instance.GetTextValue("LEVEL_GOALS_DEMO1");
    }

    public void OnHoverOverElement()
    {
        EventManager.OnPlaySoundEffect(SFX.ButtonHover);
    }

    void SetPlayerColourText()
    {
        if (this.GameInfo.LocalPlayer.Player.PlayerNumber == 0)
        {
            PlayerColour.text = "ORANJE";
            PlayerColour.color = new Color32(255, 184, 65, 255);
        }
        else
        {
            PlayerColour.text = "BLAUW";
            PlayerColour.color = new Color32(68, 222, 255, 255);
        }
    }
}
