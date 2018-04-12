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
        PhotonManager.Instance.TGEOnJoinRandomRoomFailed += (object[] codeAndMsg) => { print("Join random room Failed, Code: " + codeAndMsg[0] + " Message: " + codeAndMsg[1] + ""); Assert.IsTrue(PhotonNetwork.CreateRoom("RoomLocal")); };
        PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { print("Join room failed, Code: " + codeAndMsg[0] + " Message: " + codeAndMsg[1]); };

        PhotonManager.Instance.TGEOnJoinedLobby += () =>
        {
            GameManager.GetInstance().Players[0].photonPlayer = PhotonNetwork.player;

            Array.ForEach(PhotonNetwork.GetRoomList(), x => photonRooms.Add(x));

            //Gone for now
            //UpdateGUI();

            Debug.Log("We joined the lobby!");

            if(!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(PhotonNetwork.CreateRoom("RoomLocal"));

            PhotonManager.Instance.TGEOnPhotonPlayerConnected += (PhotonPlayer player) =>
            {
                PhotonManager.Instance.TGEOnLeftRoom += () =>
                {

                };

                PhotonManager.Instance.TGEOnPhotonPlayerDisconnected += (PhotonPlayer otherPlayer) =>
                {
                    GameManager.GetInstance().Players.RemoveAll(x => x != GameManager.GetInstance().Players.Single(y => y.photonPlayer.IsLocal));
                };

                Debug.Log("Player is here, lets see if somebody else joins");
                //We can only continue here if we have two players, multiplayer is no fun alone
                if(PhotonNetwork.playerList.Length < 2 || PhotonNetwork.playerList.Length > 2) return;
                Debug.Log("Player joined WOHOO");

                GameManager.GetInstance().Players.Add(new TGEPlayer());
                GameManager.GetInstance().Players[1].photonPlayer = player;

                print(GameManager.GetInstance().Players);

                //PhotonNetwork.room.IsOpen = false;
                int index = PhotonNetwork.isMasterClient ? 0 : 1;

                //SpawnReplicated += () => { Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); };


                //GameManager.GetInstance().StartMultiplayerGame(PhotonNetwork.playerList);

                PhotonNetwork.room.IsOpen = false;

                PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) =>
                {
                    Assert.IsTrue(PhotonNetwork.CreateRoom(null));
                };

                PhotonManager.Instance.TGEOnPlayersCreated += () =>
                {
                    print("Players are Created!");
                    GameManager.GetInstance().Players.Single(x => x.photonPlayer.IsLocal).player.SequenceChanged += (List<BaseCommand> sequence) =>
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
                    
                };

                
                Debug.Log("Connected to photon: " + PhotonNetwork.room + PhotonNetwork.room.PlayerCount);
            };
        };
    }

    private bool alreadyStarted = false;
    void FixedUpdate()
    {
        if(!alreadyStarted && PhotonNetwork.playerList.Count() > 1 && GameManager.GetInstance().IsMultiPlayer)
        {
            GameManager.GetInstance().StartMultiplayerGame(GameManager.GetInstance().Players);
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
        print("Got RPC");
        ListContainer<CommandHolder> commands = JsonUtility.FromJson<ListContainer<CommandHolder>>(commandsJson);
        
        networkPlayerSequenceBarView.UpdateSequenceBar(commands.list.Select(x => x.command).ToList());
    }

}

