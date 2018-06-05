﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Assets.Data.Command;
using Assets.Data.Goal;
using Assets.Data.Grids;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using Assets.Scripts.DataStructures.Channel;
using Assets.Scripts.Grid.DataStructure;
using Assets.Scripts.Lib.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Data.Levels
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Data/Level/NewLevel")]
    public class LevelData : ScriptableObject {
        [SerializeField] public string Name;
        [SerializeField] public Texture2D BackgroundImage;
        [SerializeField] public int MaxCommands;
        [SerializeField] public List<LevelGoalData> Goals;
        [SerializeField] public GridMapData GridMapData;
        [SerializeField] public int TileScale = 32;
        [SerializeField] public Monologue Monologue;
        [SerializeField] public List<AllowedCommand> AllowedCommands;
        [SerializeField] public LevelScore LevelScore;
        [SerializeField] public Sprite PreviewImage;

        [Serializable]
        public class AllowedCommand {
            public CommandEnum CommandType;
            public bool IsAllowed;

            public AllowedCommand(CommandEnum commandTypeValue, bool v) {
                this.CommandType = commandTypeValue;
                this.IsAllowed = v;
            }
        }
        
        private string ScoreKey => $"SCORE_{Name}";
        private Dictionary<Scripts.Player, Vector2Int> _playerPositions;

        public void OnEnable() {
            if (AllowedCommands == null || AllowedCommands.Count < Enum.GetValues(typeof(CommandEnum)).Length) {
                AllowedCommands = new List<AllowedCommand>();
                foreach (CommandEnum commandValue in Enum.GetValues(typeof(CommandEnum))) {
                    AllowedCommands.Add(new AllowedCommand(commandValue, true));
                }
            }

            if(LevelScore == null)
                LevelScore = new LevelScore(0,0);
        }

        public bool HasReachedAllGoals() {
            return Goals.All(goal => goal.HasBeenReached(_playerPositions.Select(p => p.Key)));
        }

        public void Init() {
            _playerPositions = new Dictionary<Scripts.Player, Vector2Int>();
        }

        public PlayerStartPosition InitPlayer(Scripts.Player player) {
            var playerStartPos = GetPlayerStartPosition(player.PlayerNumber);
            _playerPositions.Add(player, playerStartPos.StartPosition);
            return playerStartPos;
        }

        public GridCell GetDestinationTile(Scripts.Player player)
        {
            var directionVector = player.ViewDirection.ToVector2();
            GridCell destination = new GridCell(GridMapData, -1, -1);

            // Get current player pos
            Vector2Int playerPos;
            _playerPositions.TryGetValue(player, out playerPos);
            GridMapData.TryGetCell(playerPos.x + directionVector.x, playerPos.y + directionVector.y, out destination);

            return destination;
        }

        /// <summary>
        /// Checks whether the given player is allowed to move in the direction given on the map
        /// </summary>
        /// <param name="player">The player that wants to move</param>
        /// <param name="direction">The direction the player wants to go from it's current position</param>
        /// <param name="destination">The calculated destination cell containing it's grid position</param>
        /// <returns>Return true if the player can move in the direction. Returns false if there are any obstructions or other players on the destination</returns>
        public bool TryMoveInDirection(Scripts.Player player, CardinalDirection direction, out GridCell destination, SequenceCycle cycle) {

            var directionVector = player.ViewDirection.ToVector2();
            destination = GetDestinationTile(player);

            // IF: MultiPlayer, both MoveCommand and same destination
            if (cycle.Commands.Count > 1 
                && cycle.Commands[0].Item2 == cycle.Commands[1].Item2 
                && GetDestinationTile(cycle.Commands[0].Item1) == GetDestinationTile(cycle.Commands[1].Item1))
            {
                return false;
            }

            // Get current player pos
            Vector2Int playerPos;
            if (!_playerPositions.TryGetValue(player, out playerPos)) {
                Debug.Log(player.GetHashCode());
                Debug.Log($"Could not move player: Player does not have a position on the grid");
                return false; // current player does not have a position in this map
            }

            Debug.Log(
                $"... Trying to move \"{direction.ToString().ToUpper()} {directionVector}\" from {playerPos} to cell ({playerPos.x + directionVector.x}, {playerPos.y + directionVector.y})");

            if (!GridMapData.TryGetCell(playerPos.x + directionVector.x, playerPos.y + directionVector.y,
                                        out destination)) {
                Debug.Log(
                    $"Could not move player: Cell at ({destination.X}, {destination.Y}) does not exist/is out of bounds");
                return false;
            }

            // Check if there are any other players on the destination
            GridCell cell = destination;
            if (_playerPositions.Any(p => p.Key != player && p.Value == cell.XY)) {
                Debug.Log($"Could not move player: A player is standing on the destination");
                return false; // the destination contains a player
            }

            // Get the current player position on the map
            GridCell current;
            if (!GridMapData.TryGetCell(playerPos.x, playerPos.y, out current)) {
                Debug.Log(
                    $"Could not move player: The current player position ({playerPos.x}, {playerPos.y}) does not exist in the map");
                return false; // the current position of the player does not exist on the map
            }

            // Checks the current tile to see whether it can be exited from the direction
            var oppositeDirection = direction.ToOppositeDirection();
            var canLeaveCurrent = current.Value.IsWalkable(direction);
            // Checks the destination tile to see whether it can be entered/walked into from the given direction
            var canEnterDestination = destination.Value.IsWalkable(oppositeDirection);
            
            // Final check whether the player can actually move
            var canMove = canLeaveCurrent && canEnterDestination;

            // Move player in grid
            if (canMove) {
                Debug.Log($"Can move to {destination.XY}");
                _playerPositions[player] = destination.XY;
                player.GridPosition = destination.XY;
                // TODO: implement events here
                // player.OnMove(destination.XY);
                // current.Value.OnLeave(player);
                // destination.Value.OnEnter(player);
            }

            return canMove;
        }

        public PlayerStartPosition GetPlayerStartPosition(int playerNumber) {
            return GridMapData.PlayerStartPositions[playerNumber];
        }

        public bool TryInteract(Scripts.Player player, CardinalDirection direction,
                                out IEnumerable<DecorationConfiguration> decorationsInFrontOfPlayer) {
            decorationsInFrontOfPlayer = null;
            // Get current player pos
            Vector2Int playerPos;
            if (!_playerPositions.TryGetValue(player, out playerPos)) {
                return false; // current player does not have a position in this map
            }

            // Get the current player position on the map
            GridCell current;
            if (!GridMapData.TryGetCell(playerPos.x, playerPos.y, out current)) {
                return false; // the current position of the player does not exist on the map
            }

            Debug.Log($"Trying to interact with any decorations");
            // Get decorations that are oriented opposite to the player (so facing the player)
            decorationsInFrontOfPlayer =
                current.Value.DecorationConfigs.Where(d => d.Orientation == CardinalDirection.None ||
                                                           d.Orientation == direction.ToOppositeDirection());

            if (!decorationsInFrontOfPlayer.Any()) {
                return false; // there are no decorations to interact with
            }

            foreach (var decorationConfigurations in decorationsInFrontOfPlayer.GroupBy(d => d.Type)) {
                // Send messages through the channel system for triggers
                if (decorationConfigurations.Key == ChannelType.Trigger) {
                    Debug.Log($"Found {decorationConfigurations.Count()} triggers...");
                    var triggersByChannels = decorationConfigurations.GroupBy(d => d.Channel);
                    foreach (var triggerByChannel in triggersByChannels) {
                        Debug.Log($"Sending a message to channel: {triggerByChannel.Key}");
                        DecorationChannelManager.Instance.TriggerChannel(triggerByChannel.Key, player);
                    }
                }

                // Call interact on object
                foreach (var decorationConfiguration in decorationConfigurations) {
                    decorationConfiguration.OnInteract(decorationConfiguration.Channel, player);
                }
            }

            return true;
        }

        public void Reset(List<Scripts.Player> players, Action<List<Scripts.Player>, LevelData> animateCallback) {
            Debug.Log($"Resetting level state...");
            Assert.IsNotNull(players);
            Assert.IsTrue(players.Any());
            Assert.IsNotNull(_playerPositions);
            foreach (var player in players) {
                Assert.IsNotNull(player);
                var startPosition = GetPlayerStartPosition(player.PlayerNumber);
                _playerPositions[player] = startPosition.StartPosition;
            }
            foreach (var gridCell in GridMapData) {
                foreach (var decorationConfig in gridCell.Value.DecorationConfigs) {
                    decorationConfig.Reset();
                }
            }
            animateCallback(players, this);
        }

        public void SaveScore(int score) {
            PlayerPrefs.SetInt(ScoreKey, score);
            PlayerPrefs.Save();
        }

        public int GetScore() {
            return PlayerPrefs.HasKey(ScoreKey) ? PlayerPrefs.GetInt(ScoreKey) : 0;
        }
    }
}