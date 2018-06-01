using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomEventManager : PunBehaviour
    {
        #region Events
        public static UnityAction OnPhotonConnected;
        public static UnityAction OnPhotonDisconnected;
        public static UnityAction OnPhotonReceivedRoomListUpdate;
        public static UnityAction OnLocalPlayerJoinedRoom;
        public static UnityAction<PhotonPlayer> OnNetworkPlayerJoinedRoom;
        public static UnityAction<PhotonPlayer> OnNetworkPlayerLeftRoom;
        public static UnityAction OnLocalPlayerLeftRoom;
        public static UnityAction<bool> OnLocalPlayerReadyStateChanged;

        public static UnityAction<List<PhotonPlayer>> OnNetworkPlayerChanged;
        public static UnityAction<object[]> OnPlayerPropertiesChanged;
        public static UnityAction OnMasterClientChanged;

        //Network players properties changed (for now only readystate is handled here)
        public static UnityAction<bool> OnNetworkPlayerPropertiesChanged;
        
        //Called when every player in the room has readied up
        public static UnityAction OnAllPlayersReady;
        //Called when any player in the room unready's himself
        public static UnityAction OnAnyPlayerUnready; 
              
        //Called when the localplayer becomes the masterclient (through creating the room or someone leaving)
        public static UnityAction OnBecomingMasterClient;

        //Called when the scene is done spawning all nessescary gameobjects
        public static UnityAction OnAllGameObjectsSpawned;
        #endregion

        #region EventInvokes
        public override void OnJoinedLobby()
        {
            OnPhotonConnected?.Invoke();
        }

        public override void OnDisconnectedFromPhoton()
        {
            OnPhotonDisconnected?.Invoke();
        }

        public static void AllGameObjectsSpawned()
        {
            OnAllGameObjectsSpawned?.Invoke();
        }

        public override void OnReceivedRoomListUpdate()
        {
            OnPhotonReceivedRoomListUpdate?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            OnLocalPlayerJoinedRoom?.Invoke();
            if (PhotonNetwork.player.IsMasterClient)
                OnBecomingMasterClient?.Invoke();
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            OnNetworkPlayerJoinedRoom?.Invoke(newPlayer);
        }

        public override void OnLeftRoom()
        {
            OnLocalPlayerLeftRoom?.Invoke();
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            OnNetworkPlayerLeftRoom?.Invoke(player);
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndProperties)
        {
            OnPlayerPropertiesChanged?.Invoke(playerAndProperties);
        }

        public override void OnMasterClientSwitched(PhotonPlayer player)
        {
            OnMasterClientChanged?.Invoke();
            if(player.Equals(PhotonNetwork.player))
                OnBecomingMasterClient?.Invoke();
        }
        #endregion
    }

}