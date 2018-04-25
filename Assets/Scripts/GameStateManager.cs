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
using UnityEngine.SceneManagement;

namespace Assets.Scripts {
    public class GameStateManager : SingleMonobehaviour<GameStateManager> {
        public LevelData level;
        public bool IsMultiPlayer;

        private TGEPlayer localPlayer;

        private TinyStateMachine<GameState, GameStateTrigger> fsm;

        public void Awake() {
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
            print($"{nameof(GameStateManager)}: loading level");
            EventManager.OnLevelLoaded += (levelData) => {
                print("level loaded and presented");
                fsm.Fire(GameStateTrigger.Next); // goto Cutscene
            };

            localPlayer = new TGEPlayer();
            if (IsMultiPlayer) {
                PhotonManager.Instance.TGEOnAllPlayersJoined += OnAllPlayersJoined;
                EventManager.InitializePhoton(new TGEPlayer());
            }
            else {
                var players = new List<TGEPlayer>() {
                    localPlayer
                };
                EventManager.LoadLevel(level, players);
            }
        }

        private void OnAllPlayersJoined(Room room) {
            PhotonManager.Instance.TGEOnAllPlayersJoined -= OnAllPlayersJoined;
            var players = new List<TGEPlayer>();
            players.Add(localPlayer);
            for (int i = 0; i < room.PlayerCount - 1; i++) {
                players.Add(new TGEPlayer());
            }
            EventManager.LoadLevel(level, players);
        }

        public void OnDisable() {
            fsm.Reset();
        }

        /// <summary>
        /// State transition Cutscene
        /// </summary>
        private void OnCutsceneStateEnter() {
            print($"{nameof(GameStateManager)}: cutscene");
            // start the cutscene / monologue
            EventManager.OnMonologueEnded += OnMonologueEnded;
            EventManager.MonologueStart(level.Monologue);
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
            fsm.Fire(GameStateTrigger.Next); // goto EditSequence
        }

        /// <summary>
        /// State transition EditSequence
        /// </summary>
        private void OnEditSequenceStateEnter() {
            print($"{nameof(GameStateManager)}: edit");
            // allow players to interact with game world
            EventManager.UserInputEnable();
            EventManager.LevelReset(GameManager.GetInstance().LevelData, GameManager.GetInstance().Players.Select(x => x.Player).ToList());
            EventManager.OnReadyButtonClicked += OnReadyButtonClicked;
        }

        private void OnReadyButtonClicked() {
            EventManager.OnReadyButtonClicked -= OnReadyButtonClicked;
            fsm.Fire(GameStateTrigger.Next); // goto ReadyAndWaiting

            // register if sequence is changed
            EventManager._SequenceChanged += OnSequenceChanged;

            EventManager.OnAllPlayersReady += () => {
                print("all players are ready!");
                fsm.Fire(GameStateTrigger.Next); // goto Simulate
            };
        }

        private void OnSequenceChanged(List<BaseCommand> commands) {
            EventManager._SequenceChanged -= OnSequenceChanged;
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
            EventManager._StopButtonClicked += OnStopButtonClicked;
            EventManager.Simulate();
            EventManager.OnAllLevelGoalsReached += OnAllLevelGoalsReached;
        }

        private void OnStopButtonClicked() {
            EventManager._StopButtonClicked -= OnStopButtonClicked;
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