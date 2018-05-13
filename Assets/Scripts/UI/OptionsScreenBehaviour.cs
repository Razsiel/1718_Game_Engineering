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
        // Load settings from PlayerPrefs
        HideOptionsPanel();
    }

    public void ShowOptionsPanel()
    {
        // Load settings from PlayerPrefs
        OptionsCanvas.SetActive(true);
    }

    public void HideOptionsPanel()
    {
        OptionsCanvas.SetActive(false);
    }

    public void OnClick_BackToMainMenu()
    {
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
        // Save settings to PlayerPrefs
    }
}
