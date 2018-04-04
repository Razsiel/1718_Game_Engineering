using System;
using System.Collections;
using System.Collections.Generic;
using Assets.ScriptableObjects.Levels;
using UnityEngine;

public class LevelPresenter : MonoBehaviour
{

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
        for (int i = 0; i < gameManager.Players.Count; i++) {
            var startGridPosition = _levelData.GetPlayerStartPosition(i);
            var playerWorldPosition = GridHelper.GridToWorldPosition(_levelData, startGridPosition);
            playerWorldPosition.y = 1;
            //var playerDirection = player.viewDirection.ToQuaternion();
            var playerObject = Instantiate(gameManager.PlayerPrefab, playerWorldPosition, Quaternion.identity, this.transform);
            var playerComponent = playerObject.GetComponent<Player>();
            playerComponent.PlayerNumber = i;
            playerComponent.Data = gameManager.Players[i].player.Data;
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
                var tileObject = tile.GenerateGameObject(root, x, y);
                var transform = tileObject.transform;
                transform.position = GridHelper.GridToWorldPosition(data, new Vector2Int(x, y));
                transform.localScale /= data.TileScale;
            }
        }
        
        return root;
    }
}