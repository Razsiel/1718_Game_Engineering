using System.Linq;
using UnityEngine;

namespace Assets.ScriptableObjects.Tiles {
    [CreateAssetMenu(menuName = "Data/Tiles/Tile")]
    public class TileData : ScriptableObject {
        public Mesh TileMesh;
        public TileDecorationData[] DecorationsData;
        public Material TileMaterial;
		
		//private event OnPlayerTileEnter;
		//private event OnPlayerTileLeave;

        public bool IsWalkable() {
            return DecorationsData?.All(d => d.IsWalkable()) ?? false;
        }
    }
}