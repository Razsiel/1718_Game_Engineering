using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilities;

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
        this.Panel = this.gameObject;

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
        // Inform other player
        // TableFlip
        // Load scene Main.scene
        SceneManager.LoadScene(MainMenu);
    }

    public void OnClick_BackToLevelSelect()
    {
        // Matthijs TODO: level/player lists?
        // Inform other player
        // TableFlip
        // Load scene LevelSelect.scene
        SceneManager.LoadScene(LevelSelect);
    }

    public void OnChanged_BgmVolumeSlider(Slider BgmSlider)
    {
        BgmBehaviour.Instance.SetVolume(BgmSlider.value);
    }

    public void OnChanged_SfxVolumeSlider(Slider SfxSlider)
    {
        SfxBehaviour.Instance.SetVolume(SfxSlider.value);
    }

    public override void Submit()
    {
        Close();
    }

    public override void Cancel()
    {
        Close();
    }
}
