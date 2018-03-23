using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System;

public class RoomManager : TGEMonoBehaviour
{
    public GameObject playerPrefab;
    private event UnityAction SpawnReplicated;

    private List<RoomInfo> photonRooms;
    private RoomListView roomView;

    //Lets connect two users to Photon and a lobby (+room)
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("1.0");
        roomView = new RoomListView();
        Debug.Log("In Photon Start.");
        PhotonNetwork.autoJoinLobby = true;
        Debug.Log(PhotonNetwork.connectionState);
        PhotonManager.Instance.TGEOnJoinedLobby += () =>
        {

            Array.ForEach(PhotonNetwork.GetRoomList(), x => photonRooms.Add(x));
            roomView.UpdateListView(photonRooms);
            Debug.Log("We joined the lobby!");
            PhotonManager.Instance.TGEOnPhotonPlayerConnected += (PhotonPlayer player) =>
            {
                Debug.Log("Player is here?");
                if(PhotonNetwork.playerList.Length < 2) return;
                Debug.Log("Player joined WOHOO");
                PhotonNetwork.room.IsOpen = false;
                int index = PhotonNetwork.isMasterClient ? 0 : 1;

                SpawnReplicated += () => { Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); };

                if(!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(PhotonNetwork.CreateRoom(null));

                PhotonManager.Instance.TGEOnJoinLobbyFailed += (object[] codeAndMsg) => {
                    Assert.IsTrue(PhotonNetwork.CreateRoom(null));
                };

                Debug.Log("Connected to photon: " + PhotonNetwork.room + PhotonNetwork.room.PlayerCount);
            };
        };
    }
        void UpdateGUI()
        {
             roomView.UpdateListView(photonRooms);
        }
    

    
    

}
