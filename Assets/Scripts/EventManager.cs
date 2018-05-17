using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

public class EventManager {
    public static UnityAction OnLevelSelected;
    public static UnityAction OnLoadingCompleted;
    public static UnityAction OnAllLevelGoalsReached;
    public static UnityAction<Dictionary<TGEPlayer, int>, int> OnPlayersScoreDetermined;
    public static UnityAction OnWinScreenContinueClicked;

    public static UnityAction<GameInfo> OnInitializeUi;
<<<<<<< HEAD
    public static UnityAction<TGEPlayer> OnInitializePhoton;
=======
    public static UnityAction OnRepaintUi;
    public static UnityAction OnInitializePhoton;
>>>>>>> 060f7ea10c8c1b4bdcff1dcd204b14fb626cc279
    public static UnityAction OnAudioInitialize;
    public static UnityAction OnMonologueInitialized;

    public static UnityAction OnPlayersInitialized;
    public static UnityAction<Player> OnPlayerInitialized;

    public static UnityAction OnUserInputEnable;
    public static UnityAction OnUserInputDisable;
    public static UnityAction<List<BaseCommand>> OnSequenceChanged;
    public static UnityAction<List<BaseCommand>> OnSecondarySequenceChanged;
    public static UnityAction<ReorderableList.ReorderableListEventStruct> OnElementDroppedToMainSequenceBar;
    public static UnityAction OnReadyButtonClicked;
    public static UnityAction<bool> OnPlayerReady;
    public static UnityAction OnStopButtonClicked;
    public static UnityAction OnPhotonSynchronized;
    public static UnityAction<Monologue> OnMonologueStart;
    public static UnityAction OnMonologueEnded;
    public static UnityAction OnExecutionStarted;
    
    public static UnityAction<LevelData, List<TGEPlayer>> OnSimulate;
    public static UnityAction<GameInfo, List<Player>> OnLevelReset;
    public static UnityAction<GameInfo> OnLoadLevel;
    public static UnityAction<GameInfo> OnLevelLoaded;

    public static UnityAction OnMenuClicked;
    
    public static UnityAction OnAllPlayersReady;
    public static UnityAction<Player> OnPlayerSpawned;
    public static UnityAction<GameInfo> OnGameStart;

    public static void InitializeUi(GameInfo gameInfo) {
        OnInitializeUi?.Invoke(gameInfo);
    }

    public static void InitializePhoton() {
        OnInitializePhoton?.Invoke();
    }

    public static void UserInputEnable() {
        OnUserInputEnable?.Invoke();
    }

    public static void UserInputDisable() {
        OnUserInputDisable?.Invoke();
    }

    public static void ReadyButtonClicked() {
        OnReadyButtonClicked?.Invoke();
    }
    public static void StopButtonClicked() {
        OnStopButtonClicked?.Invoke();
    }

    public static void LevelSelected() {
        OnLevelSelected?.Invoke();
    }

    public static void LoadingCompleted() {
        OnLoadingCompleted?.Invoke();
    }

    public static void AllLevelGoalsReached() {
        OnAllLevelGoalsReached?.Invoke();
    }

    public static void WinScreenContinueClicked()
    {
        OnWinScreenContinueClicked?.Invoke();
    }

    public static void PhotonSynchronized() {
        OnPhotonSynchronized?.Invoke();
    }

    public static void MonologueEnded() {
        OnMonologueEnded?.Invoke();
    }

    public static void AudioInitialized() {
        OnAudioInitialize?.Invoke();
    }

    public static void ElementDroppedToMainSequenceBar(ReorderableList.ReorderableListEventStruct arg0)
    {
        OnElementDroppedToMainSequenceBar?.Invoke(arg0);
    }

    public static void SequenceChanged()
    {
        OnSequenceChanged?.Invoke(GlobalData.Instance.GameInfo.LocalPlayer.Player.Sequence.Commands);
    }

    public static void SecondarySequenceChanged(List<BaseCommand> commands)
    {
        OnSecondarySequenceChanged?.Invoke(commands);
    }

    public static void MonologueInitialized() {
        OnMonologueInitialized?.Invoke();
    }


    public static void MonologueStart(Monologue monologue) {
        OnMonologueStart?.Invoke(monologue);
    }

    public static void Simulate(LevelData level, List<TGEPlayer> players) {
        OnSimulate?.Invoke(level, players);
    }

    public static void LevelReset(GameInfo gameInfo, List<Player> players) {
        OnLevelReset?.Invoke(gameInfo, players);
    }

    public static void LoadLevel(GameInfo gameInfo) {
        OnLoadLevel?.Invoke(gameInfo);
    }

    public static void LevelLoaded(GameInfo gameInfo) {
        OnLevelLoaded?.Invoke(gameInfo);
    }

    public static void PlayerReady(bool isReady)
    {
        OnPlayerReady?.Invoke(isReady);
    }

    public static void AllPlayersReady() {
        OnAllPlayersReady?.Invoke();
    }

    public static void MenuClicked() {
        OnMenuClicked?.Invoke();
    }

    public static void PlayersInitialized() {
        OnPlayersInitialized?.Invoke();
    }

    public static void PlayerInitialized(Player player) {
        OnPlayerInitialized?.Invoke(player);
    }

    public static void PlayerSpawned(Player player) {
        OnPlayerSpawned?.Invoke(player);
    }

    public static void GameStart(GameInfo gameInfo) {
        OnGameStart?.Invoke(gameInfo);
    }

    public static void PlayersScoreDetermined(Dictionary<TGEPlayer, int> playerScoreDic, int combinedStars)
    {
        Debug.Log("invoking playerscore determined");
        OnPlayersScoreDetermined?.Invoke(playerScoreDic, combinedStars);
    }
}