using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Goal;
using Assets.Data.Grids;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Photon;
using Assets.Scripts.Photon.Level;
using UnityEngine.Assertions;
using UnityEngine;

public class LevelManager : TGEMonoBehaviour {
    [SerializeField]
    public GameObject PlayerPrefab;
    private GameObject _levelObject;

    public override void Awake() {
        EventManager.OnLoadLevel += Present;
        EventManager.OnLevelReset += OnLevelReset;
    }

    private void OnDestroy() {
        EventManager.OnLevelReset -= OnLevelReset;
    }

    private void OnLevelReset(GameInfo gameInfo, List<Player> players) {
        print($"{nameof(LevelManager)}: Delegating reset level");
        // reset internal data
        gameInfo.Level.Reset(players, ResetPlayers);
    }

    // Use this for initialization
    public void Present(GameInfo gameInfo) {
        EventManager.OnLoadLevel -= Present;
        Assert.IsNotNull(gameInfo);
        var levelData = gameInfo.Level;
        var players = gameInfo.Players;
        Assert.IsNotNull(PlayerPrefab);
        Assert.IsNotNull(levelData);
        Assert.IsNotNull(players);
        Assert.IsTrue(players.Any());
        
        // Create level objects in scene
        _levelObject = PresentLevel(levelData, players, this.transform);
        Assert.IsNotNull(_levelObject);
        
        EventManager.LevelLoaded(gameInfo);
    }

    public GameObject PresentLevel(LevelData data, List<TGEPlayer> players, Transform parent = null, bool hideInHierarchy = false) {
        Destroy(_levelObject);

        data.Init();
        var grid = data.GridMapData;

        GameObject root = new GameObject("Level Object") {
            hideFlags = hideInHierarchy ? HideFlags.HideAndDontSave : HideFlags.NotEditable
        };
        root.transform.parent = parent;

        // select all locationgoals from the level
        var locationGoals = data.Goals.OfType<LocationGoalData>().ToList();

        // Create tile objects
        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                var tileConfiguration = grid[x, y];
                if (tileConfiguration == null) {
                    continue;
                }

                var tilePos = new Vector2Int(x, y);
                var tileObject = tileConfiguration.Present(root, x, y);
                var tileTransform = tileObject.transform;
                tileTransform.position = GridHelper.GridToWorldPosition(data, tilePos);

                // Create any locationgoals on the current tile
                var goalsOnTile = locationGoals.Where(g => g.TargetGridPosition.Any(p => p.Location == tilePos));
                foreach (var goalData in goalsOnTile) {
                    var goalObject = goalData.Present(tileObject.transform, hideInHierarchy, name: "GoalDecoration");
                    goalObject.transform.position = Vector3.up * (data.TileScale / 2f);
                    var playerGoal = goalData.TargetGridPosition.First(p => p.Location == tilePos);
                    var colorBehaviour = goalObject.GetComponent<PlayerGoalMaterialBehaviour>();
                    if (colorBehaviour != null) {
                        colorBehaviour.PlayerNumber = playerGoal.PlayerNumber;
                    }
                }
                
                tileTransform.localScale /= data.TileScale;
            }
        }

        // Create player objects and set players to start position in scene;
        for (int i = 0; i < players.Count; i++)
        {
            var playerObject = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, parent);
            var playerComponent = playerObject.GetComponent<Player>();
            var playerAnimation = playerObject.GetComponent<PlayerAnimationBehaviour>();
            players[i].Player = playerComponent;
            playerComponent.PlayerNumber = i;

            playerAnimation.OnPlayerSpawned(playerComponent);
            EventManager.PlayerSpawned(playerComponent);

            var playerPos = data.InitPlayer(playerComponent);
            PresentPlayerOnPosition(data, playerComponent, playerPos);
        }

        return root;
    }

    private void PresentPlayerOnPosition(LevelData levelData, Player player, PlayerStartPosition playerStartPosition)
    {
        var playerWorldPosition = GridHelper.GridToWorldPosition(levelData, playerStartPosition.StartPosition);
        playerWorldPosition.y = 1;
        player.ViewDirection = playerStartPosition.Facing;
        player.transform.position = playerWorldPosition;
        player.transform.rotation = Quaternion.Euler(player.ViewDirection.ToEuler());
    }

    private void ResetPlayers(List<Player> players, LevelData levelData)
    {
        foreach (var player in players)
        {
            player.Reset();
            PresentPlayerOnPosition(levelData, player, levelData.GetPlayerStartPosition(player.PlayerNumber));
        }
    }
}