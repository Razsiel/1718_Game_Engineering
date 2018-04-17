﻿using System.Collections;
using System.Collections.Generic;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
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
    public static UnityAction InitializeMonologue;

    public static UnityAction EnableUserInput;
    public static UnityAction DisableUserInput;
    public static UnityAction<List<BaseCommand>> SequenceChanged;
    public static UnityAction ReadyButtonClicked;
    public static UnityAction StopButtonClicked;
    public static UnityAction PhotonSynchronized;
    public static UnityAction<Monologue> MonologueStart;
    public static UnityAction MonologueEnded;

    public static UnityAction<SFX> PlaySoundEffect;
    public static UnityAction<BGM> PlayMusicClip;
    public static UnityAction Simulate;
    public static UnityAction<LevelData, List<Player>> LevelReset;
    public static UnityAction<LevelData, List<TGEPlayer>> LoadLevel;
    public static UnityAction<LevelData> LevelLoaded;

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
        EnableUserInput?.Invoke();
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

    public static void OnInitializeMonologue()
    {
        InitializeMonologue?.Invoke();
    }


    public static void OnMonologueStart(Monologue monologue)
    {
        MonologueStart?.Invoke(monologue);
    }

    public static void OnSimulate() {
        Simulate?.Invoke();
    }

    public static void OnLevelReset() {
        //LevelReset?.Invoke();
    }

    public static void OnLoadLevel(LevelData levelData, List<TGEPlayer> players) {
        LoadLevel?.Invoke(levelData, players);
    }

    public static void OnLevelLoaded(LevelData levelData) {
        LevelLoaded?.Invoke(levelData);
    }
}
