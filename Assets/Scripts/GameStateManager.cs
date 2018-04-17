using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using Assets.Scripts.Photon;
using M16h;
using UnityEngine;

namespace Assets.Scripts {
    // TODO: Make singleton-monobehaviour
    public class GameStateManager : MonoBehaviour {
        public Monologue monologue;

        private TinyStateMachine<GameState, GameStateTrigger> fsm;

        public GameStateManager() {
            fsm = new TinyStateMachine<GameState, GameStateTrigger>(GameState.Init);

            // START GAME -> CUTSCENE
            fsm.Tr(GameState.Init, GameStateTrigger.Next, GameState.Cutscene)
               .On(OnCutsceneStateEnter);

            // CUTSCENE -> GAMESTART
            fsm.Tr(GameState.Cutscene, GameStateTrigger.Next, GameState.GameStart)
               .On(OnStartGameStateEnter);

            // GAMESTART -> EDITSEQUENCE
            fsm.Tr(GameState.GameStart, GameStateTrigger.Next, GameState.EditSequence);

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
            if (true /*PhotonManager.Instance.AllPlayersConnected*/) {
                fsm.Fire(GameStateTrigger.Next); // goto Cutscene
            }
        }

        public void OnDisable() {
            fsm.Reset();
        }

        /// <summary>
        /// State transition Cutscene
        /// </summary>
        private void OnCutsceneStateEnter() {
            print("cutscene");
            // start the cutscene / monologue
            EventManager.OnMonologueStart(monologue); // TODO: Get the level monologue
            EventManager.MonologueEnded += () => {
                fsm.Fire(GameStateTrigger.Next); // goto StartGame
            };

            fsm.Fire(GameStateTrigger.Next);
        }

        /// <summary>
        /// State transition GameStart
        /// </summary>
        private void OnStartGameStateEnter() {
            print("game start");
            fsm.Fire(GameStateTrigger.Next); // goto EditSequence
        }

        /// <summary>
        /// State transition EditSequence
        /// </summary>
        private void OnEditSequenceStateEnter() {
            print("edit");
            // allow players to interact with game world
            EventManager.OnEnableUserInput();
            EventManager.ReadyButtonClicked += OnReadyButtonClicked;
        }

        private void OnReadyButtonClicked() {
            fsm.Fire(GameStateTrigger.Next); // goto ReadyAndWaiting
            EventManager.ReadyButtonClicked -= OnReadyButtonClicked;

            // register if sequence is changed
            EventManager.SequenceChanged += OnSequenceChanged;

            if (true /*PhotonManager.Instance.AllPlayersReady*/) {
                print("all players are ready!");
                fsm.Fire(GameStateTrigger.Next); // goto Simulate
            }
        }

        private void OnSequenceChanged(List<BaseCommand> commands) {
            fsm.Fire(GameStateTrigger.Back);
            EventManager.SequenceChanged -= OnSequenceChanged;
        }

        /// <summary>
        /// State transition ReadyAndWaitingForPlayers
        /// </summary>
        private void OnPlayerReadyStateEnter() {
            print("ready and waiting");
            // if all players are ready; goto Simulate
        }

        /// <summary>
        /// State transition Simulate
        /// </summary>
        private void OnSimulateStateEnter() {
            print("simulating");
            EventManager.OnDisableUserInput();
            EventManager.StopButtonClicked += () => { fsm.Fire(GameStateTrigger.Back); };
            EventManager.OnSimulate();
            EventManager.AllLevelGoalsReached += () => {
                fsm.Fire(GameStateTrigger.Next); // goto LevelComplete
            };
        }

        /// <summary>
        /// State transition LevelComplete
        /// </summary>
        private void OnLevelCompleteStateEnter() {
            print("level complete!");
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