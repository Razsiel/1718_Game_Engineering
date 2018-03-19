using System;
using System.Linq;
using UnityEngine;

namespace Assets.ScriptableObjects.Tiles {
    [Serializable]
    [CreateAssetMenu(menuName = "Data/Tiles/Tile")]
    public class TileData : ScriptableObject {
        [SerializeField]
        public Mesh TileMesh;
        [SerializeField]
        public TileDecorationData[] DecorationsData;
        [SerializeField]
        public Material TileMaterial;
		
		//private event OnPlayerTileEnter;
		//private event OnPlayerTileLeave;

        public bool IsWalkable() {
            return DecorationsData?.All(d => d.IsWalkable()) ?? false;
        }
    }
}