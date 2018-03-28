using UnityEngine;

namespace Assets.ScriptableObjects.Tiles {
    [CreateAssetMenu(menuName = "Data/Tiles/Decorative Tile")]
    public class DecorativeTileData : TileData {
        public override bool IsWalkable() {
            return false;
        }
    }
}