using System;
using System.Linq;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Tiles {
    [Serializable]
    [CreateAssetMenu(menuName = "Data/Tiles/Tile")]
    public class TileData : ScriptableObject {
        [SerializeField] public Mesh Mesh;
        [SerializeField] public Material Material;

        //private event OnPlayerTileEnter;
        //private event OnPlayerTileLeave;
        
        public GameObject GenerateGameObject(GameObject parent, bool hidden = false) {
            return GenerateGameObject(parent.transform, hidden);
        }

        public GameObject GenerateGameObject(Transform parent, bool hidden = false) {
            var tile = new GameObject($"Tile",
                                      typeof(MeshFilter),
                                      typeof(MeshRenderer)) {
                hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };

            tile.transform.parent = parent;

            // Fill in components
            var meshFilter = tile.GetComponent<MeshFilter>();
            if (meshFilter != null) {
                meshFilter.mesh = this.Mesh;
            }
            var meshRenderer = tile.GetComponent<MeshRenderer>();
            if (meshRenderer != null) {
                meshRenderer.sharedMaterial = this.Material;
            }
            
            return tile;
        }
    }
}