using System;
using System.Collections;
using System.Collections.Generic;
using Assets.ScriptableObjects.Levels;
using UnityEngine;

public class LevelPresenter : MonoBehaviour {
    private LevelData _levelData;

    private static GameObject levelObject;

    // Use this for initialization
    void Start() {
        var gameManager = GameManager.GetInstance();
        _levelData = gameManager.LevelData;
        if (_levelData != null) {
            CreateGameObjectFromLevelData(_levelData, this.transform);
            
            //init players in level
            foreach (var player in gameManager.Players)
            {
                Instantiate(gameManager.PlayerPrefab, Vector3.zero, Quaternion.identity, this.transform);
            }
        }
    }

    public static GameObject CreateGameObjectFromLevelData(LevelData data, Transform parent = null, bool hideInHierarchy = false) {
        Destroy(levelObject);
        var grid = data.GridMapData;

        GameObject root = new GameObject("Level Object") {
            hideFlags = hideInHierarchy ? HideFlags.HideAndDontSave : HideFlags.NotEditable
        };
        root.transform.parent = parent;

        for (int y = 0; y < grid.Height; y++) {
            for (int x = 0; x < grid.Width; x++) {
                var tile = grid[x, y];
                if (tile == null) {
                    continue;
                }
                var tileObject = tile.GenerateGameObject(root);
                tileObject.name = $"Tile ({x}, {y})";
                var transform = tileObject.transform;
                transform.position = new Vector3(x - (grid.Width - 1) * 0.5f,
                                                 0,
                                                 y - (grid.Height - 1) * 0.5f);
                transform.localScale /= data.TileScale;
            }
        }

        levelObject = root;
        return root;
    }
}