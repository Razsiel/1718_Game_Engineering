using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Reflection;
using Assets.Data.Levels;
using UnityEngine.Assertions;
using Photon;
using Assets.Scripts.DataStructures;
using System.Linq;

namespace Assets.Scripts.Photon.Level
{
    public class PhotonManager : PunBehaviour
    {
        //Backing field of our singleton instance
        private static PhotonManager _instance;
        private RoomManager _roomManager;

        private GameInfo _gameInfo;

        //Events to react on
        public UnityAction TGEOnJoinedLobby;
        public UnityAction<PhotonPlayer> TGEOnPhotonPlayerConnected;
        public UnityAction<object[]> TGEOnJoinRandomRoomFailed;
        public UnityAction<object[]> TGEOnJoinRoomFailed;
        public UnityAction TGEOnCreatedRoom;
        public UnityAction TGEOnJoinedRoom;
        public UnityAction TGEOnLeftRoom;
        public UnityAction<PhotonPlayer> TGEOnPhotonPlayerDisconnected;
        public UnityAction TGEOnPlayersCreated;
        public UnityAction<Room> TGEOnAllPlayersJoined;

        //Our singleton instance of the Photonmanager
        public static PhotonManager Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        public RoomManager RoomManager
        {
            get { return _roomManager; }
            set { _roomManager = value; }
        }

        //Private because of SingleTon
        private PhotonManager() { }

        void Awake()
        {
            Instance = this;
            RoomManager = gameObject.GetComponent<RoomManager>();
            Assert.IsNotNull(_roomManager);
            EventManager.OnGameStart += gameInfo => {
                _gameInfo = gameInfo;
            };
        }
             
        public void PlayersReady()
        {
            TGEOnPlayersCreated?.Invoke();
        }

        public void CreateRoom(string roomName)
        {
            print("Creating Room!");
            PhotonNetwork.CreateRoom(roomName);
        }

        public void JoinRoom(string roomName)
        {
            print("Joining Room!");
            PhotonNetwork.JoinRoom(roomName);
        }

        #region PhotonCallbacks
        public override void OnJoinedLobby()
        {
            Debug.Log("InPUNCAll");
            TGEOnJoinedLobby?.Invoke();
        }

        public override void OnDisconnectedFromPhoton()
        {
            //TGEOnDisconnectedFromPhoton?.Invoke();
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            print("InPhotonPlayerConnected");
            TGEOnPhotonPlayerConnected?.Invoke(newPlayer);
        }

        public override void OnConnectedToPhoton()
        {
            Debug.Log("Connected");
        }

        public override void OnCreatedRoom()
        {
            TGEOnCreatedRoom?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            TGEOnJoinedRoom?.Invoke();
            //TGEOnPhotonPlayerConnected?.Invoke(PhotonNetwork.player);
        }

        
        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            TGEOnJoinRoomFailed?.Invoke(codeAndMsg); 
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            TGEOnJoinRandomRoomFailed?.Invoke(codeAndMsg);
        }

        public override void OnLeftRoom()
        {
            TGEOnLeftRoom?.Invoke();
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            TGEOnPhotonPlayerDisconnected?.Invoke(otherPlayer);
        }

        public void OnAllPlayersJoined(Room room) {
            TGEOnAllPlayersJoined?.Invoke(room);
        }
        #endregion
    }
}