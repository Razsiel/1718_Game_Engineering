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
    
    private RoomListView RoomView;
    public GameObject RoomPanel;
    private GameManager gameManager;

    void Awake()
    {
        //photonRooms = new List<RoomInfo>();
        //roomView = roomPanel.GetComponents<RoomListView>()[0];
        //print(roomView);
        //Assert.IsNotNull(roomView);
    }

    //Lets connect two users to Photon and a lobby (+room)
    void Start()
    {
        gameManager = GameManager.GetInstance();
        if (!gameManager.IsMultiPlayer)
            return;

        PhotonNetwork.autoJoinLobby = true;
        Assert.IsTrue(PhotonNetwork.ConnectUsingSettings("1.0"));

        PrintIfMultiplayer(PhotonNetwork.connectionState);
        PhotonManager.Instance.TGEOnJoinRandomRoomFailed += (object[] codeAndMsg) => { PrintCodeAndMessage(codeAndMsg); Assert.IsTrue(PhotonNetwork.CreateRoom(Guid.NewGuid().ToString())); };
        PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { PrintCodeAndMessage(codeAndMsg); };

        PhotonManager.Instance.TGEOnJoinedLobby += () =>
        {
            GameManager.GetInstance().Players[0].photonPlayer = PhotonNetwork.player;

            //Array.ForEach(PhotonNetwork.GetRoomList(), x => photonRooms.Add(x));

            //Gone for now
            //UpdateGUI();
           
            if (!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(PhotonNetwork.CreateRoom(Guid.NewGuid().ToString()));

            PhotonManager.Instance.TGEOnPhotonPlayerConnected += (PhotonPlayer player) =>
            {
                PhotonManager.Instance.TGEOnLeftRoom += () =>
                { };

                PhotonManager.Instance.TGEOnPhotonPlayerDisconnected += (PhotonPlayer otherPlayer) =>
                {
                    gameManager.Players.RemoveAll(x => x.photonPlayer == otherPlayer);
                };

                //We can only continue here if we have two players, multiplayer is no fun alone
                if (PhotonNetwork.playerList.Length < 2 || PhotonNetwork.playerList.Length > 2) return;
                    PrintIfMultiplayer("Player joined WOHOO");

                if (PhotonNetwork.player.IsMasterClient)
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
                    gameManager.Players.GetLocalPlayer().Player.SequenceChanged += (List<BaseCommand> sequence) =>
                    {                        
                        var listCommands = new ListContainer<CommandEnum>() { list = new List<CommandEnum>() };
                        var commandOptions = gameManager.CommandLibrary.Commands;

                        foreach (BaseCommand c in sequence)                            
                            listCommands.list.Add(commandOptions.GetKey(c));

                        string seqJson = JsonUtility.ToJson(listCommands);

                        this.photonView.RPC(nameof(UpdateOtherPlayersCommands), PhotonTargets.Others, seqJson);
                        PrintIfMultiplayer("done sending");
                    };

                    gameManager.Players.GetLocalPlayer().Player.OnPlayerReady += () =>
                    {
                        gameManager.Players.GetLocalPlayer().Player.IsReady = true;

                        this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.MasterClient);
                    };

                    gameManager.Players.GetLocalPlayer().Player.OnPlayerStop += () =>
                    {
                        SendStopExecution();
                    };

                    gameManager.Players.GetLocalPlayer().Player.OnPlayerUnready += () =>
                    {
                        gameManager.Players.GetLocalPlayer().Player.IsReady = false;

                        if(!gameManager.Players.GetLocalPlayer().photonPlayer.IsMasterClient)
                            this.photonView.RPC(nameof(UpdateUnreadyState), PhotonTargets.MasterClient);
                    };
                };

            };
        };
    }

    private bool alreadyStarted = false;
    void FixedUpdate()
    {
        if (!alreadyStarted && PhotonNetwork.playerList.Count() > 1 && gameManager.IsMultiPlayer)
        {
            alreadyStarted = true;
            gameManager.Players.Add(new TGEPlayer());
            gameManager.Players[1].photonPlayer = PhotonNetwork.playerList.Single(x => !x.IsLocal);

            gameManager.StartMultiplayerGame(gameManager.Players);           
        }

    }

    public void UpdateGUI()
    {
        Assert.IsNotNull(RoomView);
        RoomView.ToString();
        RoomView.UpdateListView(PhotonNetwork.GetRoomList().ToList());
    }

    public void AddRoom(RoomInfo room)
    {
        //this.photonRooms.Add(room);
    }

    public void CloseRoom(RoomInfo room)
    {
        //this.photonRooms.Remove(room);
    }

    [PunRPC]
    public void UpdateOtherPlayersCommands(string commandsJson, PhotonMessageInfo info)
    {
        PrintIfMultiplayer("Got RPC");
        var commands = JsonUtility.FromJson<ListContainer<CommandEnum>>(commandsJson);
        gameManager.Players.GetNetworkPlayer().Player.UpdateSequence(commands.list);
        networkPlayerSequenceBarView.UpdateSequenceBar(commands.list);
    }

    [PunRPC]
    public void UpdateReadyState(PhotonMessageInfo info)
    {
        PrintIfMultiplayer("GOT RPC Ready state");
        //The other player is now ready

        if (info.sender != gameManager.Players.GetLocalPlayer().photonPlayer)
            gameManager.Players.GetNetworkPlayer().Player.IsReady = true;

        if (gameManager.Players.GetLocalPlayer().photonPlayer.IsMasterClient)
            if (gameManager.Players.All(x => x.Player.IsReady))
                SendStartExecution();
    }

    [PunRPC]
    public void UpdateUnreadyState(PhotonMessageInfo info)
    {
        gameManager.Players.GetNetworkPlayer().Player.IsReady = false;
    }

    private void SendStartExecution()
    {
        this.photonView.RPC(nameof(StartExecution), PhotonTargets.All);
    }

    private void SendStopExecution()
    {
        this.photonView.RPC(nameof(StopExecution), PhotonTargets.All);
    }

    [PunRPC]
    public void StartExecution(PhotonMessageInfo info)
    {
        EventManager.ExecutionStarted?.Invoke();
        int playersSequenceRan = 0;

        foreach (TGEPlayer p in gameManager.Players)
        {
            p.Player.StartExecution();
            p.Player.OnPlayerSequenceRan += () => 
            {
                playersSequenceRan++;
                if (playersSequenceRan > 1)
                    if (!gameManager.LevelData.HasReachedAllGoals())
                    {
                        EventManager.OnLevelReset(gameManager.LevelData,
                            gameManager.Players.Select(x => x.Player).ToList());
                       
                            foreach (var player in gameManager.Players)
                            {
                                player.Player.IsReady = false;
                            }                       
                    }
                    else
                    {
                        EventManager.OnAllLevelGoalsReached();
                    }

            };
        }
    }

    [PunRPC]
    public void StopExecution(PhotonMessageInfo info)
    {
        foreach (TGEPlayer p in gameManager.Players)
        {
            p.Player.StopExecution();
        }
    }

    private void PrintIfMultiplayer(object message)
    {
        if (gameManager.IsMultiPlayer)
            print(message);
    }

    private void PrintCodeAndMessage(object[] codeAndMsg)
    {
        print("Code: " + codeAndMsg[0] + "message: " + codeAndMsg[1]);
    }
}

