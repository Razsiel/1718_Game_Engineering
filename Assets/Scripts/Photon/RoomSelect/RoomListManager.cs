using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Photon;
using UnityEngine;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomListManager : MonoBehaviour
    {
        public RoomListView roomListView;

        #region classdata
        public static RoomListManager Instance
        {
            get
            {                
                return _instance;
            }
        }

        private static RoomListManager _instance;
        #endregion

        private void Awake()
        {
            _instance = this;
            RoomEventManager.OnPhotonConnected += () => 
            {                          
                //PhotonNetwork.CreateRoom("Room1"); PhotonNetwork.CreateRoom("Room2", null, TypedLobby.Default);
                print("In lobby: " + PhotonNetwork.insideLobby + "Rooms: " + PhotonNetwork.GetRoomList());
            };
            RoomEventManager.OnPhotonReceivedRoomListUpdate += UpdateRooms;
            RoomEventManager.OnLocalPlayerJoinedRoom += JoinedRoom;          
        }

        private void JoinedRoom()
        {
            print("Joined room: " + PhotonNetwork.room);
        }

        public void UpdateRooms()
        {
            var rooms = PhotonNetwork.GetRoomList();
            print("Rooms: " + rooms.Length);
            roomListView.UpdateListView(rooms);
        }     
    }
}