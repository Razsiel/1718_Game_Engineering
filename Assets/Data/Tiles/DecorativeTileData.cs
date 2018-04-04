using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Tiles {
    [CreateAssetMenu(menuName = "Data/Tiles/Decorative Tile")]
    public class DecorativeTileData : TileData {
        public override bool IsWalkable(CardinalDirection direction) {
            return true;
        }
    }
}