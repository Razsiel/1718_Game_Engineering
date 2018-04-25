using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Photon;
using SmartLocalization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Assets.Scripts {
    public class GameManager : MonoBehaviour {
        private static GameManager _instance;

        // Use PrefabContainer to access project-files
        public PrefabContainer PrefabContainer;

        public List<TGEPlayer> Players;

        public CommandLibrary CommandLibrary;


        public bool IsMultiPlayer = false;

        //To be filled with the level (from UI layer)
        //For now the editor can handle this for us
        //To Be Added: [HideInInspector]
        public LevelData LevelData;


        #region GLOBAL_EVENTS

        public UnityAction<Player> PlayerInitialized;

//        public UnityAction PlayersInitialized;
        //public UnityAction 

        #endregion

        void Awake() {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);

            Players = new List<TGEPlayer>();
            TGEPlayer player = new TGEPlayer();
            Players.Add(player);
        }

        void Start() {
            // Setup level

            // Temp Players setup (done in awake)
            //Players = new List<TGEPlayer>();
            //TGEPlayer player = new TGEPlayer();
            //Players.Add(player);

            // Start Level Events
            // Make sure de scene contains required GameObjects

            // PrefabContainer filled and available
            PrefabContainer.Initialize();
            AssertAllNotNull();

            // Initialize LevelData with Players
            if (!IsMultiPlayer)
                StartSinglePlayerGame(Players[0]);

            // Initialize UI
            EventManager.InitializeUi();
            EventManager.UserInputDisable();
            EventManager.AudioInitialized();
            EventManager.MonologueInitialized();

            // Link Player2 to Photon instance
            // Start Level
            EventManager.MonologueStart(LevelData.Monologue);
        }

        public void StartSinglePlayerGame(TGEPlayer player /*, LevelData level*/) {
            //TO:DO Start the level given with the local player
            var players = new List<TGEPlayer>() {player};
            CreatePlayers(players);

            this.Players = players;

            //LevelData.Init(players);
            LevelPresenter.Present(LevelData, players);
        }

        public void StartMultiplayerGame(List<TGEPlayer> players /*, LevelData level*/) {
            //PhotonManager.Instance.GetOtherPlayers();
            var orderedList = players.OrderByDescending(x => x.photonPlayer.IsMasterClient).ToList();
            //this.Players = orderedList;

            CreatePlayers(orderedList);
            this.Players = orderedList;
            PhotonManager.Instance.PlayersReady();
            //LevelData.Init(players);
            LevelPresenter.Present(LevelData, orderedList);
            ////Lets do some GameStarting logic here
            //PhotonManager.Instance.StartMultiplayerGame(LevelData, Players);
        }


        private void CreatePlayers(List<TGEPlayer> players) {
            for (int i = 0; i < players.Count; i++) {
                var playerObject = Instantiate(this.PrefabContainer.PlayerPrefab, Vector3.zero, Quaternion.identity,
                                               this.transform);
                var playerComponent = playerObject.GetComponent<Player>();
                playerComponent.PlayerNumber = i;
                playerComponent.Data.GenerateGameObject(playerObject);

                ////playerComponent.IsLocalPlayer = (IsMultiPlayer ? players[i].photonPlayer == PhotonManager.Instance.GetLocalPlayer() : false);

                // Assigning colour to player TODO: dat kan beter!
                if (i == 0)
                {
                    playerObject.GetComponent<Renderer>().material = PrefabContainer.Mat_Orange;
                }
                else
                {
                    playerObject.GetComponent<Renderer>().material = PrefabContainer.Mat_Blue;
                }

                players[i].PlayerObject = playerObject;
                players[i].Player = playerComponent;
                //PlayerInitialized(playerComponent);
                EventManager.PlayerInitialized(playerComponent);

//                if (IsMultiPlayer && players[i].photonPlayer.IsMasterClient)
//                    playerComponent.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                // layerInitialized(playerComponent);
            }
            EventManager.PlayersInitialized();
            EventManager.PlayerColourSet();
        }

        public static GameManager GetInstance() {
            return _instance;
        }

        private bool AssertAllNotNull() {
            Assert.IsNotNull(PrefabContainer);
            Assert.IsNotNull(PrefabContainer.PlayerPrefab);
            //Assert.IsNotNull(PrefabContainer.ImageNotFound);
            Assert.IsNotNull(CommandLibrary);

            return true;
        }
    }
}