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
using Assets.Scripts.DataStructures.Command;
using Assets.Scripts.Lib.Helpers;
using Assets.Scripts.Photon.Level;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomManager : global::Photon.MonoBehaviour
    {
        public GameObject playerPrefab;

        public NetPlayerSeqView networkPlayerSequenceBarView;

        private RoomListView RoomView;
        public GameObject RoomPanel;
        private GameInfo _gameInfo;

        void Awake()
        {
            //photonRooms = new List<RoomInfo>();
            //roomView = roomPanel.GetComponents<RoomListView>()[0];
            //print(roomView);
            //Assert.IsNotNull(roomView);
            EventManager.OnGameStart += (gameInfo) =>
            {
                _gameInfo = gameInfo;
            };
        }

        //Lets connect two users to Photon and a lobby (+room)
        void Start()
        {
            if(!_gameInfo.IsMultiplayer)
                return;

            PhotonNetwork.autoJoinLobby = true;
            Assert.IsTrue(PhotonNetwork.ConnectUsingSettings("1.0"));

            PrintIfMultiplayer(PhotonNetwork.connectionState);
            PhotonManager.Instance.TGEOnJoinRandomRoomFailed += (object[] codeAndMsg) => { PrintCodeAndMessage(codeAndMsg); Assert.IsTrue(PhotonNetwork.CreateRoom(Guid.NewGuid().ToString())); };
            PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { PrintCodeAndMessage(codeAndMsg); };

            PhotonManager.Instance.TGEOnJoinedLobby += () =>
            {
                _gameInfo.LocalPlayer.photonPlayer = PhotonNetwork.player;

            //Array.ForEach(PhotonNetwork.GetRoomList(), x => photonRooms.Add(x));

            //Gone for now
            //UpdateGUI();

            if(!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(PhotonNetwork.CreateRoom(Guid.NewGuid().ToString()));

                PhotonManager.Instance.TGEOnPhotonPlayerConnected += (PhotonPlayer player) =>
                {
                    PhotonManager.Instance.TGEOnLeftRoom += () =>
                    { };

                    PhotonManager.Instance.TGEOnPhotonPlayerDisconnected += (PhotonPlayer otherPlayer) =>
                    {
                        _gameInfo.Players.RemoveAll(x => x.photonPlayer == otherPlayer);
                    };

                //We can only continue here if we have two players, multiplayer is no fun alone
                if(PhotonNetwork.playerList.Length < 2 || PhotonNetwork.playerList.Length > 2) return;
                    PrintIfMultiplayer("Player joined WOHOO");

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
                        _gameInfo.LocalPlayer.Player.Sequence.OnSequenceChanged += (List<BaseCommand> sequence) =>
                        {
                            var listCommands = new ListContainer<CommandEnum>() { list = new List<CommandEnum>() };
                            var commandOptions = new List<CommandKVP>();//_gameInfo.CommandLibrary.Commands;

                        foreach(BaseCommand c in sequence)
                                listCommands.list.Add(commandOptions.GetKey(c));

                            string seqJson = JsonUtility.ToJson(listCommands);

                            this.photonView.RPC(nameof(UpdateOtherPlayersCommands), PhotonTargets.Others, seqJson);
                            PrintIfMultiplayer("done sending");
                        };

                    /*
                    _gameInfo.LocalPlayer.Player.OnPlayerReady += () =>
                    {
                        gameManager.Players.GetLocalPlayer().Player.IsReady = true;

                        this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.MasterClient);
                    };

                    _gameInfo.LocalPlayer.Player.OnPlayerStop += () =>
                    {
                        SendStopExecution();
                    };

                    _gameInfo.LocalPlayer.Player.OnPlayerUnready += () =>
                    {
                        gameManager.Players.GetLocalPlayer().Player.IsReady = false;

                        if(!gameManager.Players.GetLocalPlayer().photonPlayer.IsMasterClient)
                            this.photonView.RPC(nameof(UpdateUnreadyState), PhotonTargets.MasterClient);
                    };*/
                    };

                };
            };
        }

        private bool alreadyStarted = false;
        void FixedUpdate()
        {
            if(!alreadyStarted && PhotonNetwork.playerList.Count() > 1 && _gameInfo.IsMultiplayer)
            {
                alreadyStarted = true;
                //gameManager.Players.Add(new TGEPlayer());
                //gameManager.Players[1].photonPlayer = PhotonNetwork.playerList.Single(x => !x.IsLocal);

                //gameManager.StartMultiplayerGame(gameManager.Players);           
            }

        }

        public void UpdateGUI()
        {
            Assert.IsNotNull(RoomView);
            RoomView.ToString();
            RoomView.UpdateListView(PhotonNetwork.GetRoomList());
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
            _gameInfo.Players.GetNetworkPlayer().Player.UpdateSequence(commands.list);
            networkPlayerSequenceBarView.UpdateSequenceBar(commands.list);
        }

        [PunRPC]
        public void UpdateReadyState(PhotonMessageInfo info)
        {
            PrintIfMultiplayer("GOT RPC Ready state");
            //The other player is now ready

            if(info.sender != _gameInfo.LocalPlayer.photonPlayer)
                _gameInfo.Players.GetNetworkPlayer().Player.IsReady = true;

            if(_gameInfo.LocalPlayer.photonPlayer.IsMasterClient)
                if(_gameInfo.Players.All(x => x.Player.IsReady))
                    SendStartExecution();
        }

        [PunRPC]
        public void UpdateUnreadyState(PhotonMessageInfo info)
        {
            _gameInfo.LocalPlayer.Player.IsReady = false;
        }

        private void SendStartExecution()
        {
            this.photonView.RPC(nameof(StartExecution), PhotonTargets.All);
        }

        private void SendStopExecution()
        {
            this.photonView.RPC(nameof(StopExecution), PhotonTargets.All);
        }

        private int amountOfSequenceRan = 0;

        [PunRPC]
        public void StartExecution(PhotonMessageInfo info)
        {
            EventManager.OnExecutionStarted?.Invoke();

            foreach(TGEPlayer p in _gameInfo.Players)
            {
                p.Player.StartExecution();
                //p.Player.OnPlayerSequenceRan += (Player player) => PlayerSequenceRan(player);
            }
        }

        private void PlayerSequenceRan(Player p)
        {
            amountOfSequenceRan++;
            if(amountOfSequenceRan > 1)
            {
                if(!_gameInfo.Level.HasReachedAllGoals())
                {
                    //EventManager.OnLevelReset(gameManager.LevelData,
                    //  gameManager.Players.Select(x => x.Player).ToList());
                }
                else
                {
                    EventManager.AllLevelGoalsReached();
                }
                amountOfSequenceRan = 0;
            }

            //p.OnPlayerSequenceRan -= PlayerSequenceRan;
        }

        [PunRPC]
        public void StopExecution(PhotonMessageInfo info)
        {
            foreach(TGEPlayer p in _gameInfo.Players)
            {
                p.Player.StopAllCoroutines();
                /*EventManager.LevelReset(gameManager.LevelData,
                                gameManager.Players.Select(x => x.Player).ToList());*/
            }
        }

        private void PrintIfMultiplayer(object message)
        {
            if(_gameInfo.IsMultiplayer)
                print(message);
        }

        private void PrintCodeAndMessage(object[] codeAndMsg)
        {
            print("Code: " + codeAndMsg[0] + "message: " + codeAndMsg[1]);
        }
    }
}
