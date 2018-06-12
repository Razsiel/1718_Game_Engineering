using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilities;
using Assets.Scripts;
using Assets.Scripts.Photon.Level;

public class IngameMenuBehaviour : ModalPanel
{
    [SerializeField] private SceneField LevelSelect;
    [SerializeField] private SceneField MainMenu;
    [SerializeField] private Button BackToGame;
    [SerializeField] private Button BackToMainMenu;
    [SerializeField] private Button BackToLevelSelect;
    [SerializeField] private Slider BgmVolumeSlider;
    [SerializeField] private Slider SfxVolumeSlider;    

    void Awake()
    {  
        BgmVolumeSlider.value = PlayerPrefs.GetFloat("BGM Volume");
        SfxVolumeSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        Close();
    }

    public void OnClick_BackToGame()
    {
        Close();
    }

    public void OnClick_BackToMainMenu()
    {
        if (GlobalData.Instance.GameInfo.IsMultiplayer)
        {
            LevelPhotonManager.Instance.LeaveRoom();           
        }
        // Inform other player
        // TableFlip
        // Load scene Main.scene
        SceneManager.LoadScene(MainMenu);
    }

    public void OnClick_BackToLevelSelect()
    {
        if(GlobalData.Instance.GameInfo.IsMultiplayer)
        {
            LevelPhotonManager.Instance.GoToScene(LevelSelect);
        }
        else
            SceneManager.LoadScene(LevelSelect.SceneName);
        // Matthijs TODO: level/player lists?
        // Inform other player
        // TableFlip
        // Load scene LevelSelect.scene
    }

    public void OnChanged_BgmVolumeSlider(Slider BgmSlider)
    {
        BgmBehaviour.Instance.SetVolume(BgmSlider.value);
    }

    public void OnChanged_SfxVolumeSlider(Slider SfxSlider)
    {
        SfxBehaviour.Instance.SetVolume(SfxSlider.value);
    }
}
