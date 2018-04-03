using System.Collections.Generic;
using System.Linq;
using Assets.ScriptableObjects.Grids;
using Assets.ScriptableObjects.Player;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Grid.DataStructure;
using UnityEngine;

namespace Assets.ScriptableObjects.Levels {
    [CreateAssetMenu(fileName = "Level_0", menuName = "Data/Level")]
    [System.Serializable]
    public class LevelData : ScriptableObject {
        [SerializeField] public string Name;
        [SerializeField] public Texture2D BackgroundImage;
        [SerializeField] public List<string> Goals; // convert to data type later containing additional variables
        [SerializeField] public GridMapData GridMapData;
        [SerializeField] public int TileScale = 32;

        private Dictionary<PlayerData, Vector2Int> _playerPositions;

        public bool HasReachedAllGoals() {
            foreach (var goal in Goals) {
                /*if (!goal.HasBeenReached) {
                    return false;
                }*/
            }
            return true;
        }

        public void Init(List<TGEPlayer> players) {
            _playerPositions = new Dictionary<PlayerData, Vector2Int>();
            for (int i = 0; i < players.Count; i++) {
                _playerPositions.Add(players[i].player.Data, GetPlayerStartPosition(i));
            }
        }

        /// <summary>
        /// Checks whether the given player is allowed to move in the direction given on the map
        /// </summary>
        /// <param name="player">The player that wants to move</param>
        /// <param name="direction">The direction the player wants to go from it's current position</param>
        /// <returns>Return true if the player can move in the direction. Returns false if there are any obstructions or other players on the destination</returns>
        public bool TryMoveInDirection(PlayerData player, CardinalDirection direction) {
            var directionVector = direction.ToVector2();

            // Get current player pos
            Vector2Int playerPos;
            if (!_playerPositions.TryGetValue(player, out playerPos))
            {
                Debug.Log(player.GetHashCode());
                Debug.Log($"Could not move player: Player does not have a position on the grid");
                return false; // current player does not have a position in this map
            }
            
            Debug.Log($"... Trying to move \"{direction.ToString().ToUpper()} {directionVector}\" from {playerPos} to cell ({playerPos.x + directionVector.x}, {playerPos.y + directionVector.y})");

            GridCell destination;
            if (!GridMapData.TryGetCell(playerPos.x + directionVector.x, playerPos.y + directionVector.y, out destination)) {
                Debug.Log($"Could not move player: Cell at ({destination.X}, {destination.Y}) does not exist/is out of bounds");
                return false;
            }

            // Check if there are any other players on the destination
            if (_playerPositions.Any(p => p.Key != player && p.Value == destination.XY)) {
                Debug.Log($"Could not move player: A player is standing on the destination");
                return false; // the destination contains a player
            }
            
            // Get the current player position on the map
            GridCell current;
            if (!GridMapData.TryGetCell(playerPos.x, playerPos.y, out current)) {
                Debug.Log($"Could not move player: The current player position ({playerPos.x}, {playerPos.y}) does not exist in the map");
                return false; // the current position of the player does not exist on the map
            }

            // Checks the current tile to see whether it can be exited from the direction
            var canLeaveCurrent = current.Value.CanExit(direction);
            // Checks the destination tile to see whether it can be entered/walked into from the given direction
            var canEnterDestination = destination.Value.IsWalkable(direction);

            // Final check whether the player can actually move
            var canMove = canLeaveCurrent && canEnterDestination;

            // Move player in grid
            if (canMove) {
                Debug.Log($"Can move to {destination.XY}");
                _playerPositions[player] = destination.XY;
            }

            return canMove;
        }

        public Vector2Int GetPlayerStartPosition(int playerNumber) {
            return GridMapData.PlayerStartPositions[playerNumber];
        }
    }
}