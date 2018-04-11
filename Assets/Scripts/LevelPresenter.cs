using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Data.Levels;
using Assets.Scripts.DataStructures;
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
        //Assert.IsEmpty is not an existing one
        //Assert.IsNo(players);

        // create level objects in scene
        levelObject = CreateGameObjectFromLevelData(levelData, gameManager.transform);
        
        // Set players to start position in scene;
        for (int i = 0; i < players.Count; i++) {
            var startGridPosition = levelData.GetPlayerStartPosition(i);
            var playerWorldPosition = GridHelper.GridToWorldPosition(levelData, startGridPosition);
            playerWorldPosition.y = 1;
            //var playerDirection = player.viewDirection.ToQuaternion();
            players[i].PlayerObject.transform.position = playerWorldPosition;
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