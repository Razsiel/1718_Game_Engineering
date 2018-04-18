using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Scripts.Photon;
using System;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts.DataStructures;
using Assets.Scripts;
using Assets.Scripts.Lib.Helpers;


public class RoomManager : Photon.MonoBehaviour
{
    public GameObject playerPrefab;
    private event UnityAction SpawnReplicated;

    public NetPlayerSeqView networkPlayerSequenceBarView;
    private List<RoomInfo> photonRooms;
    private RoomListView roomView;
    public GameObject roomPanel;

    public void Awake() {
        EventManager.InitializePhoton += Init;
    }

    //Lets connect two users to Photon and a lobby (+room)
    void Init(TGEPlayer localPlayer)
    {
        Assert.IsNotNull(localPlayer);
        PhotonNetwork.autoJoinLobby = true;
        Assert.IsTrue(PhotonNetwork.ConnectUsingSettings("1.0"));

        Debug.Log(PhotonNetwork.connectionState);
        PhotonManager.Instance.TGEOnJoinRandomRoomFailed += OnJoinRandomRoomFailed;
        PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { print("Join room failed, Code: " + codeAndMsg[0] + " Message: " + codeAndMsg[1]); };

        PhotonManager.Instance.TGEOnJoinedLobby += () =>
        {
            localPlayer.photonPlayer = PhotonNetwork.player;

            //Gone for now
            //UpdateGUI();

            Debug.Log("We joined the lobby!");

            if(!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(CreateRoom());

            PhotonManager.Instance.TGEOnPhotonPlayerConnected += OnPhotonPlayerConnected;

            PhotonManager.Instance.TGEOnJoinedRoom += () => {
                Debug.Log("Joined a room");

                PhotonManager.Instance.TGEOnPlayersCreated += () =>
                {
                    print("Players are Created!");
                    localPlayer.Player.SequenceChanged += (List<BaseCommand> sequence) =>
                    {
                        //PhotonPlayer otherPlayer = GameManager.GetInstance().Players.Single(x => !x.photonPlayer.IsLocal).photonPlayer;
                        print("about to send rpc");
                        ListContainer<CommandHolder> listCommands = new ListContainer<CommandHolder>() { list = new List<CommandHolder>() };
                        var commandOptions = GameManager.GetInstance().CommandLibrary.Commands;
                        //listCommands.list = sequence.Select(x => commandOptions.GetKey(x));  //GameManager.GetInstance().CommandLibrary.Commands
                        foreach(BaseCommand c in sequence)
                            listCommands.list.Add(new CommandHolder(commandOptions.GetKey(c)));
                        string seqJson = JsonUtility.ToJson(listCommands);
                        string methodToCall = nameof(UpdateOtherPlayersCommands);
                        this.photonView.RPC(methodToCall, PhotonTargets.Others, seqJson);
                        print("done sending");
                    };


                    localPlayer.Player.OnPlayerReady += () =>
                    {                 
                        localPlayer.Player.IsReady = true;

                        if(!PhotonNetwork.player.IsMasterClient)
                            this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.MasterClient);
                    };
                };
            };
        };
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Player joined WOHOO");
        PhotonManager.Instance.TGEOnLeftRoom += () => { };
        PhotonManager.Instance.TGEOnPhotonPlayerDisconnected += (PhotonPlayer otherPlayer) => {
            //gameManager.Players.RemoveAll(x => x != gameManager.Players.Single(y => y.photonPlayer.IsLocal));
        };

        //We can only continue here if we have two players, multiplayer is no fun alone
        if (PhotonNetwork.room.PlayerCount != PhotonNetwork.room.MaxPlayers)
            return;

        if (PhotonNetwork.player.IsMasterClient) {
            PhotonNetwork.room.IsOpen = false;
            this.photonView.RPC(nameof(OnAllPlayersJoined), PhotonTargets.All);
        }

        PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { Assert.IsTrue(PhotonNetwork.CreateRoom(null)); };
        Debug.Log($"Connected to photon: {PhotonNetwork.room}");
    }

    private bool CreateRoom() {
        print("Creating a new room all for myself!");
        var roomOptions = new RoomOptions {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 2
        };
        return PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default);
    }

    private void OnJoinRandomRoomFailed(object[] codeAndMsg) {
        print($"Join random room Failed, Code: {codeAndMsg[0]} Message: {codeAndMsg[1]}");
        Assert.IsTrue(CreateRoom());
    }

    [PunRPC]
    private void OnAllPlayersJoined() {
        PhotonManager.Instance.OnAllPlayersJoined(PhotonNetwork.room);
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

    [PunRPC]
    public void UpdateOtherPlayersCommands(string commandsJson, PhotonMessageInfo info)
    {
        print("Got RPC");
        ListContainer<CommandHolder> commands = JsonUtility.FromJson<ListContainer<CommandHolder>>(commandsJson);
        List<CommandEnum> commandEnums = commands.list.Select(x => x.command).ToList();
        GameManager.GetInstance().Players.Single(x => !x.photonPlayer.IsLocal).Player.UpdateSequence(commandEnums);
        networkPlayerSequenceBarView.UpdateSequenceBar(commandEnums);
    }

    [PunRPC]
    public void UpdateReadyState(PhotonMessageInfo info)
    {
        print("GOT RPC Ready state");
        SendStartExecution();
        /*gameManager.Players.Single(x => !x.photonPlayer.IsLocal).Player.IsReady = true;

        if(gameManager.Players.All(x => x.Player.IsReady))
            SendStartExecution();*/
    }

    private void SendStartExecution()
    {
        this.photonView.RPC(nameof(StartExecution), PhotonTargets.All);
    }

    [PunRPC]
    public void StartExecution(PhotonMessageInfo info) {
        EventManager.OnAllPlayersReady();
    }

}

