using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Scripts.Photon;
using System;


public class RoomManager : Photon.MonoBehaviour
{
    public GameObject playerPrefab;
    private event UnityAction SpawnReplicated;

    private List<RoomInfo> photonRooms;
    private RoomListView roomView;
    public GameObject roomPanel;


    public void Awake()
    {
        photonRooms = new List<RoomInfo>();

        roomView = roomPanel.GetComponents<RoomListView>()[0];
        print(roomView);
        Assert.IsNotNull(roomView);
    }

    //Lets connect two users to Photon and a lobby (+room)
    void Start()
    {
        PhotonNetwork.autoJoinLobby = true;
        Assert.IsTrue(PhotonNetwork.ConnectUsingSettings("1.0"));

        Debug.Log(PhotonNetwork.connectionState);
        PhotonManager.Instance.TGEOnJoinRandomRoomFailed += (object[] codeAndMsg) => { print("Join random room Failed"); Assert.IsTrue(PhotonNetwork.CreateRoom("RoomLocal")); };
        PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { print("Join room failed"); };

        PhotonManager.Instance.TGEOnJoinedLobby += () =>
        {
            Array.ForEach(PhotonNetwork.GetRoomList(), x => photonRooms.Add(x));
            UpdateGUI();
            Debug.Log("We joined the lobby!");

            if (!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(PhotonNetwork.CreateRoom("RoomLocal"));
            print(PhotonNetwork.inRoom);
            PhotonManager.Instance.TGEOnPhotonPlayerConnected += (PhotonPlayer player) =>
            {
                Debug.Log("Player is here, lets see if somebody else joins");
                //We can only continue here if we have two players, multiplayer is no fun alone
                if (PhotonNetwork.playerList.Length < 2) return;
                Debug.Log("Player joined WOHOO");
                //PhotonNetwork.room.IsOpen = false;
                int index = PhotonNetwork.isMasterClient ? 0 : 1;

                //SpawnReplicated += () => { Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); };

                GameManager.GetInstance().StartMultiplayerGame();

                PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) =>
                {
                    Assert.IsTrue(PhotonNetwork.CreateRoom(null));
                };

                Debug.Log("Connected to photon: " + PhotonNetwork.room + PhotonNetwork.room.PlayerCount);
            };
        };
    }

    public void UpdateGUI()
    {
        Assert.IsNotNull(roomView);
        roomView.ToString();
        roomView.UpdateListView(photonRooms);
    }

    public void AddRoom(RoomInfo room)
    {
        this.photonRooms.Add(room);
    }

    public void CloseRoom(RoomInfo room)
    {
        this.photonRooms.Remove(room);
    }
}

