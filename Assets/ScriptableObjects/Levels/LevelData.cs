using System.Collections.Generic;
using Assets.ScriptableObjects.Grids;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
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

        public bool HasReachedAllGoals() {
            foreach (var goal in Goals) {
                /*if (!goal.HasBeenReached) {
                    return false;
                }*/
            }
            return true;
        }

        public bool CanMoveToTileFromDirection(CardinalDirection direction) {
            
            /*var toX = 1;
            var toY = 0;

            if (GridMapData.IsValidTile(toX, toY)) {
                var toTile = GridMapData[toX, toY];
            }*/

            return true;
        }
    }
}