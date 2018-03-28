﻿using System;
using System.Linq;
using UnityEngine;

namespace Assets.ScriptableObjects.Tiles {
    [Serializable]
    [CreateAssetMenu(menuName = "Data/Tiles/Tile")]
    public class TileData : ScriptableObject {
        [SerializeField] public Mesh TileMesh;
        [SerializeField] public TileDecorationData[] DecorationsData;
        [SerializeField] public Material TileMaterial;

        //private event OnPlayerTileEnter;
        //private event OnPlayerTileLeave;

        public virtual bool IsWalkable() {
            return DecorationsData?.All(d => d.IsWalkable()) ?? false;
        }

        public GameObject GenerateGameObject(GameObject parent, bool hidden = false) {
            return GenerateGameObject(parent.transform, hidden);
        }

        public GameObject GenerateGameObject(Transform parent, bool hidden = false) {
            var tile = new GameObject("Tile",
                                      typeof(MeshFilter),
                                      typeof(MeshRenderer)) {
                hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };

            tile.transform.parent = parent;

            // Fill in components
            var meshFilter = tile.GetComponent<MeshFilter>();
            if (meshFilter != null) {
                meshFilter.mesh = this.TileMesh;
            }
            var meshRenderer = tile.GetComponent<MeshRenderer>();
            if (meshRenderer != null) {
                meshRenderer.sharedMaterial = this.TileMaterial;
            }

            return tile;
        }
    }
}