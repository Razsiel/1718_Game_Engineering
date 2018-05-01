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
        public static UnityAction<bool> OnNetworkPlayerPropertiesChanged;
        public static UnityAction OnAllPlayersReady;
        public static UnityAction OnAnyPlayerUnready;
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

        public override void OnReceivedRoomListUpdate()
        {
            OnPhotonReceivedRoomListUpdate?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            OnLocalPlayerJoinedRoom?.Invoke();
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
        #endregion
    }

}