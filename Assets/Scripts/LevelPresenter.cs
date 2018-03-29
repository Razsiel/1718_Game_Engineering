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
        if (_levelData == null)
            return;

        // create level objects in scene
        levelObject = CreateGameObjectFromLevelData(_levelData, this.transform);
            
        // create player objects in scene
        foreach (var player in gameManager.Players)
        {
            /*TODO: 
             * Get player grid start position
             * Transform grid position to world position
             */
            var startGridPosition = _levelData.GetPlayerStartPosition(player);
            var playerWorldPosition = GridHelper.GridToWorldPosition(_levelData, startGridPosition);
            //var playerDirection = player.viewDirection.ToQuaternion();
            Instantiate(gameManager.PlayerPrefab, playerWorldPosition, Quaternion.identity, this.transform);
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
                transform.position = GridHelper.GridToWorldPosition(data, new Vector2Int(x, y));
                transform.localScale /= data.TileScale;
            }
        }
        
        return root;
    }
}

public class GridHelper {
    public static Vector3 GridToWorldPosition(LevelData levelData, Vector2Int gridPosition) {
        var grid = levelData.GridMapData;
        var worldPos = new Vector3(gridPosition.x - (grid.Width - 1) * 0.5f,
                                  0,
                                  gridPosition.y - (grid.Height - 1) * 0.5f);
        return worldPos;
    }
}