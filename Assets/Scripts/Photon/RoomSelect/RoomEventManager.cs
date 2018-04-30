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
        #endregion
    }

}