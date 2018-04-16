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

    public NetPlayerSeqView networkPlayerSequenceBarView;
    private List<RoomInfo> photonRooms;
    private RoomListView roomView;
    public GameObject roomPanel;
    public GameManager gameManager;

    public void Awake()
    {
        photonRooms = new List<RoomInfo>();
        //roomView = roomPanel.GetComponents<RoomListView>()[0];
        //print(roomView);
        //Assert.IsNotNull(roomView);
    }

    //Lets connect two users to Photon and a lobby (+room)
    void Start()
    {
        gameManager = GameManager.GetInstance();
        if(!gameManager.IsMultiPlayer)
            return;

        PhotonNetwork.autoJoinLobby = true;
        Assert.IsTrue(PhotonNetwork.ConnectUsingSettings("1.0"));

        PrintIfMultiplayer(PhotonNetwork.connectionState);
        PhotonManager.Instance.TGEOnJoinRandomRoomFailed += (object[] codeAndMsg) => { PrintCodeAndMessage(codeAndMsg); Assert.IsTrue(PhotonNetwork.CreateRoom("RoomLocal")); };
        PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { PrintCodeAndMessage(codeAndMsg); };

        PhotonManager.Instance.TGEOnJoinedLobby += () =>
        {
            GameManager.GetInstance().Players[0].photonPlayer = PhotonNetwork.player;

            Array.ForEach(PhotonNetwork.GetRoomList(), x => photonRooms.Add(x));

            //Gone for now
            //UpdateGUI();

            Debug.Log("We joined the lobby!");

            if(!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(PhotonNetwork.CreateRoom(Guid.NewGuid().ToString()));

            PhotonManager.Instance.TGEOnPhotonPlayerConnected += (PhotonPlayer player) =>
            {
                PhotonManager.Instance.TGEOnLeftRoom += () =>
                { };

                PhotonManager.Instance.TGEOnPhotonPlayerDisconnected += (PhotonPlayer otherPlayer) =>
                {
                    gameManager.Players.RemoveAll(x => x != gameManager.Players.Single(y => y.photonPlayer.IsLocal));
                };

                //We can only continue here if we have two players, multiplayer is no fun alone
                if(PhotonNetwork.playerList.Length < 2 || PhotonNetwork.playerList.Length > 2) return;
                Debug.Log("Player joined WOHOO");

                if(PhotonNetwork.player.IsMasterClient)
                    PhotonNetwork.room.IsOpen = false;

                PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) =>
                {
                    Assert.IsTrue(PhotonNetwork.CreateRoom(null));
                };
                PrintIfMultiplayer("Connected to photon: " + PhotonNetwork.room + PhotonNetwork.room.PlayerCount);
            };

            PhotonManager.Instance.TGEOnJoinedRoom += () =>
            {
                PhotonManager.Instance.TGEOnPlayersCreated += () =>
                {
                    PrintIfMultiplayer("Players are Created!");
                    gameManager.Players.GetLocalPlayer().player.SequenceChanged += (List<BaseCommand> sequence) =>
                    {
                        //var listCommands = new ListContainer<CommandHolder>() { list = new List<CommandHolder>() };
                        var listCommands = new ListContainer<CommandEnum>() { list = new List<CommandEnum>() };
                        var commandOptions = gameManager.CommandLibrary.Commands;

                        foreach(BaseCommand c in sequence)
                            //listCommands.list.Add(new CommandHolder(commandOptions.GetKey(c)));
                            listCommands.list.Add(commandOptions.GetKey(c));

                        string seqJson = JsonUtility.ToJson(listCommands);

                        this.photonView.RPC(nameof(UpdateOtherPlayersCommands), PhotonTargets.Others, seqJson);
                        PrintIfMultiplayer("done sending");
                    };


                    gameManager.Players.GetLocalPlayer().player.OnPlayerReady += () =>
                    {
                        gameManager.Players.GetLocalPlayer().player.IsReady = true;

                        this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.MasterClient);
                    };
                };

            };
        };
    }

    private bool alreadyStarted = false;
    void FixedUpdate()
    {
        if(!alreadyStarted && PhotonNetwork.playerList.Count() > 1 && gameManager.IsMultiPlayer)
        {
            gameManager.Players.Add(new TGEPlayer());
            gameManager.Players[1].photonPlayer = PhotonNetwork.playerList.Single(x => !x.IsLocal);

            gameManager.StartMultiplayerGame(gameManager.Players);
            alreadyStarted = true;
        }

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
        PrintIfMultiplayer("Got RPC");
        //ListContainer<CommandHolder> commands = JsonUtility.FromJson<ListContainer<CommandHolder>>(commandsJson);
        var commands = JsonUtility.FromJson<ListContainer<CommandEnum>>(commandsJson);
        //List<CommandEnum> commandEnums = commands.list.Select(x => x.command).ToList();
        var commandEnums = commands.list;
        gameManager.Players.GetNetworkPlayer().player.UpdateSequence(commandEnums);
        networkPlayerSequenceBarView.UpdateSequenceBar(commandEnums);
    }

    [PunRPC]
    public void UpdateReadyState(PhotonMessageInfo info)
    {
        PrintIfMultiplayer("GOT RPC Ready state");
        if(info.sender != gameManager.Players.GetLocalPlayer().photonPlayer)
            gameManager.Players.GetNetworkPlayer().player.IsReady = true;

        if(gameManager.Players.GetLocalPlayer().photonPlayer.IsMasterClient)
            if(gameManager.Players.All(x => x.player.IsReady))
                SendStartExecution();
    }

    private void SendStartExecution()
    {
        this.photonView.RPC(nameof(StartExecution), PhotonTargets.All);
    }

    [PunRPC]
    public void StartExecution(PhotonMessageInfo info)
    {
        foreach(TGEPlayer p in gameManager.Players)
            p.player.StartExecution();
    }

    private void PrintIfMultiplayer(object message)
    {
        if(gameManager.IsMultiPlayer)
            print(message);
    }

    private void PrintCodeAndMessage(object[] codeAndMsg)
    {
        print("Code: " + codeAndMsg[0] + "message: " + codeAndMsg[1]);
    }
}

