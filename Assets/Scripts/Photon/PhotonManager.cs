﻿using UnityEngine;
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

namespace Assets.Scripts.Photon
{
    public class PhotonManager : PunBehaviour
    {
        //Backing field of our singleton instance
        private static PhotonManager _instance;
        private RoomManager _roomManager;

        //Events to react on
        public event UnityAction TGEOnJoinedLobby;
        public event UnityAction<PhotonPlayer> TGEOnPhotonPlayerConnected;
        public event UnityAction<object[]> TGEOnJoinRandomRoomFailed;
        public event UnityAction<object[]> TGEOnJoinRoomFailed;
        public event UnityAction TGEOnCreatedRoom;
        public event UnityAction TGEOnJoinedRoom;
        public event UnityAction TGEOnLeftRoom;
        public event UnityAction<PhotonPlayer> TGEOnPhotonPlayerDisconnected;
        public event UnityAction TGEOnPlayersCreated;
        public event UnityAction TGEOnDisconnectedFromPhoton;

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
        }
       
        /// <summary>
        /// To be called later when a startbutton is called
        /// </summary>
        /// <param name="levelData">The level we want to start</param>
        public void StartMultiplayerGame(LevelData levelData, List<TGEPlayer> players)
        {
            foreach (TGEPlayer p in players.Where(x => !x.photonPlayer.IsLocal))
            {
                //Send RPC on every player to load the level
                
            }
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

        public void GetOtherPlayers()
        {
            photonView.RPC(nameof(ReturnOtherPlayers), PhotonTargets.Others);
        }

        public void ReturnOtherPlayers()
        {
            photonView.RPC(nameof(ReceiveOtherPlayers), PhotonTargets.Others, GameManager.GetInstance().Players[0]);
        }

        public void ReceiveOtherPlayers(GameObject playerObject)
        {
            LevelManager.Instance.Players[1].PlayerObject = playerObject;
            GameManager.GetInstance().Players[1].PlayerObject = playerObject;
            LevelManager.Instance.Players[1].Player = playerObject.GetComponent<Player>();
            GameManager.GetInstance().Players[1].Player = playerObject.GetComponent<Player>();
        }

        #region PhotonCallbacks
        public override void OnJoinedLobby()
        {
            Debug.Log("InPUNCAll");
            TGEOnJoinedLobby?.Invoke();
        }

        public override void OnDisconnectedFromPhoton()
        {
            TGEOnDisconnectedFromPhoton?.Invoke();
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
        #endregion
    }
}