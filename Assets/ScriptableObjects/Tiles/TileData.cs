using System.Linq;
using UnityEngine;

namespace Assets.ScriptableObjects.Tiles {
    [CreateAssetMenu(menuName = "Data/Tiles/Tile")]
    public class TileData : ScriptableObject {
        public Mesh TileMesh;
        public TileDecoration[] Decorations;
        public Material TileMaterial;

        public bool IsWalkable() {
            return Decorations?.All(d => d.IsWalkable()) ?? false;
        }
    }
}