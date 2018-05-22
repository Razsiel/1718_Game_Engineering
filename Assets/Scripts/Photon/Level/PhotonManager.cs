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

        private GameInfo _gameInfo;
        public CommandLibrary CommandLib;

        public UnityAction<Room> TGEOnAllPlayersJoined;

        //Our singleton instance of the Photonmanager
        public static PhotonManager Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        //Private because of SingleTon
        private PhotonManager() { }

        void Awake()
        {
            print($"{nameof(PhotonManager)}: in awake");
            Instance = this;

            EventManager.OnInitializePhoton += OnInitializePhoton;

            EventManager.OnGameStart += OnGameStart;
        }

        #region EventImplementations
        private void OnInitializePhoton()
        {
            EventManager.OnInitializePhoton -= OnInitializePhoton;
            this.photonView.viewID = (int)PhotonViewIndices.InLevel;
            TGEOnAllPlayersJoined?.Invoke(PhotonNetwork.room);
        }

        private void OnGameStart(GameInfo gameInfo)
        {
            EventManager.OnGameStart -= OnGameStart;
            _gameInfo = gameInfo;
            EventManager.OnPlayerSpawned += OnPlayerSpawned;
        }

        private void OnPlayerSpawned(Player player)
        {
            EventManager.OnPlayerSpawned -= OnPlayerSpawned;
            EventManager.OnSequenceChanged += OnSequenceChanged;
            EventManager.OnPlayerReady += OnPlayerReady;
        }

        private void OnPlayerReady(bool isReady)
        {
            _gameInfo.Players.GetLocalPlayer().Player.IsReady = isReady;
            this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.All, isReady);
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
        #endregion

        [PunRPC]
        public void UpdateOtherPlayersCommands(string commandsJson, PhotonMessageInfo info)
        {
            var commands = JsonUtility.FromJson<ListContainer<CommandEnum>>(commandsJson);
            var commandOptions = CommandLib.Commands;
            var baseCommands = new List<BaseCommand>();

            foreach (var ce in commands.List) baseCommands.Add(commandOptions.GetValue(ce));

            _gameInfo.Players.GetNetworkPlayer().Player.UpdateSequence(baseCommands, false);
            EventManager.SecondarySequenceChanged(baseCommands);
        }

        [PunRPC]
        public void UpdateReadyState(bool isReady, PhotonMessageInfo info)
        {
            print($"{nameof(PhotonManager)} in updatereadystate");
            //The other player is now (un)ready
            if (!info.sender.Equals(PhotonNetwork.player))
            {
                print($"{nameof(PhotonManager)}: Readystate of Network player: {info.sender} is now {isReady}");
                _gameInfo.Players.GetNetworkPlayer().Player.IsReady = isReady;
            }
            print($"{nameof(PhotonManager)} in startexecution: Me: {_gameInfo.Players.GetLocalPlayer().Player.IsReady} Network: {_gameInfo.Players.GetNetworkPlayer().Player.IsReady}");
            print($"{nameof(PhotonManager)} im the masterclient unready player = {_gameInfo.Players.SingleOrDefault(x => !x.Player.IsReady)?.photonPlayer}");
            if (_gameInfo.Players.All(x => x.Player.IsReady))
            {
                print($"{nameof(PhotonManager)} everybody is ready");
                if (_gameInfo.LocalPlayer.photonPlayer.IsMasterClient) SendStartExecution();
                else this.photonView.RPC(nameof(MasterClientShouldStart), PhotonTargets.MasterClient);
            }

        }

        [PunRPC]
        public void MasterClientShouldStart(PhotonMessageInfo info)
        {
            SendStartExecution();
        }

        [PunRPC]
        public void UpdateUnreadyState(PhotonMessageInfo info)
        {
            _gameInfo.Players.GetNetworkPlayer().Player.IsReady = false;
        }

        private void SendStartExecution()
        {
            print($"{nameof(PhotonManager)} in sendstartexecution");
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
            //Start the execution on both players
            print($"{nameof(PhotonManager)} in startexecution");
            foreach (var player in _gameInfo.Players)
                print($"Player {player.photonPlayer}, IsMasterClient: {player.photonPlayer.IsMasterClient}, IsLocalPlayer: {player.photonPlayer.IsLocal}");
            EventManager.AllPlayersReady();

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
            //Stop the execution


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