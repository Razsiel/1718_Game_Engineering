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
using System.Text;
using Utilities;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Photon.Level
{
    public class PhotonManager : PunBehaviour
    {
        //Backing field of our singleton instance
        private static PhotonManager _instance;

        private GameInfo _gameInfo;
        public CommandLibrary CommandLib;
        public SceneField MainMenu;

        public UnityAction<Room> TGEOnAllPlayersJoined;
        public UnityAction TGEOnOtherPlayerLeft;

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

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom(false);
        }

        #region EventImplementations
        private void OnInitializePhoton()
        {
            EventManager.OnInitializePhoton -= OnInitializePhoton;
            this.photonView.viewID = (int)PhotonViewIndices.InLevel;
            TGEOnAllPlayersJoined?.Invoke(PhotonNetwork.room);
            TGEOnOtherPlayerLeft += OnOtherPlayerLeft;
        }

        private void OnOtherPlayerLeft()
        {
            LeaveRoom();
            SceneManager.LoadScene(MainMenu);
        }
       
        public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            TGEOnOtherPlayerLeft?.Invoke();
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
            EventManager.OnStopButtonClicked += OnStopButtonClicked;
        }

        private void OnStopButtonClicked()
        {
            print($"{nameof(PhotonManager)}: OnStopButtonClicked");
            SendStopExecution();
        }

        private void OnPlayerReady(Player player, bool isReady)
        {
            player.IsReady = isReady;
            this.photonView.RPC(nameof(UpdateReadyState), PhotonTargets.All, isReady);
        }

        private void OnSequenceChanged(List<BaseCommand> sequence)
        {
            var listCommands = new ListContainer<SerializedLoopCommand> { List = new List<SerializedLoopCommand>() };                      
            listCommands.List = GetSerializedCommands(sequence);
                             
            var seqJson = JsonUtility.ToJson(listCommands);
            this.photonView.RPC(nameof(UpdateOtherPlayersCommands), PhotonTargets.Others, seqJson);
        }
    
        private List<SerializedLoopCommand> GetSerializedCommands(List<BaseCommand> commands)
        {
            var serialized = new List<SerializedLoopCommand>();
            foreach (var bc in commands)
            {
                if (bc is LoopCommand)
                {
                    var lc = (LoopCommand)bc;
                    var serializeList = GetSerializedCommands(lc.Sequence.Commands);
                   
                    serialized.Add(new SerializedLoopCommand(lc.LoopCount, serializeList) { Command = CommandEnum.LoopCommand });
                }
                else
                {
                    serialized.Add(new SerializedLoopCommand() { Command = CommandLib.Commands.GetKey(bc) });
                }
            }            
            return serialized;
        }
        #endregion

        public void GoToScene(SceneField scene)
        {
            Assert.IsNotNull(scene);

            print($"{nameof(PhotonManager)}: Im the masterclient?: {PhotonNetwork.player.IsMasterClient} with scene: {scene.SceneName}");
            if (PhotonNetwork.player.IsMasterClient)
                PhotonNetwork.LoadLevel(scene);
            else
                this.photonView.RPC(nameof(MasterClientShouldLoadScene), PhotonTargets.MasterClient, scene.SceneName);
        }

        [PunRPC]
        public void MasterClientShouldLoadScene(string sceneName, PhotonMessageInfo info)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }

        [PunRPC]
        public void UpdateOtherPlayersCommands(string commandsJson, PhotonMessageInfo info)
        {
            var commands = JsonUtility.FromJson<ListContainer<SerializedLoopCommand>>(commandsJson);           
            var baseCommands = GetBaseCommands(commands.List);
                     
            _gameInfo.Players.GetNetworkPlayer().Player.UpdateSequence(baseCommands, false);           
            EventManager.SecondarySequenceChanged(baseCommands);           
        }

        private List<BaseCommand> GetBaseCommands(List<SerializedLoopCommand> commands)
        {
            var baseCommands = new List<BaseCommand>();
            foreach(var command in commands)
            {
                if(command.Command == CommandEnum.LoopCommand)
                {                    
                    var unSerialized = GetBaseCommands(command.Commands);
                    var baseLoop = Instantiate(CommandLib.LoopCommand) as LoopCommand;                   
                    baseLoop.LoopCount = command.LoopCount;
                    baseLoop.Sequence = new Sequence();
                    baseLoop.Sequence.Commands = unSerialized;
                    baseCommands.Add(baseLoop);
                }
                else
                {
                    baseCommands.Add(CommandLib.Commands.GetValue(command.Command));
                }
            }
            return baseCommands;
        }

        [PunRPC]
        public void UpdateReadyState(bool isReady, PhotonMessageInfo info)
        {            
            //The other player is now (un)ready
            if (!info.sender.Equals(PhotonNetwork.player))
            {
                print($"{nameof(PhotonManager)}: Readystate of Network player: {info.sender} is now {isReady}");
                var networkPlayer = _gameInfo.Players.GetNetworkPlayer().Player;
                networkPlayer.IsReady = isReady;
                networkPlayer.OnReady?.Invoke(isReady);

                if (!isReady)
                {
                    this.photonView.RPC(nameof(UpdateUnreadyState), PhotonTargets.Others);
                }
            }

            // This print code throws linq-exceptions...
//            print($"{nameof(PhotonManager)} in startexecution: Me: {_gameInfo.Players.GetLocalPlayer().Player.IsReady} Network: {_gameInfo.Players.GetNetworkPlayer().Player.IsReady}");
//            print($"{nameof(PhotonManager)} im the masterclient unready player = {_gameInfo.Players.SingleOrDefault(x => !x.Player.IsReady)?.photonPlayer}");
            if (_gameInfo.Players.All(x => x.Player.IsReady))
            {
                print($"{nameof(PhotonManager)} everybody is ready");
                if (_gameInfo.LocalPlayer.photonPlayer.IsMasterClient)
                    SendStartExecution();
                else
                    this.photonView.RPC(nameof(MasterClientShouldStart), PhotonTargets.MasterClient);
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
            print($"{nameof(PhotonManager)}: SendStopExecution RPC");
            this.photonView.RPC(nameof(StopExecution), PhotonTargets.All);
        }

        [PunRPC]
        public void StartExecution(PhotonMessageInfo info)
        {
            //Start the execution on both players
            print($"{nameof(PhotonManager)} in startexecution");
            foreach (var player in _gameInfo.Players)
                print($"Player {player.photonPlayer}, IsMasterClient: {player.photonPlayer.IsMasterClient}, IsLocalPlayer: {player.photonPlayer.IsLocal}");
            EventManager.AllPlayersReady();          
        }

        [PunRPC]
        public void StopExecution(PhotonMessageInfo info)
        {
            //Stop the execution
            print($"{nameof(PhotonManager)}: StopExecution RPC");
            EventManager.SimulationStop();          
        }

        public void OnDestroy()
        {
            EventManager.OnSequenceChanged -= OnSequenceChanged;
            EventManager.OnPlayerReady -= OnPlayerReady;
            EventManager.OnStopButtonClicked -= OnStopButtonClicked;
            TGEOnOtherPlayerLeft -= OnOtherPlayerLeft;
            EventManager.OnPlayerSpawned -= OnPlayerSpawned;
            EventManager.OnGameStart -= OnGameStart;
        }
    }
}