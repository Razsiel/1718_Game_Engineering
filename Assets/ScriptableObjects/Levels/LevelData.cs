using System.Collections.Generic;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEngine;

namespace Assets.ScriptableObjects.Levels {
    [CreateAssetMenu(fileName = "Level_0", menuName = "Data/Level")]
    public class LevelData : ScriptableObject {
        public string Name;
        public GridMap<TileData> GridMap;
        public Texture2D BackgroundImage;
        public List<string> Goals; // convert to data type later containing additional variables

        public bool HasReachedAllGoals() {
            foreach (var goal in Goals) {
                /*if (!goal.HasBeenReached) {
                    return false;
                }*/
            }
            return true;
        }
        
        public int Width {
            get { return GridMap.Width; }
            set { GridMap = new GridMap<TileData>(value, GridMap.Height); }
        }
        
        public int Height {
            get { return GridMap.Height; }
            set { GridMap = new GridMap<TileData>(GridMap.Width, value); }
        }
    }
}