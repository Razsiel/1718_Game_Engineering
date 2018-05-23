using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Lib.Extensions;
using Assets.Scripts.Photon;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class PhotonConnectionManager : global::Photon.MonoBehaviour
    {
        #region classdata
        public static PhotonConnectionManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private static PhotonConnectionManager _instance;
        //public GameObject BtnCreateRoom;
        //public RoomSelectView RoomSelectCanvas;
        //public InRoomView InRoomCanvas;
        public RoomListView RoomListView;
        public const string ReadyKey = "ready";
        private GameInfo _gameInfo;

        [SerializeField]
        public SceneField LevelSelectScene;
        #endregion

        private void Awake()
        {
            _instance = this;
            GlobalData.SceneDataLoader.OnSceneLoaded += gameinfo =>
            {
                this._gameInfo = gameinfo;
            };
            Init();
        }

        public void Init()
        {
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.automaticallySyncScene = true;

            var connected = PhotonNetwork.ConnectUsingSettings("1.1");
            //Assert.IsTrue(connected);
            print("lobby: " + PhotonNetwork.lobby + "In lobby: " + PhotonNetwork.insideLobby);
            //Show Error Because we dont have internet
            //if(connected)
            //    RoomEventManager.OnPhotonConnected += RoomListManager.Instance.UpdateRooms;
            print("photon connected = " + connected);

            var customProperties = new Hashtable() { { ReadyKey, false } };
            PhotonNetwork.player.SetCustomProperties(customProperties);

            //Implement Callbacks
            //RoomEventManager.OnLocalPlayerJoinedRoom += OnLocalPlayerJoinedRoom;
            //RoomEventManager.OnLocalPlayerLeftRoom += OnLocalPlayerLeftRoom;

            RoomEventManager.OnPhotonReceivedRoomListUpdate += UpdateRooms;
            RoomEventManager.OnNetworkPlayerJoinedRoom += NetworkPlayerChanged;
            RoomEventManager.OnNetworkPlayerLeftRoom += NetworkPlayerChanged;
            RoomEventManager.OnLocalPlayerReadyStateChanged += OnLocalPlayerReadyStateChanged;
            RoomEventManager.OnPlayerPropertiesChanged += OnPlayerPropertiesChanged;
        }

        private void OnPlayerPropertiesChanged(object[] playerAndProperties)
        {
            print("someones properties changed");
            var player = (PhotonPlayer)playerAndProperties[0];
            var hashtable = (Hashtable)playerAndProperties[1];
            bool ready;
            hashtable.TryGetTypedValue(ReadyKey, out ready);

            //Is the player a network player
            if (!player.Equals(PhotonNetwork.player))           
                RoomEventManager.OnNetworkPlayerPropertiesChanged?.Invoke(ready);
            
            //Is everyone ready?
            if (AreAllPlayersReady())
                RoomEventManager.OnAllPlayersReady?.Invoke();

            //Somebody unreadied
            if (!ready)
                RoomEventManager.OnAnyPlayerUnready?.Invoke();
        }

        private bool AreAllPlayersReady()
        {
            if (PhotonNetwork.playerList.Count() < 2) return false;
            var allReady = true;
            foreach (var player in PhotonNetwork.playerList)
            {
                var table = player.CustomProperties;
                bool ready;
                table.TryGetTypedValue(ReadyKey, out ready);
                if (!ready) allReady = false;
            }

            return allReady;
        }

        private void OnLocalPlayerReadyStateChanged(bool ready)
        {
            print("local properties changed");
            var customProperties = new Hashtable() { { ReadyKey, ready } };
            PhotonNetwork.player.SetCustomProperties(customProperties);
        }

        public void UpdateRooms()
        {
            var rooms = PhotonNetwork.GetRoomList();
            RoomListView.UpdateListView(rooms);
        }

        private void NetworkPlayerChanged(PhotonPlayer player)
        {
            RoomEventManager.OnNetworkPlayerChanged?.Invoke(this.GetAllPlayersInRoom());
        }

        public void CreateRoom(string roomName)
        {
            var roomOptions = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 2
            };
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        /// <summary>
        /// Gets the players in the room ordered by who is the masterclient (masterclient is first in the list)
        /// </summary>
        /// <returns></returns>
        public List<PhotonPlayer> GetAllPlayersInRoom()
        {
            return PhotonNetwork.playerList.OrderByDescending(x => x.IsMasterClient).ToList();
        }

        public void SendGoToLevelSelect()
        {
            //if (PhotonNetwork.player.IsMasterClient)
            //    this.photonView.RPC(nameof(GoToLevelSelect), PhotonTargets.All);


            if (PhotonNetwork.player.IsMasterClient)
            {
                this.photonView.RPC(nameof(SetPlayers), PhotonTargets.All);
                PhotonNetwork.LoadLevel(LevelSelectScene);
            }
        }

        [PunRPC]
        public void SetPlayers()
        {
            _gameInfo.Players = new List<TGEPlayer>();
            var players = GetAllPlayersInRoom();
            foreach (var p in players)
                _gameInfo.Players.Add(new TGEPlayer { photonPlayer = p });

            //SceneManager.LoadScene(LevelSelectScene);
        }
    }
}