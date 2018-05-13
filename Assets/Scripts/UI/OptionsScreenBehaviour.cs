using System.Collections;
using System.Collections.Generic;
using SmartLocalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class OptionsScreenBehaviour : MonoBehaviour {
    
    [SerializeField] private GameObject OptionsCanvas;
    [SerializeField] private Button LanguageFlagDutch;
    [SerializeField] private Button LanguageFlagEnglish;
    [SerializeField] private Slider BgmVolumeSlider;
    [SerializeField] private Slider SfxVolumeSlider;

    void Awake()
    {
        HideOptionsPanel();
    }

    public void ShowOptionsPanel()
    {
        OptionsCanvas.SetActive(true);

        BgmVolumeSlider.value = PlayerPrefs.GetFloat("BGM Volume");
        SfxVolumeSlider.value = PlayerPrefs.GetFloat("SFX Volume");
    }

    public void HideOptionsPanel()
    {
        OptionsCanvas.SetActive(false);
    }

    public void OnClick_BackToMainMenu()
    {
        SaveSettings();
        HideOptionsPanel();
    }

    public void OnClick_LanguageFlagDutch()
    {
        // Set current language setting to Dutch
        LanguageManager.Instance.ChangeLanguage("nl-NL");
        print("Language set to: " + LanguageManager.Instance.CurrentlyLoadedCulture.nativeName);
    }

    public void OnClick_LanguageFlagEnglish()
    {
        // Set current language setting to English
        LanguageManager.Instance.ChangeLanguage("en-GB");
        print("Language set to: " + LanguageManager.Instance.CurrentlyLoadedCulture.nativeName);
    }

    public void OnChanged_BgmVolumeSlider(Slider BgmSlider)
    {
        BgmBehaviour.Instance.SetVolume(BgmSlider.value);
    }

    public void OnChanged_SfxVolumeSlider(Slider SfxSlider)
    {
        SfxBehaviour.Instance.SetVolume(SfxSlider.value);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGM Volume", BgmVolumeSlider.value);
        PlayerPrefs.SetFloat("SFX Volume", SfxVolumeSlider.value);

        PlayerPrefs.Save();
    }
}
