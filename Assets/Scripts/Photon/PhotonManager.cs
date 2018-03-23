using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PhotonManager
{
    //Our singleton instance of the Photonmanager
    public static PhotonManager Instance
    {
        get { if(instance == null) { instance = new PhotonManager(); } return instance; }
        private set { instance = value; }
    }

    //Backing field of our singleton instance
    private static PhotonManager instance;

    public RoomManager RoomManager
    {
        get { if(roomManager == null) roomManager = new RoomManager(); return roomManager; }
        private set { roomManager = value; }
    }
    public RoomManager roomManager;

    public event UnityAction TGEOnJoinedLobby;
    public event UnityAction<PhotonPlayer> TGEOnPhotonPlayerConnected;
    public event UnityAction<object[]> TGEOnJoinLobbyFailed;

    //private PhotonCallbacks callbacks = new PhotonCallbacks();

    private PhotonManager() { }

    #region PhotonCallbacks
    void OnJoinedLobby()
    {
        Debug.Log("InPUNCAll");
        TGEOnJoinedLobby.Invoke();
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        TGEOnPhotonPlayerConnected.Invoke(newPlayer);
    }

    void OnConnectedToPhoton()
    {
        Debug.Log("Connected");
    }
    #endregion
}

