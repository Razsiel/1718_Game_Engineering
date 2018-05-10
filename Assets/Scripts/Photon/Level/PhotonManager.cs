using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Reflection;
using Assets.Data.Levels;
using UnityEngine.Assertions;
using Photon;
using Assets.Scripts.DataStructures;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts.Lib.Helpers;

namespace Assets.Scripts.Photon.Level
{
    public class PhotonManager : PunBehaviour
    {
        //Backing field of our singleton instance
        private static PhotonManager _instance;
        //private RoomManager _roomManager;

        private GameInfo _gameInfo;

        public CommandLibrary CommandLib;

        ////Events to react on
        //public UnityAction TGEOnJoinedLobby;
        //public UnityAction<PhotonPlayer> TGEOnPhotonPlayerConnected;
        //public UnityAction<object[]> TGEOnJoinRandomRoomFailed;
        //public UnityAction<object[]> TGEOnJoinRoomFailed;
        //public UnityAction TGEOnCreatedRoom;
        //public UnityAction TGEOnJoinedRoom;
        //public UnityAction TGEOnLeftRoom;
        //public UnityAction<PhotonPlayer> TGEOnPhotonPlayerDisconnected;
        //public UnityAction TGEOnPlayersCreated;
        public UnityAction<Room> TGEOnAllPlayersJoined;

        //Our singleton instance of the Photonmanager
        public static PhotonManager Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        //public RoomManager RoomManager
        //{
        //    get { return _roomManager; }
        //    set { _roomManager = value; }
        //}

        //Private because of SingleTon
        private PhotonManager() { }

        void Awake()
        {
            
            print($"{nameof(PhotonManager)}: in awake");
            Instance = this;

            GlobalData.SceneDataLoader.OnSceneLoaded += gameInfo =>
            {
                this.photonView.viewID = (int)PhotonViewIndices.InLevel;
                
                TGEOnAllPlayersJoined?.Invoke(PhotonNetwork.room);

            };

            EventManager.OnGameStart += gameInfo =>
            {
                if (!gameInfo.IsMultiplayer) return;
                _gameInfo = gameInfo;
                
                print($"{nameof(PhotonManager)}: GameStarted");

                EventManager.OnPlayerSpawned += player =>
                {
                    EventManager.OnSequenceChanged += OnSequenceChanged;

                    EventManager.OnPlayerReady += isReady =>
                    {
                        _gameInfo.Players.GetLocalPlayer().Player.IsReady = true;
                        this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.MasterClient);
                    };
                };
                
            };
        }

        private void OnSequenceChanged(List<BaseCommand> sequence)
        {
            var listCommands = new ListContainer<CommandEnum> { List = new List<CommandEnum>() };
            var commandOptions = CommandLib.Commands;

            //Add the enum of the command to our list
            foreach (var bc in sequence) listCommands.List.Add(commandOptions.GetKey(bc));

            var seqJson = JsonUtility.ToJson(listCommands);

            this.photonView.RPC(nameof(UpdateOtherPlayersCommands), PhotonTargets.Others, seqJson);
        }

        [PunRPC]
        public void UpdateOtherPlayersCommands(string commandsJson, PhotonMessageInfo info)
        {
            var commands = JsonUtility.FromJson<ListContainer<CommandEnum>>(commandsJson);

            //networkPlayerSequenceBarView.UpdateSequenceBar(commands.list);
        }

        [PunRPC]
        public void UpdateReadyState(PhotonMessageInfo info)
        {
            //The other player is now ready

            if (!info.sender.Equals(_gameInfo.Players.GetLocalPlayer().photonPlayer))
                _gameInfo.Players.GetNetworkPlayer().Player.IsReady = true;

            if (_gameInfo.Players.GetLocalPlayer().photonPlayer.IsMasterClient)
                if (_gameInfo.Players.All(x => x.Player.IsReady))
                    SendStartExecution();
        }

        [PunRPC]
        public void UpdateUnreadyState(PhotonMessageInfo info)
        {
            _gameInfo.Players.GetNetworkPlayer().Player.IsReady = false;
        }

        private void SendStartExecution()
        {
            this.photonView.RPC(nameof(StartExecution), PhotonTargets.All);
        }

        private void SendStopExecution()
        {
            this.photonView.RPC(nameof(StopExecution), PhotonTargets.All);
        }

        //        private int amountOfSequenceRan = 0;

        [PunRPC]
        public void StartExecution(PhotonMessageInfo info)
        {
            //EventManager.OnExecutionStarted?.Invoke();

            //foreach(TGEPlayer p in gameManager.Players)
            //{
            //    p.Player.StartExecution();
            //    //p.Player.OnPlayerSequenceRan += (Player player) => PlayerSequenceRan(player);
            //}
        }

        //        private void PlayerSequenceRan(Player p)
        //        {
        //            amountOfSequenceRan++;
        //            if(amountOfSequenceRan > 1)
        //            {
        //                //if(!gameManager.LevelData.HasReachedAllGoals())
        //                //{
        //                //    //EventManager.OnLevelReset(gameManager.LevelData,
        //                //    //  gameManager.Players.Select(x => x.Player).ToList());
        //                //}
        //                //else
        //                //{
        //                //    EventManager.AllLevelGoalsReached();
        //                //}
        //                amountOfSequenceRan = 0;
        //            }

        //            //p.OnPlayerSequenceRan -= PlayerSequenceRan;
        //        }

        [PunRPC]
        public void StopExecution(PhotonMessageInfo info)
        {
            //foreach(TGEPlayer p in gameManager.Players)
            //{
            //    p.Player.StopAllCoroutines();
            //    //EventManager.LevelReset(gameManager.LevelData,
            //    //                gameManager.Players.Select(x => x.Player).ToList());
            //}
        }

        //public void PlayersReady()
        //{
        //    TGEOnPlayersCreated?.Invoke();
        //}      
    }
}