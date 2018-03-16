using System.Collections.Generic;
using Assets.ScriptableObjects.Grids;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEngine;

namespace Assets.ScriptableObjects.Levels {
    [CreateAssetMenu(fileName = "Level_0", menuName = "Data/Level")]
    [System.Serializable]
    public class LevelData : ScriptableObject {
        [SerializeField] public string Name;
        [SerializeField] public Texture2D BackgroundImage;
        [SerializeField] public List<string> Goals; // convert to data type later containing additional variables
        [SerializeField] public GridMapData GridMapData;

        public bool HasReachedAllGoals() {
            foreach (var goal in Goals) {
                /*if (!goal.HasBeenReached) {
                    return false;
                }*/
            }
            return true;
        }
    }
}