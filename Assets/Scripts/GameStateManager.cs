﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Photon;
using M16h;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Assets.Scripts {
    public class GameStateManager : SingleMonobehaviour<GameStateManager> {
        private GameInfo _gameInfo;

        public LevelData Level;
        public CommandLibrary CommandLibrary;
        public bool IsMultiPlayer;

        private TinyStateMachine<GameState, GameStateTrigger> fsm;

        public void Awake() {
            Debug.Log($"Awake: {nameof(GameStateManager)}");
            fsm = new TinyStateMachine<GameState, GameStateTrigger>(GameState.Init);

            // START GAME -> CUTSCENE
            fsm.Tr(GameState.Init, GameStateTrigger.Next, GameState.Cutscene)
               .On(OnCutsceneStateEnter);

            // CUTSCENE -> GAMESTART
            fsm.Tr(GameState.Cutscene, GameStateTrigger.Next, GameState.GameStart)
               .On(OnStartGameStateEnter);

            // GAMESTART -> EDITSEQUENCE
            fsm.Tr(GameState.GameStart, GameStateTrigger.Next, GameState.EditSequence)
               .On(OnEditSequenceStateEnter);

            // EDITSEQUENCE <-> READYANDWAITINGFORPLAYERS
            fsm.Tr(GameState.EditSequence, GameStateTrigger.Next, GameState.ReadyAndWaitingForPlayers)
               .On(OnPlayerReadyStateEnter)
               .Tr(GameState.ReadyAndWaitingForPlayers, GameStateTrigger.Back, GameState.EditSequence)
               .On(OnEditSequenceStateEnter);

            // READYANDWAITINGFORPLAYERS -> SIMULATE
            fsm.Tr(GameState.ReadyAndWaitingForPlayers, GameStateTrigger.Next, GameState.Simulate)
               .On(OnSimulateStateEnter);

            // SIMULATE -> LEVELCOMPLETE
            fsm.Tr(GameState.Simulate, GameStateTrigger.Next, GameState.LevelComplete)
               .On(OnLevelCompleteStateEnter)
               .Tr(GameState.Simulate, GameStateTrigger.Back, GameState.EditSequence)
               .On(OnEditSequenceStateEnter);
        }

        public void Start() {
            Debug.Log($"Start: {nameof(GameStateManager)}");

            _gameInfo = new GameInfo {
                Level = Level,
                IsMultiplayer = IsMultiPlayer,
                Players = new List<TGEPlayer>(),
                AllCommands = CommandLibrary
            };

            EventManager.OnGameStart += gameInfo => {
                print("loading level");
                EventManager.LoadLevel(_gameInfo);
            };

            EventManager.OnLevelLoaded += (levelData) => {
                print("level loaded and presented");
                fsm.Fire(GameStateTrigger.Next); // goto Cutscene
            };

            if (_gameInfo.IsMultiplayer) {
                StartMultiplayer();
            }
            else {
                StartSingleplayer();
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void OnDisable()
        {
            fsm.Reset();
        }

        private void StartSingleplayer() {
            _gameInfo.Players.Add(new TGEPlayer());
            EventManager.GameStart(_gameInfo);
        }

        private void StartMultiplayer()
        {
            PhotonManager.Instance.TGEOnAllPlayersJoined += OnAllPlayersJoined;
            _gameInfo.Players.Add(new TGEPlayer());
            EventManager.InitializePhoton(_gameInfo.Players[0]);
        }

        private void OnAllPlayersJoined(Room room) {
            PhotonManager.Instance.TGEOnAllPlayersJoined -= OnAllPlayersJoined;
            for (int i = 0; i < room.PlayerCount; i++) {
                _gameInfo.Players.Add(new TGEPlayer());
            }
            EventManager.GameStart(_gameInfo);
        }

        /// <summary>
        /// State transition Cutscene
        /// </summary>
        private void OnCutsceneStateEnter() {
            print($"{nameof(GameStateManager)}: cutscene");
            // start the cutscene / monologue
            EventManager.OnMonologueEnded += OnMonologueEnded;
            EventManager.MonologueStart(Level.Monologue);
        }

        private void OnMonologueEnded() {
            EventManager.OnMonologueEnded -= OnMonologueEnded;
            fsm.Fire(GameStateTrigger.Next); // goto StartGame
        }

        /// <summary>
        /// State transition GameStart
        /// </summary>
        private void OnStartGameStateEnter() {
            print($"{nameof(GameStateManager)}: game start");
            EventManager.InitializeUi(_gameInfo);

            print("Commands allowed:");
            foreach (var allowedCommand in _gameInfo.AllowedCommands) {
                print($"    - {allowedCommand.Name}");
            }

            fsm.Fire(GameStateTrigger.Next); // goto EditSequence
        }

        /// <summary>
        /// State transition EditSequence
        /// </summary>
        private void OnEditSequenceStateEnter() {
            print($"{nameof(GameStateManager)}: edit");
            // allow players to interact with game world
            EventManager.UserInputEnable();
            EventManager.LevelReset(_gameInfo, _gameInfo.Players.Select(x => x.Player).ToList());
            EventManager.OnReadyButtonClicked += OnReadyButtonClicked;
        }

        private void OnReadyButtonClicked() {
            EventManager.OnReadyButtonClicked -= OnReadyButtonClicked;
            fsm.Fire(GameStateTrigger.Next); // goto ReadyAndWaiting

            // register if sequence is changed
            EventManager.OnSequenceChanged += OnSequenceChanged;

            EventManager.OnAllPlayersReady += () => {
                print("all players are ready!");
                fsm.Fire(GameStateTrigger.Next); // goto Simulate
            };
        }

        private void OnSequenceChanged(List<BaseCommand> commands) {
            EventManager.OnSequenceChanged -= OnSequenceChanged;
            fsm.Fire(GameStateTrigger.Back);
        }

        /// <summary>
        /// State transition ReadyAndWaitingForPlayers
        /// </summary>
        private void OnPlayerReadyStateEnter() {
            print($"{nameof(GameStateManager)}: ready and waiting for other players");
            // if all players are ready; goto Simulate
        }

        /// <summary>
        /// State transition Simulate
        /// </summary>
        private void OnSimulateStateEnter() {
            print($"{nameof(GameStateManager)}: simulating");
            EventManager.UserInputDisable();
            EventManager.OnStopButtonClicked += OnStopButtonClicked;
            EventManager.Simulate();
            EventManager.OnAllLevelGoalsReached += OnAllLevelGoalsReached;
        }

        private void OnStopButtonClicked() {
            EventManager.OnStopButtonClicked -= OnStopButtonClicked;
            fsm.Fire(GameStateTrigger.Back);
        }

        private void OnAllLevelGoalsReached() {
            EventManager.OnAllLevelGoalsReached -= OnAllLevelGoalsReached;
            fsm.Fire(GameStateTrigger.Next); // goto LevelComplete
        }

        /// <summary>
        /// State transition LevelComplete
        /// </summary>
        private void OnLevelCompleteStateEnter() {
            print($"{nameof(GameStateManager)}: level complete!");
            StartCoroutine(ReloadLevel());
        }

        private IEnumerator ReloadLevel() {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public enum GameState {
        Init,
        Cutscene,
        GameStart,
        EditSequence,
        ReadyAndWaitingForPlayers,
        Simulate,
        LevelComplete
    }

    public enum GameStateTrigger {
        Next,
        Back
    }
}