using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using Assets.Scripts.DataStructures.Channel;
using UnityEngine.Assertions;
using UnityEngine;

public class LevelPresenter : MonoBehaviour
{
    private static GameObject levelObject;

    // Use this for initialization
    public static void Present(LevelData levelData, List<TGEPlayer> players) {
        var gameManager = GameManager.GetInstance();
        Assert.IsNotNull(gameManager);
        Assert.IsNotNull(levelData);
        Assert.IsNotNull(players);

        // create level objects in scene
        levelObject = CreateGameObjectFromLevelData(levelData, gameManager.transform);
        
        // Set players to start position in scene;
        for (int i = 0; i < players.Count; i++) {
            var playerStartPosition = levelData.GetPlayerStartPosition(i);
            var startGridPosition = playerStartPosition.StartPosition;
            var playerWorldPosition = GridHelper.GridToWorldPosition(levelData, startGridPosition);
            playerWorldPosition.y = 1;
            var player = players[i];
            player.Player.ViewDirection = playerStartPosition.Facing;
            player.PlayerObject.transform.position = playerWorldPosition;
            player.PlayerObject.transform.rotation = Quaternion.Euler(player.Player.ViewDirection.ToEuler());
        }
    }

    public static GameObject CreateGameObjectFromLevelData(LevelData data, Transform parent = null, bool hideInHierarchy = false) {
        Destroy(levelObject);
        var grid = data.GridMapData;

        GameObject root = new GameObject("Level Object") {
            hideFlags = hideInHierarchy ? HideFlags.HideAndDontSave : HideFlags.NotEditable
        };
        root.transform.parent = parent;

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                var tileConfiguration = grid[x, y];
                if (tileConfiguration == null) {
                    continue;
                }
                var tileObject = tileConfiguration.GenerateGameObject(root, x, y);
                var transform = tileObject.transform;
                transform.position = GridHelper.GridToWorldPosition(data, new Vector2Int(x, y));
                transform.localScale /= data.TileScale;
            }
        }
        
        return root;
    }
}