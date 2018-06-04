using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using RockVR.Video;
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

    private string _videoSaveDirectory;

    void Start()
    {
        LanguageManager.SetDontDestroyOnLoad();
        PlayerPrefs.SetInt("RecordVideos", 0); // Disable video recording
        _videoSaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Progranimals\\Video";

        if (PlayerPrefs.GetInt("IHavePlayedBefore") == 0)
        {
            BgmVolumeSlider.value = 0.5f;
            SfxVolumeSlider.value = 0.5f;
            OnClick_LanguageFlagDutch();
            Directory.CreateDirectory(_videoSaveDirectory);

            PlayerPrefs.SetInt("IHavePlayedBefore", 1);
            SaveSettings();
        }
        else
        {
            LoadPlayerPreferences();
        }

        HideOptionsPanel();
    }

    void LoadPlayerPreferences()
    {
        BgmVolumeSlider.value = PlayerPrefs.GetFloat("BGM Volume");
        SfxVolumeSlider.value = PlayerPrefs.GetFloat("SFX Volume");
    }

    public void ShowOptionsPanel()
    {
        OptionsCanvas.SetActive(true);

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
        LanguageManager.Instance.defaultLanguage = "nl-NL";
        print("Language set to: " + LanguageManager.Instance.CurrentlyLoadedCulture.nativeName);
    }

    public void OnClick_LanguageFlagEnglish()
    {
        // Set current language setting to English
        LanguageManager.Instance.ChangeLanguage("en-GB");
        LanguageManager.Instance.defaultLanguage = "en-GB";
        print("Language set to: " + LanguageManager.Instance.CurrentlyLoadedCulture.nativeName);
    }

    public void OnChanged_BgmVolumeSlider(Slider BgmSlider) {
        double newVolume = (Math.Exp(BgmSlider.value) - 1) / (Math.E - 1);
        BgmBehaviour.Instance.SetVolume((float) newVolume);
    }

    public void OnChanged_SfxVolumeSlider(Slider SfxSlider)
    {
        double newVolume = (Math.Exp(SfxSlider.value) - 1) / (Math.E - 1);
        SfxBehaviour.Instance.SetVolume((float) newVolume);
    }

    public void OnToggleRecordVideos(Toggle recordToggle)
    {
        PlayerPrefs.SetInt("RecordVideos", Convert.ToInt32(recordToggle.isOn));
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGM Volume", BgmVolumeSlider.value);
        PlayerPrefs.SetFloat("SFX Volume", SfxVolumeSlider.value);
        PlayerPrefs.SetString("Default language", LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);

        PlayerPrefs.Save();
    }

    public void OnClickOpenVideoFolder()
    {
        Directory.CreateDirectory(_videoSaveDirectory);
        Process.Start(@_videoSaveDirectory);
    }
}
