using System;
using System.Collections;
using System.Collections.Generic;
using Assets.ScriptableObjects.Levels;
using UnityEngine;

public class LevelPresenter : MonoBehaviour {
    [SerializeField] private LevelData _levelData;

    private static GameObject levelObject;

    // Use this for initialization
    void Start() {
        if (_levelData != null) {
            CreateGameObjectFromLevelData(_levelData);
        }
    }

    public static GameObject CreateGameObjectFromLevelData(LevelData data, bool hideInHierarchy = false) {
        Destroy(levelObject);
        var grid = data.GridMapData;

        GameObject root = new GameObject("Level Object") {
            hideFlags = hideInHierarchy ? HideFlags.HideAndDontSave : HideFlags.NotEditable
        };

        for (int y = 0; y < grid.Height; y++) {
            for (int x = 0; x < grid.Width; x++) {
                var tile = grid[x, y];
                if (tile == null) {
                    continue;
                }
                var tileObject = new GameObject($"Tile ({x}, {y})",
                                                typeof(MeshFilter),
                                                typeof(MeshRenderer)) {
                    hideFlags = hideInHierarchy ? HideFlags.HideAndDontSave : HideFlags.NotEditable
                };
                var meshFilter = tileObject.GetComponent<MeshFilter>();
                if (meshFilter != null) {
                    meshFilter.mesh = tile.TileMesh;
                }
                var meshRenderer = tileObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null) {
                    meshRenderer.sharedMaterial = tile.TileMaterial;
                }
                var transform = tileObject.transform;
                transform.parent = root.transform;
                transform.position = new Vector3(x - (grid.Width - 1) * 0.5f,
                                                 0,
                                                 y - (grid.Height - 1) * 0.5f);
                transform.localScale /= 32;
            }
        }

        levelObject = root;
        return root;
    }
}