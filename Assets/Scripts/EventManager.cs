using System.Collections;
using System.Collections.Generic;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static UnityAction OnLevelSelected;
    public static UnityAction OnLoadingCompleted;
    public static UnityAction OnAllLevelGoalsReached;
           
    public static UnityAction OnInitializeUi;
    public static UnityAction<TGEPlayer> OnInitializePhoton;
    public static UnityAction OnAudioInitialize;
    public static UnityAction OnMonologueInitialized;

    public static UnityAction OnPlayersInitialized;
    public static UnityAction<Player> OnPlayerInitialized;

    public static UnityAction OnUserInputEnable;
    public static UnityAction OnUserInputDisable;
    public static UnityAction<List<BaseCommand>> OnSequenceChanged;
    public static UnityAction OnReadyButtonClicked;
    public static UnityAction OnStopButtonClicked;
    public static UnityAction OnPhotonSynchronized;
    public static UnityAction<Monologue> OnMonologueStart;
    public static UnityAction OnMonologueEnded;
    public static UnityAction OnExecutionStarted;

    public static UnityAction<SFX> OnPlaySoundEffect;
    public static UnityAction<BGM> OnPlayMusicClip;
    public static UnityAction OnSimulate;
    public static UnityAction<LevelData, List<Player>> OnLevelReset;
    public static UnityAction<LevelData, List<TGEPlayer>> OnLoadLevel;
    public static UnityAction<LevelData> OnLevelLoaded;
    public static UnityAction OnMenuClicked;

    // Temp calls
    public static UnityAction OnPlayerColourSet;

    public static UnityAction OnAllPlayersReady;
    public static UnityAction<Player> OnPlayerSpawned;

    public static void InitializeUi()
    {
        OnInitializeUi?.Invoke();
    }

    public static void InitializePhoton(TGEPlayer localPlayer)
    {
        OnInitializePhoton?.Invoke(localPlayer);
    }

    public static void UserInputEnable()
    {
        OnUserInputEnable?.Invoke();
    }

    public static void UserInputDisable()
    {
        OnUserInputDisable?.Invoke();
    }

    public static void ReadyButtonClicked()
    {
        OnReadyButtonClicked?.Invoke();
    }

    public static void LevelSelected()
    {
        OnLevelSelected?.Invoke();
    }

    public static void LoadingCompleted()
    {
        OnLoadingCompleted?.Invoke();
    }

    public static void AllLevelGoalsReached()
    {
        OnAllLevelGoalsReached?.Invoke();
    }

    public static void PhotonSynchronized()
    {
        OnPhotonSynchronized?.Invoke();
    }

    public static void MonologueEnded()
    {
        OnMonologueEnded?.Invoke();
    }

    public static void AudioInitialized()
    {
        OnAudioInitialize?.Invoke();
    }

    public static void MonologueInitialized()
    {
        OnMonologueInitialized?.Invoke();
    }


    public static void MonologueStart(Monologue monologue)
    {
        OnMonologueStart?.Invoke(monologue);
    }

    public static void Simulate() {
        OnSimulate?.Invoke();
    }

    public static void LevelReset(LevelData levelData, List<Player> players) {
        OnLevelReset?.Invoke(levelData, players);
    }

    public static void LoadLevel(LevelData levelData, List<TGEPlayer> players) {
        OnLoadLevel?.Invoke(levelData, players);
    }

    public static void LevelLoaded(LevelData levelData) {
        OnLevelLoaded?.Invoke(levelData);
    }

    public static void AllPlayersReady() {
        OnAllPlayersReady?.Invoke();
    }

    public static void MenuClicked()
    {
        OnMenuClicked?.Invoke();
    }

    public static void PlayerColourSet()
    {
        OnPlayerColourSet?.Invoke();
    }

    public static void PlayersInitialized()
    {
        OnPlayersInitialized?.Invoke();
    }

    public static void PlayerInitialized(Player player)
    {
        OnPlayerInitialized?.Invoke(player);
    }

    public static void PlayerSpawned(Player player) {
        OnPlayerSpawned?.Invoke(player);
    }
}
