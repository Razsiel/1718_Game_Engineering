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
            //var listCommands = new ListContainer<CommandEnum> { List = new List<CommandEnum>() };
            var listCommands = new ListContainer<SerializedLoopCommand> { List = new List<SerializedLoopCommand>() };

            var commandOptions = CommandLib.Commands;

            //Add the enum of the command to our list
            //foreach (var bc in sequence) listCommands.List.Add(commandOptions.GetKey(bc));

            print("Serializing commands");
            listCommands.List = GetSerializedCommands(sequence);
                   
            //print(testStringBuild(listCommands.List, 0));

            var seqJson = JsonUtility.ToJson(listCommands);

            this.photonView.RPC(nameof(UpdateOtherPlayersCommands), PhotonTargets.Others, seqJson);
        }

        //private string testStringBuild(List<SerializedCommand> commands, int tabIndex)
        //{
        //    var sb = new StringBuilder();
        //    foreach (var command in commands)
        //    {
        //        if (command is SerializedLoopCommand)
        //        {
        //            sb.AppendLine($"Loop");
        //            tabIndex++;
        //            for (int i = 0; i < tabIndex; i++)
        //            {
        //                sb.Append('\t');
        //            }
        //            sb.Append(testStringBuild((command as SerializedLoopCommand).Commands, tabIndex));
        //            sb.Append('\n');
        //        }
        //        else
        //        {
        //            sb.AppendLine(command.Command.ToString());
        //        }
        //    }
        //    return sb.ToString();
        //}

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

        [PunRPC]
        public void UpdateOtherPlayersCommands(string commandsJson, PhotonMessageInfo info)
        {
            var commands = JsonUtility.FromJson<ListContainer<SerializedLoopCommand>>(commandsJson);
            print($"{nameof(PhotonManager)}: received the commands and loaded them back to a list");
            var baseCommands = GetBaseCommands(commands.List);
            print($"{nameof(PhotonManager)}: received the basecommands list");
            //TestJsonDeserialize(baseCommands);

            _gameInfo.Players.GetNetworkPlayer().Player.UpdateSequence(baseCommands, false);
            print($"{nameof(PhotonManager)}: Updated list of secondary player");
            EventManager.SecondarySequenceChanged(baseCommands);
            print($"{nameof(PhotonManager)}: Updated UI of the sequence");
        }

        private void TestJsonDeserialize(List<BaseCommand> commands)
        {
            foreach (BaseCommand lc in commands)
                if (lc is LoopCommand)
                    print($"Loop with loopcount: {((LoopCommand)lc).LoopCount} CommandsCount: {((LoopCommand)lc).Sequence.Count}");
        }

        private List<BaseCommand> GetBaseCommands(List<SerializedLoopCommand> commands)
        {
            var baseCommands = new List<BaseCommand>();
            foreach(var command in commands)
            {
                if(command.Command == CommandEnum.LoopCommand)
                {                    
                    var unSerialized = GetBaseCommands(command.Commands);
                    var baseLoop = (LoopCommand)CommandLib.Commands.GetValue(CommandEnum.LoopCommand);
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
            print($"{nameof(PhotonManager)} in updatereadystate");
            //The other player is now (un)ready
            if (!info.sender.Equals(PhotonNetwork.player))
            {
                print($"{nameof(PhotonManager)}: Readystate of Network player: {info.sender} is now {isReady}");
                var networkPlayer = _gameInfo.Players.GetNetworkPlayer().Player;
                networkPlayer.IsReady = isReady;
                networkPlayer.OnReady?.Invoke(isReady);
            }

            print($"{nameof(PhotonManager)} in startexecution: Me: {_gameInfo.Players.GetLocalPlayer().Player.IsReady} Network: {_gameInfo.Players.GetNetworkPlayer().Player.IsReady}");
            print($"{nameof(PhotonManager)} im the masterclient unready player = {_gameInfo.Players.SingleOrDefault(x => !x.Player.IsReady)?.photonPlayer}");
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
            print($"{nameof(PhotonManager)}: StopExecution RPC");
            EventManager.SimulationStop();

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