using System;
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
using Assets.Scripts.Photon.Level;

namespace Assets.Scripts {
    public class GameStateManager : MonoBehaviour {
        private GameInfo _gameInfo;

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
            fsm.Tr(GameState.GameStart, GameStateTrigger.Edit, GameState.EditSequence)
               .On(OnEditSequenceStateEnter);

            // EDITSEQUENCE <-> READYANDWAITINGFORPLAYERS
            fsm.Tr(GameState.EditSequence, GameStateTrigger.Next, GameState.ReadyAndWaitingForPlayers)
               .On(OnPlayerReadyStateEnter)
               .Tr(GameState.ReadyAndWaitingForPlayers, GameStateTrigger.Edit, GameState.EditSequence)
               .On(OnEditSequenceStateEnter);

            // READYANDWAITINGFORPLAYERS -> SIMULATE
            fsm.Tr(GameState.ReadyAndWaitingForPlayers, GameStateTrigger.Simulate, GameState.Simulate)
               .On(OnSimulateStateEnter);

            // SIMULATE -> LEVELCOMPLETE
            fsm.Tr(GameState.Simulate, GameStateTrigger.Next, GameState.LevelComplete)
               .On(OnLevelCompleteStateEnter)
               .Tr(GameState.Simulate, GameStateTrigger.Edit, GameState.EditSequence)
               .On(OnEditSequenceStateEnter);

            GlobalData.SceneDataLoader.OnSceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(string previousSceneName, GameInfo gameInfo) {
            GlobalData.SceneDataLoader.OnSceneLoaded -= OnSceneLoaded;
            this._gameInfo = gameInfo;
        }

        public void Start() {
            Debug.Log($"Start: {nameof(GameStateManager)}. FSM state: {fsm.State}");

            EventManager.OnGameStart += OnGameStart;
            EventManager.OnLevelLoaded += OnLevelLoaded;

            if (_gameInfo.IsMultiplayer) {
                print("GameStateManager: We are gonna start in multiplayer");
                StartMultiplayer();
            }
            else {
                StartSingleplayer();
            }
        }

        void OnDestroy() {
            EventManager.OnGameStart -= OnGameStart;
            EventManager.OnLevelLoaded -= OnLevelLoaded;
            EventManager.OnReadyButtonClicked -= OnReadyButtonClicked;
            EventManager.OnAllPlayersReady -= OnAllPlayersReady;
            EventManager.OnSequenceChanged -= OnSequenceChanged;
            EventManager.OnStopButtonClicked -= OnStopButtonClicked;
            EventManager.OnSimulationStop -= OnSimulationStop;
            EventManager.OnWinScreenContinueClicked -= OnWinScreenContinueClicked;
        }

        private void OnLevelLoaded(GameInfo levelData)
        {
            EventManager.OnLevelLoaded -= OnLevelLoaded;
            print($"{nameof(GameStateManager)}: level loaded and presented. FSM state: {fsm.State}");
            fsm.Fire(GameStateTrigger.Next); // goto Cutscene
        }

        private void OnGameStart(GameInfo gameInfo)
        {
            EventManager.OnGameStart -= OnGameStart;
            print($"{nameof(GameStateManager)}: loading level. FSM state: {fsm.State}");
            EventManager.LoadLevel(_gameInfo);
        }

        public void OnDisable()
        {
            //fsm.Reset();
        }

        private void StartSingleplayer() {
            EventManager.GameStart(_gameInfo);
        }

        private void StartMultiplayer()
        {
            PhotonManager.Instance.TGEOnAllPlayersJoined += OnAllPlayersJoined;
            EventManager.InitializePhoton();
        }

        private void OnAllPlayersJoined(Room room) {
            PhotonManager.Instance.TGEOnAllPlayersJoined -= OnAllPlayersJoined;
            print($"{nameof(GameStateManager)} All players joing room: {room}");
            EventManager.GameStart(_gameInfo);
        }

        /// <summary>
        /// State transition Cutscene
        /// </summary>
        private void OnCutsceneStateEnter() {
            print($"{nameof(GameStateManager)}: cutscene. FSM state: {fsm.State}");
            // start the cutscene / monologue
            EventManager.OnMonologueEnded += OnMonologueEnded;
            EventManager.MonologueStart(_gameInfo.Level.Monologue);
        }

