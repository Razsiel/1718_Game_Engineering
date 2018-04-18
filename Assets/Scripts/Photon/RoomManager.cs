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

namespace Assets.Scripts.Photon
{
    public class RoomManager : global::Photon.MonoBehaviour
    {
        public NetPlayerSeqView NetworkPlayerSequenceBarView;
        
        private RoomListView roomView;
        
       // public GameObject RoomPanel;

        public void Awake()
        {
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

                if (!PhotonNetwork.JoinRandomRoom()) Assert.IsTrue(CreateRoom());

                PhotonManager.Instance.TGEOnPhotonPlayerConnected += OnPhotonPlayerConnected;

                PhotonManager.Instance.TGEOnJoinedRoom += () =>
                {
                    Debug.Log("Joined a room");

                    PhotonManager.Instance.TGEOnPlayersCreated += () =>
                    {
                        print("Players are Created!");
                        localPlayer.Player.SequenceChanged += (List<BaseCommand> sequence) =>
                        {                            
                            print("about to send rpc");
                            var listCommands = new ListContainer<CommandHolder>() { list = new List<CommandHolder>() };
                            var commandOptions = GameManager.GetInstance().CommandLibrary.Commands;
                            
                            foreach (BaseCommand c in sequence)
                                listCommands.list.Add(new CommandHolder(commandOptions.GetKey(c)));
                                                  
                            this.photonView.RPC(nameof(UpdateOtherPlayersCommands), PhotonTargets.Others, JsonUtility.ToJson(listCommands));
                            print("done sending");
                        };


                        localPlayer.Player.OnPlayerReady += () =>
                        {
                            localPlayer.Player.IsReady = true;

                            if (!PhotonNetwork.player.IsMasterClient)
                                this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.MasterClient);
                        };
                    };
                };
            };
        }

        /// <summary>
        /// Called when a networkplayer joins the room (not the local player)
        /// </summary>
        /// <param name="player">The player that joined the room</param>
        private void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            Debug.Log("Player joined WOHOO");
            PhotonManager.Instance.TGEOnLeftRoom += () => { };
            PhotonManager.Instance.TGEOnPhotonPlayerDisconnected += (PhotonPlayer otherPlayer) =>
            {
                //gameManager.Players.RemoveAll(x => x != gameManager.Players.Single(y => y.photonPlayer.IsLocal));
            };

            //We can only continue here if we have two players, multiplayer is no fun alone
            if (PhotonNetwork.room.PlayerCount != PhotonNetwork.room.MaxPlayers)
                return;

            if (PhotonNetwork.player.IsMasterClient)
            {
                PhotonNetwork.room.IsOpen = false;
                this.photonView.RPC(nameof(OnAllPlayersJoined), PhotonTargets.All);
            }

            PhotonManager.Instance.TGEOnJoinRoomFailed += (object[] codeAndMsg) => { Assert.IsTrue(PhotonNetwork.CreateRoom(null)); };
            Debug.Log($"Connected to photon: {PhotonNetwork.room}");
        }

        private bool CreateRoom()
        {
            print("Creating a new room all for myself!");
            var roomOptions = new RoomOptions
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 2
            };
            return PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default);
        }

        private void OnJoinRandomRoomFailed(object[] codeAndMsg)
        {
            print($"Join random room Failed, Code: {codeAndMsg[0]} Message: {codeAndMsg[1]}");
            Assert.IsTrue(CreateRoom());
        }

        /// <summary>
        /// All players joined our room (we are now full and closed)
        /// </summary>
        [PunRPC]
        private void OnAllPlayersJoined()
        {
            PhotonManager.Instance.OnAllPlayersJoined(PhotonNetwork.room);
        }

        /*
        //OBSOLETE: Was used to start the game
        private bool alreadyStarted = false;
        void FixedUpdate()
        {
            if(!alreadyStarted && PhotonNetwork.playerList.Count() > 1 && GameManager.GetInstance().IsMultiPlayer)
            {

                GameManager.GetInstance().Players.Add(new TGEPlayer());
                GameManager.GetInstance().Players[1].photonPlayer = PhotonNetwork.playerList.Single(x => !x.IsLocal);

                GameManager.GetInstance().StartMultiplayerGame(GameManager.GetInstance().Players);
                alreadyStarted = true;
            }

        }*/

        public void UpdateGUI()
        {
            Assert.IsNotNull(roomView);            
            roomView.UpdateListView(PhotonNetwork.GetRoomList().ToList());
        }

        public void AddRoom(RoomInfo room)
        {
            //this.photonRooms.Add(room);
        }

        public void CloseRoom(RoomInfo room)
        {
            //this.photonRooms.Remove(room);
        }

        /// <summary>
        /// Update the network player's command sequence
        /// </summary>
        [PunRPC]
        public void UpdateOtherPlayersCommands(string commandsJson, PhotonMessageInfo info)
        {
            print("Got RPC");
            var commands = JsonUtility.FromJson<ListContainer<CommandHolder>>(commandsJson);
            var commandEnums = commands.list.Select(x => x.command).ToList();
            GameManager.GetInstance().Players.Single(x => !x.photonPlayer.IsLocal).Player.UpdateSequence(commandEnums);
            NetworkPlayerSequenceBarView.UpdateSequenceBar(commandEnums);
        }

        /// <summary>
        /// Update the ready state of me to the master (could be me)
        /// </summary>
        /// <param name="info"></param>
        [PunRPC]
        public void UpdateReadyState(PhotonMessageInfo info)
        {
            print("GOT RPC Ready state");

            //FOR NOW EXECTION STARTS WHEN ONE PLAYER READY's UP
            SendStartExecution();

            /*gameManager.Players.Single(x => !x.photonPlayer.IsLocal).Player.IsReady = true;

            if(gameManager.Players.All(x => x.Player.IsReady))
                SendStartExecution();*/
        }

        /// <summary>
        /// Send a RPC to start the execution on all players in the room
        /// </summary>
        private void SendStartExecution()
        {
            this.photonView.RPC(nameof(StartExecution), PhotonTargets.All);
        }

        /// <summary>
        /// A RPC that will make the local players execute their sequence
        /// </summary>
        [PunRPC]
        public void StartExecution(PhotonMessageInfo info)
        {
            EventManager.OnAllPlayersReady();
        }

    }
}

