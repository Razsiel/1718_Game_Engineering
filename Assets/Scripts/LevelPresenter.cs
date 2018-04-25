using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Grids;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using Assets.Scripts.DataStructures.Channel;
using UnityEngine.Assertions;
using UnityEngine;

public class LevelPresenter : MonoBehaviour {
    [SerializeField]
    private GameObject _playerPrefab;

    private static GameObject _levelObject;

    public void Awake() {
        //EventManager.LoadLevel += Present;
        EventManager.OnLevelReset += (levelData, players) => {
            // reset internal data
            levelData.Reset(players, ResetPlayers);
            // reset world representation
            int playerIndex = 0;
            foreach (var player in players) {
                var playerPos = levelData.GetPlayerStartPosition(playerIndex);
                PresentPlayerOnPosition(levelData, player, playerPos);
                playerIndex++;
            }
        };

    }

    // Use this for initialization
    public static void Present(LevelData levelData, List<TGEPlayer> players) {
        Assert.IsNotNull(levelData);
        Assert.IsNotNull(players);
        Assert.IsTrue(players.Any());

        EventManager.LoadLevel(levelData, players);

        // create level objects in scene
        _levelObject = CreateGameObjectFromLevelData(levelData);
        Assert.IsNotNull(_levelObject);
        
        // Set players to start position in scene;
        for (int i = 0; i < players.Count; i++) {
            var playerObject =  players[i].PlayerObject; //Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity, this.transform);
            var player = playerObject.GetComponent<Player>();
            player.PlayerNumber = i;
            var playerPos = levelData.InitPlayer(player);
            PresentPlayerOnPosition(levelData, player, playerPos);
        }

        EventManager.LevelLoaded(levelData);
    }

    private static void PresentPlayerOnPosition(LevelData levelData, Player player, PlayerStartPosition playerStartPosition)
    {
        var playerWorldPosition = GridHelper.GridToWorldPosition(levelData, playerStartPosition.StartPosition);
        playerWorldPosition.y = 1;
        player.ViewDirection = playerStartPosition.Facing;
        player.transform.position = playerWorldPosition;
        player.transform.rotation = Quaternion.Euler(player.ViewDirection.ToEuler());
    }

    private void ResetPlayers(List<Player> players, LevelData levelData) {
        foreach (var player in players) {
            PresentPlayerOnPosition(levelData, player, levelData.GetPlayerStartPosition(player.PlayerNumber));
        }
    }

    public static GameObject CreateGameObjectFromLevelData(LevelData data, Transform parent = null, bool hideInHierarchy = false) {
        Destroy(_levelObject);
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