        private void OnMonologueEnded() {
            print($"{nameof(GameStateManager)}: monoloque ended. FSM state: {fsm.State}");
            EventManager.OnMonologueEnded -= OnMonologueEnded;
            fsm.Fire(GameStateTrigger.Next); // goto StartGame
        }

        /// <summary>
        /// State transition GameStart
        /// </summary>
        private void OnStartGameStateEnter() {
            print($"{nameof(GameStateManager)}: game start. FSM state: {fsm.State}");
            EventManager.InitializeUi(_gameInfo);

            print("Commands allowed:");
            foreach (var allowedCommand in _gameInfo.AllowedCommands) {
                print($"    - {allowedCommand.Name}");
            }

            fsm.Fire(GameStateTrigger.Edit); // goto EditSequence
        }

        /// <summary>
        /// State transition EditSequence
        /// </summary>
        private void OnEditSequenceStateEnter() {
            print($"{nameof(GameStateManager)}: edit. FSM state: {fsm.State}");
            EventManager.OnSequenceChanged -= OnSequenceChanged;
            // allow players to interact with game world
            EventManager.OnReadyButtonClicked += OnReadyButtonClicked;
            EventManager.UserInputEnable();
            EventManager.LevelReset(_gameInfo, _gameInfo.Players.Select(x => x.Player).ToList());
        }

        private void OnReadyButtonClicked() {
            EventManager.OnReadyButtonClicked -= OnReadyButtonClicked;
            print($"{nameof(GameStateManager)}: ready! FSM state: {fsm.State}");
            
            EventManager.OnAllPlayersReady += OnAllPlayersReady;

            fsm.Fire(GameStateTrigger.Next); // goto ReadyAndWaiting
        }

        private void OnAllPlayersReady() {
            EventManager.OnAllPlayersReady -= OnAllPlayersReady;
            print("all players are ready!");
            fsm.Fire(GameStateTrigger.Simulate); // goto Simulate
        }

        private void OnSequenceChanged(List<BaseCommand> commands) {
            print($"{nameof(GameStateManager)}: sequence was changed. Going back to edit sequence! FSM state: {fsm.State}");
            EventManager.OnSequenceChanged -= OnSequenceChanged;
            fsm.Fire(GameStateTrigger.Edit);
        }

        /// <summary>
        /// State transition ReadyAndWaitingForPlayers
        /// </summary>
        private void OnPlayerReadyStateEnter() {
            print($"{nameof(GameStateManager)}: ready and waiting for other players. FSM state: {fsm.State}");
            
            EventManager.OnSequenceChanged += OnSequenceChanged;

            if (!_gameInfo.IsMultiplayer) {
                EventManager.AllPlayersReady();
            }
        }

        /// <summary>
        /// State transition Simulate
        /// </summary>
        private void OnSimulateStateEnter() {
            print($"{nameof(GameStateManager)}: simulating. FSM state: {fsm.State}");
            EventManager.UserInputDisable();
            EventManager.OnStopButtonClicked += OnStopButtonClicked;
            EventManager.OnSimulationStop += OnSimulationStop;
            EventManager.Simulate(_gameInfo.Level, _gameInfo.Players);
            EventManager.OnWinScreenContinueClicked += OnWinScreenContinueClicked;
        }

        private void OnSimulationStop()
        {
            print($"{nameof(GameStateManager)}: simulation stop. FSM state: {fsm.State}");
            EventManager.OnSimulationStop -= OnSimulationStop;
            fsm.Fire(GameStateTrigger.Edit);
        }

        private void OnStopButtonClicked() {
            print($"{nameof(GameStateManager)}: stop button clicked. FSM state: {fsm.State}");
            EventManager.OnStopButtonClicked -= OnStopButtonClicked;
            if (!_gameInfo.IsMultiplayer)
                EventManager.SimulationStop();          
        }

        private void OnWinScreenContinueClicked()
        {
            EventManager.OnWinScreenContinueClicked -= OnWinScreenContinueClicked;
            fsm.Fire(GameStateTrigger.Next); // goto LevelComplete
        }

        /// <summary>
        /// State transition LevelComplete
        /// </summary>
        private void OnLevelCompleteStateEnter() {
            print($"{nameof(GameStateManager)}: level complete! FSM state: {fsm.State}");
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
        Back,

        Edit,
        Simulate
    }
}