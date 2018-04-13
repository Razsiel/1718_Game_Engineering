using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static UnityAction LevelSelected;
    public static UnityAction LoadingCompleted;
    public static UnityAction AllLevelGoalsReached;
           
    public static UnityAction InitializeUi;
    public static UnityAction InitializePhoton;
    public static UnityAction InitializeAudio;

    public static UnityAction EnableUserInput;
    public static UnityAction DisableUserInput;
    public static UnityAction ReadyButtonClicked;
    public static UnityAction PhotonSynchronized;
    public static UnityAction MonologueEnded;

    public static UnityAction<SFX> PlaySoundEffect;
    public static UnityAction<BGM> PlayMusicClip;


    public static void OnInitializeUi()
    {
        InitializeUi?.Invoke();
    }

    public static void OnInitializePhoton()
    {
        InitializePhoton?.Invoke();
    }

    public static void OnEnableUserInput()
    {
        EnableUserInput?.Invoke();
    }

    public static void OnDisableUserInput()
    {
        DisableUserInput?.Invoke();
    }

    public static void OnClickReadyButton()
    {
        ReadyButtonClicked?.Invoke();
    }

    public static void OnLevelSelected()
    {
        LevelSelected?.Invoke();
    }

    public static void OnLoadingCompleted()
    {
        LoadingCompleted?.Invoke();
    }

    public static void OnAllLevelGoalsReached()
    {
        AllLevelGoalsReached?.Invoke();
    }

    public static void OnPhotonSynchronized()
    {
        PhotonSynchronized?.Invoke();
    }

    public static void OnMonologueEnded()
    {
        MonologueEnded?.Invoke();
    }

    public static void OnPlaySoundEffect(SFX soundName)
    {
        PlaySoundEffect?.Invoke(soundName);
    }

    public static void OnPlayMusicClip(BGM clipName)
    {
        PlayMusicClip?.Invoke(clipName);
    }
    

    public static void OnInitializeAudio()
    {
        InitializeAudio?.Invoke();
    }
}
