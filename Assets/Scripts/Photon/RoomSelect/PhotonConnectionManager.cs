using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Photon;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

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
        public GameObject BtnCreateRoom;
        #endregion

        private void Awake()
        {
            _instance = this;
            Init();
        }

        public void Init()
        {
            PhotonNetwork.autoJoinLobby = true;

            bool connected = PhotonNetwork.ConnectUsingSettings("1.1");
            Assert.IsTrue(connected);
            print("lobby: " + PhotonNetwork.lobby + "In lobby: " + PhotonNetwork.insideLobby);
            //Show Error Because we dont have internet
            //if(connected)
            //    RoomEventManager.OnPhotonConnected += RoomListManager.Instance.UpdateRooms;
            print("photon connected = " + connected);

            //RoomEventManager.OnPhotonConnected += () =>
            //{
            //    BtnCreateRoom.GetComponent<Button>().onClick.AddListener(() => CreateRoom());
            //};
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
    }
}